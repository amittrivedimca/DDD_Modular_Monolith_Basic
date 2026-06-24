# GenerateDddEntityFromTableSchema

Generate a complete set of DDD domain files from a SQL Server table schema pasted in the following tabular format:

```
ColumnName    DataType          Nullable
Id            uniqueidentifier  Unchecked
UserId        uniqueidentifier  Checked
CreatedDate   datetime          Checked
Status        char(20)          Checked
```

Each row contains three tab-separated or whitespace-separated columns:

# GenerateDddEntityFromTableSchema

Generate a complete set of DDD domain files from a SQL Server table schema pasted in the following tabular format:

```
ColumnName    DataType          Nullable
Id            uniqueidentifier  Unchecked
UserId        uniqueidentifier  Checked
CreatedDate   datetime          Checked
Status        char(20)          Checked
```

Each row contains three tab-separated or whitespace-separated columns:

1. **ColumnName** – the SQL column name (PascalCase assumed; convert if snake_case)
2. **DataType** – the SQL Server data type (e.g. `uniqueidentifier`, `nvarchar(200)`, `datetime`, `bit`)
3. **Nullable** – `Checked` means the column is nullable; `Unchecked` means NOT NULL

Ignore any empty rows at the end of the table.

---

## SQL-to-C# Type Mapping

| SQL Server type                                                          | C# type                          | EF Core annotation                         |
| ------------------------------------------------------------------------ | -------------------------------- | ------------------------------------------ |
| `uniqueidentifier` (Id column)                                           | typed `ValueObject` ID           | `HasConversion(...)` in config             |
| `uniqueidentifier` (FK, name ends with `Id`)                             | typed `ValueObject` ID or `Guid` | `HasConversion(...)` if ValueObject exists |
| `int`                                                                    | `int`                            | —                                          |
| `bigint`                                                                 | `long`                           | —                                          |
| `smallint`                                                               | `short`                          | —                                          |
| `tinyint`                                                                | `byte`                           | —                                          |
| `bit`                                                                    | `bool`                           | —                                          |
| `decimal(p,s)` / `numeric(p,s)` / `money` / `smallmoney`                 | `decimal`                        | `[Precision(p, s)]`                        |
| `float` / `real`                                                         | `double`                         | —                                          |
| `datetime` / `datetime2` / `smalldatetime`                               | `DateTime`                       | —                                          |
| `date`                                                                   | `DateOnly`                       | —                                          |
| `time`                                                                   | `TimeOnly`                       | —                                          |
| `datetimeoffset`                                                         | `DateTimeOffset`                 | —                                          |
| `char(n)` / `nchar(n)` / `varchar(n)` / `nvarchar(n)` / `text` / `ntext` | `string`                         | `[MaxLength(n)]`                           |
| `varbinary` / `binary` / `image`                                         | `byte[]`                         | —                                          |

Apply `?` suffix for every C# property that maps to a Nullable (`Checked`) column, **except** for `string` and `byte[]` which are already reference types (use nullable annotation `string?` / `byte[]?` for nullable columns).

---

## Files to Generate

Given a table named **`<TableName>`** belonging to **`<ModuleName>`** module, generate the following **five** files:

---

### 1. Typed ID Value Object — `<ModuleName>.Domain/ValueObjects/<TableName>Id.cs`

```csharp
using SharedKernel;

namespace <ModuleName>.Domain.ValueObjects;

public sealed class <TableName>Id : ValueObject
{
	public Guid Id { get; }

	private <TableName>Id(Guid value) { Id = value; }

	public static <TableName>Id CreateUnique() => new(Guid.NewGuid());

	public static <TableName>Id Create(Guid? value)
	{
		if (value == null || value == Guid.Empty)
			throw new ArgumentException("<TableName> ID cannot be empty.", nameof(value));
		return new <TableName>Id(value.Value);
	}

	protected override IEnumerable<object> GetEqualityComponents() { yield return Id; }

	public override string ToString() => Id.ToString();
}
```

