# GitHub Copilot Instructions

## Project Overview

This is a **DDD Modular Monolith** built with **.NET 10** and **ASP.NET Core Web API**.  
The solution models a shopping-cart system and is organized into self-contained vertical modules (`Catalog`, `Cart`) backed by a shared `SharedKernel`.

---

## Solution Structure

```
Src/DDD_Modular_Monolith_Basic/
├── MM.CatalogAPI/                          # ASP.NET Core entry-point (host)
├── Shared/
│   └── SharedKernel/                       # Base types shared across all modules
└── Modules/
	├── Catalog/
	│   ├── Catalog.Domain/                 # Entities, ValueObjects, Events, Repository interfaces
	│   ├── Catalog.Application/            # Services, DTOs, Event handlers, DI registration
	│   └── Catalog.Infrastructure/         # EF Core DbContext, Configurations, Repositories, Interceptors
	└── Cart/
		├── Cart.Domain/
		├── Cart.Application/
		└── Cart.Infrastructure/
```

Each module is **independently deployable** — do not create cross-module project references.  
Modules may only communicate via **domain events** (MediatR) or a **shared abstraction** placed in `SharedKernel`.

---

## Architecture & DDD Rules

### Layers (per module)
| Layer | Responsibility |
|---|---|
| **Domain** | Entities, Aggregates, Value Objects, Domain Events, Repository interfaces |
| **Application** | Application services, DTOs (`record`), Event handlers, DI extension |
| **Infrastructure** | EF Core, Repositories, Configurations, Interceptors |

### Key Constraints
- **Domain** projects have **zero infrastructure dependencies** (no EF Core, no HTTP, no MediatR).
- **Application** projects depend only on their own **Domain**.
- **Infrastructure** projects depend on their own **Domain** and **Application**.
- The **host** (`MM.CatalogAPI`) wires everything together via module DI extensions.

---

## SharedKernel Base Types

Always derive from these types — never re-implement them.

| Base Type | When to use |
|---|---|
| `Entity<TId>` | Any domain entity with an identity |
| `AggregateRoot<TId>` | Root entities that own domain events |
| `ValueObject` | Immutable value types (IDs, Money, Address, …) |
| `IDomainEvent` | Marker for domain events |
| `IHasDomainEvent` | Aggregates that raise events (already satisfied by `AggregateRoot<TId>`) |

---

## Coding Conventions

### Entities & Aggregates
- Inherit from `AggregateRoot<TId>` for aggregate roots; `Entity<TId>` for child entities.
- Use **private constructors** + `static` factory methods (`CreateNew`, `CreateWithId`).
- Validate invariants inside `private` setter methods (e.g., `SetName`).
- Mark aggregate root classes `sealed` unless designed for inheritance.
- Raise domain events by calling `RaiseDomainEvent(...)` — never dispatch directly.

```csharp
public sealed class Category : AggregateRoot<CategoryId>
{
	private Category(CategoryId id, string name) { ... }

	public static Category CreateNew(string name) => new(CategoryId.CreateUnique(), name);
}
```

### Value Objects (Typed IDs)
- Always wrap primitive IDs in a `ValueObject` subclass (e.g., `CategoryId`, `ProductId`).
- Expose `CreateUnique()` (new Guid) and `Create(Guid?)` (from existing) factory methods.
- Override `GetEqualityComponents()` to yield the wrapped value.

```csharp
public sealed class OrderId : ValueObject
{
	public Guid Id { get; }
	private OrderId(Guid value) { Id = value; }
	public static OrderId CreateUnique() => new(Guid.NewGuid());
	public static OrderId Create(Guid? value) => new(value ?? throw new ArgumentException(...));
	protected override IEnumerable<object> GetEqualityComponents() { yield return Id; }
}
```

### Domain Events
- Implement `IDomainEvent` (also implement `INotification` for MediatR dispatch).
- Name events in the past tense: `CategoryCreatedDomainEvent`, `ProductPriceChangedDomainEvent`.
- Raise inside aggregate methods, not from outside the aggregate.