---

### 2. Domain Events — `<ModuleName>.Domain/Events/<TableName>DomainEvents.cs`

Emit one event per aggregate lifecycle verb: `Created` and `Updated`.

> **Skip this file** if the entity is a child entity (not an aggregate root).

```csharp
using <ModuleName>.Domain.ValueObjects;
using SharedKernel;

namespace <ModuleName>.Domain.Events;

public record <TableName>CreatedDomainEvent(<TableName>Id <TableName>Id) : IDomainEvent
{
	public Guid EventId { get; } = Guid.NewGuid();
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record <TableName>UpdatedDomainEvent(<TableName>Id <TableName>Id) : IDomainEvent
{
	public Guid EventId { get; } = Guid.NewGuid();
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
```

---

### 3. Entity / Aggregate Root — `<ModuleName>.Domain/Entities/<TableName>.cs`

Rules:

- **Default to `AggregateRoot<<TableName>Id>`**. If the user specifies a child entity, use `Entity<<TableName>Id>` instead and omit `RaiseDomainEvent` calls and the domain events file.
- Every non-Id column becomes a public property with a **private setter** and appropriate data annotations (`[MaxLength(n)]`, `[Precision(p,s)]`).
- A foreign-key column (name ends with `Id`, type `uniqueidentifier`) maps to the corresponding typed ValueObject ID if it exists in the module, otherwise `Guid`.
- Validation lives in `private Set<PropertyName>(...)` methods; use `ArgumentException.ThrowIfNullOrWhiteSpace` for strings and null-guard checks for value types.
- Include `OnCreated()` and `OnUpdated()` methods that call `RaiseDomainEvent(...)` for aggregate roots.
- Mark the class `sealed`.
- Include a private parameterless constructor for EF Core.

```csharp
using <ModuleName>.Domain.Events;
using <ModuleName>.Domain.ValueObjects;
using SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace <ModuleName>.Domain.Entities;

public sealed class <TableName> : AggregateRoot<<TableName>Id>
{
	// --- properties (one per non-Id column) ---
	[MaxLength(n)]
	public <CSharpType> <ColumnName> { get; private set; } = <default>;
	// ... repeat ...

	// EF Core parameterless constructor
	private <TableName>() { }

	private <TableName>(<TableName>Id id, <required-params>)
	{
		Id = id;
		Set<Property1>(<param1>);
		// ... repeat for required (NOT NULL) columns ...
	}

	public static <TableName> CreateNew(<required-params>) =>
		new(<TableName>Id.CreateUnique(), <required-params>);

	public static <TableName> CreateWithId(Guid id, <required-params>) =>
		new(<TableName>Id.Create(id), <required-params>);

	public void OnCreated() => RaiseDomainEvent(new <TableName>CreatedDomainEvent(Id));
	public void OnUpdated() => RaiseDomainEvent(new <TableName>UpdatedDomainEvent(Id));

	// --- private setters for validation ---
	private void Set<Property1>(<type> value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value); // for strings
		<Property1> = value;
	}
}
```

---

### 4. Repository Interface — `<ModuleName>.Domain/Repositories/I<TableName>Repository.cs`

```csharp
using <ModuleName>.Domain.Entities;

namespace <ModuleName>.Domain.Repositories;

public interface I<TableName>Repository
{
	Task<IReadOnlyCollection<<TableName>>> GetAll();
	Task<<TableName>?> GetById(Guid id);
	Task Add(<TableName> entity);
	Task Update(<TableName> entity);
	Task Delete(Guid id);
}
```

---

### 5. EF Core Configuration — `<ModuleName>.Infrastructure/Persistance/<TableName>Configuration.cs`

```csharp
using <ModuleName>.Domain.Entities;
using <ModuleName>.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace <ModuleName>.Infrastructure.Persistance;

public sealed class <TableName>Configuration : IEntityTypeConfiguration<<TableName>>
{
	public void Configure(EntityTypeBuilder<<TableName>> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Id)
			.HasConversion(
				id => id.Id,
				id => <TableName>Id.Create(id));

		// string column example:
		builder.Property(e => e.<StringColumn>)
			.HasMaxLength(n)
			.IsRequired(); // .IsRequired(false) for nullable columns

		// decimal column example:
		builder.Property(e => e.<DecimalColumn>)
			.HasPrecision(p, s)
			.IsRequired(false);
	}
}
```

---

## Step-by-Step Instructions

1. **Ask** for the module name (e.g. `Cart`, `Catalog`) and entity/table name if not already provided.
2. **Ask** if this is an aggregate root or a child entity (default: **aggregate root**).
3. **Parse** each non-empty row: extract `ColumnName`, `DataType`, `Nullable` flag.
4. **Map** each SQL type to a C# type using the table above. Apply `?` for nullable columns.
5. **Identify** foreign-key columns (name ends with `Id`, type `uniqueidentifier`); reference typed IDs where possible.
6. **Determine** required (NOT NULL, non-Id) columns — these become constructor parameters.
7. **Generate** all five files in order: ID ValueObject → Domain Events → Entity → Repository Interface → EF Configuration.
8. **Follow** all conventions from `.github/copilot-instructions.md` (sealed classes, private constructors, `ArgumentException.ThrowIfNullOrWhiteSpace`, `IReadOnlyCollection<T>`, no EF/MediatR in Domain).

> **Child entity override:** If the user says the entity is a child (not a root), skip the Domain Events file, inherit from `Entity<TId>`, and omit `OnCreated()` / `OnUpdated()`.

---

## Quality Checklist

After generation, verify:

- ✅ Entity inherits from `AggregateRoot<TId>` (or `Entity<TId>` for child entities)
- ✅ ID is extracted into a sealed `ValueObject` class with `CreateUnique()`, `Create(Guid?)`, and `ToString()`
- ✅ All non-ID properties have correct nullability (`?` for Checked columns)
- ✅ Data annotations applied on entity properties (`[MaxLength(n)]`, `[Precision(p,s)]`)
- ✅ `private Set<Property>()` methods with `ArgumentException.ThrowIfNullOrWhiteSpace` guards
- ✅ Private parameterless constructor present for EF Core
- ✅ Static factory methods `CreateNew(...)` and `CreateWithId(...)`
- ✅ `OnCreated()` / `OnUpdated()` raising domain events (aggregate roots only)
- ✅ Repository interface uses `IReadOnlyCollection<T>` and no `IQueryable<T>`
- ✅ EF Configuration handles ValueObject ID conversion and per-property constraints (`HasMaxLength`, `HasPrecision`, `IsRequired`)
- ✅ File locations match module structure: `{Module}.Domain/...`, `{Module}.Infrastructure/Persistance/`

---

## Example

**Input:**

```
Id            uniqueidentifier  Unchecked
UserId        uniqueidentifier  Checked
CreatedDate   datetime          Checked
Status        char(20)          Checked
```

**Module:** `Cart` **Entity name:** `Order` **Type:** Aggregate Root

**Generated files:**

- `Cart.Domain/ValueObjects/OrderId.cs`
- `Cart.Domain/Events/OrderDomainEvents.cs`
- `Cart.Domain/Entities/Order.cs`
- `Cart.Domain/Repositories/IOrderRepository.cs`
- `Cart.Infrastructure/Persistance/OrderConfiguration.cs`

The `Order` entity will have:

- `Id` → `OrderId` (aggregate identity)
- `UserId` → `Guid? UserId` (nullable; if a `UserId` ValueObject exists in the module, use that type instead)
- `CreatedDate` → `DateTime? CreatedDate`
- `Status` → `[MaxLength(20)] string? Status`

---

## Related Files

- [copilot-instructions.md](../copilot-instructions.md) — DDD rules and architecture constraints
- `SharedKernel/` — Base types: `Entity<TId>`, `AggregateRoot<TId>`, `ValueObject`, `IDomainEvent`