### DTOs
- Use C# `sealed record` types for all DTOs in the Application layer.
- Place them in `<Module>.Application/DTOs/`.

```csharp
public sealed record CategoryInfo(Guid? CategoryId, string Name, Photo? Image);
```

### Application Services
- Injected via constructor with repository interfaces.
- Return DTOs — never return domain entities out of the Application layer.
- Keep service methods `async Task<T>`.

### Repository Interfaces
- Defined in the **Domain** layer (`<Module>.Domain/Repositories/`).
- Implemented in the **Infrastructure** layer.
- Use simple, intention-revealing method names: `GetAll`, `GetById`, `Add`, `Update`, `Delete`.

### EF Core & DbContext
- One `DbContext` per module (e.g., `CatalogDBContext`).
- Apply all EF configurations via `ApplyConfigurationsFromAssembly` in `OnModelCreating`.
- Register each module's migrations under its own schema (`MigrationsHistoryTable("__EFMigrationsHistory", "<module>")`).
- Domain events are dispatched **after** `SaveChangesAsync` via `DomainEventDispatcherInterceptor`.

### Dependency Injection
- Each module exposes exactly **two** DI extension methods on `IServiceCollection`:
  - `Add<Module>Application(IConfiguration)` — in the Application project.
  - `Add<Module>Infrastructure(IConfiguration)` — in the Infrastructure project.
- Register these in the host `Program.cs` only — not inside modules themselves.

### API Layer (Host)
- Use **MVC Controllers** (no Minimal API endpoints unless explicitly requested).
- API documentation via **Scalar** (`/scalar/`) + `Microsoft.AspNetCore.OpenApi`.
- Controllers live in `MM.CatalogAPI/Controllers/`.

---

## Naming Conventions

| Concept | Convention | Example |
|---|---|---|
| Aggregate root | `sealed class`, PascalCase | `Cart`, `Category` |
| Entity | `class`, PascalCase | `CartItem` |
| Value Object | `sealed class : ValueObject` | `CartId`, `Money` |
| Domain Event | `…DomainEvent` record/class | `CategoryCreatedDomainEvent` |
| Event Handler | `…DomainEventHandler` | `CategoryCreatedDomainEventHandler` |
| Repository interface | `I…Repository` | `ICategoryRepository` |
| Repository impl | `…Repository` | `CategoryRepository` |
| Application service interface | `I…Service` | `IProductService` |
| Application service | `…Service` | `ProductService` |
| DTO | `sealed record`, `…Info` / `…Request` / `…Response` | `CategoryInfo`, `CreateCategoryRequest` |
| DbContext | `…DBContext` | `CatalogDBContext` |
| EF Configuration | `…Configuration : IEntityTypeConfiguration<T>` | `CategoryConfiguration` |

---

## Technology Stack

| Concern | Library / Approach |
|---|---|
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core (SQL Server) |
| Domain event bus | MediatR (`INotification` / `INotificationHandler`) |
| API documentation | Scalar + `Microsoft.AspNetCore.OpenApi` |
| Nullable reference types | Enabled (`<Nullable>enable</Nullable>`) |
| Implicit usings | Enabled |

---

## Do's and Don'ts

**Do:**
- Keep domain logic inside aggregates and entities — no anemic domain model.
- Use `ArgumentException.ThrowIfNullOrWhiteSpace` and similar guard helpers for input validation.
- Clear domain events after dispatch (`ClearDomainEvents()`).
- Use `IReadOnlyCollection<T>` for exposing internal collections from aggregates.
- Prefer `sealed` on leaf classes (handlers, interceptors, value objects).

**Don't:**
- Don't reference EF Core, MediatR, or any infrastructure concern from the Domain layer.
- Don't expose `IQueryable<T>` from repositories — return materialized collections or single entities.
- Don't add cross-module project references — use events or a shared abstraction instead.
- Don't return domain entities from Application services — map to DTOs.
- Don't put business logic in controllers or application services — delegate to the domain.
