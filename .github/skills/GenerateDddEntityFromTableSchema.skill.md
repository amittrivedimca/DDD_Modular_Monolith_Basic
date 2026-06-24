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
2. **DataType**   – the SQL Server data type (e.g. `uniqueidentifier`, `nvarchar(200)`, `datetime`, `bit`)
3. **Nullable**   – `Checked` means the column is nullable; `Unchecked` means NOT NULL

Ignore any empty rows at the end of the table.

## SQL-to-C# Type Mapping

| SQL Server type | C# type |
|---|---|
| `uniqueidentifier` | `Guid` |
| `int` | `int` |
| `bigint` | `long` |
| `smallint` | `short` |
| `tinyint` | `byte` |
| `bit` | `bool` |
| `decimal(p,s)` / `numeric(p,s)` / `money` / `smallmoney` | `decimal` |
| `float` / `real` | `double` |
| `datetime` / `datetime2` / `smalldatetime` | `DateTime` |
| `date` | `DateOnly` |
| `time` | `TimeOnly` |
| `datetimeoffset` | `DateTimeOffset` |
| `char(n)` / `nchar(n)` / `varchar(n)` / `nvarchar(n)` / `text` / `ntext` | `string` |
| `varbinary` / `binary` / `image` | `byte[]` |
| `uniqueidentifier` used as a foreign-key column (name ends with `Id`) | typed `ValueObject` ID |

Apply `?` suffix for every C# property that maps to a Nullable (`Checked`) column, **except** for `string` and `byte[]` which are already reference types (use nullable annotation `string?` / `byte[]?` for nullable columns).

## Files to Generate

Given a table named **`<TableName>`** belonging to **`<ModuleName>`** module, generate the following four files:

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

### 2. Domain Event — `<ModuleName>.Domain/Events/<TableName>DomainEvents.cs`

Emit one event per aggregate lifecycle verb: `Created` and `Updated`.

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
- The column named `Id` (type `uniqueidentifier`, NOT NULL) always becomes the aggregate identity → use the typed ID `<TableName>Id` and inherit from `AggregateRoot<<TableName>Id>`.
- If the class is not intended as a root (user states it is a child entity), inherit from `Entity<<TableName>Id>` instead and omit `RaiseDomainEvent` calls.
- Every non-Id column becomes a public property with a **private setter**.
- A foreign-key column (name ends with `Id`, type `uniqueidentifier`) maps to the corresponding typed ValueObject ID (e.g. `UserId` → `UserId` type from the same module, if it exists, otherwise `Guid`).
- Validation lives in `private Set<PropertyName>(...)` methods; use `ArgumentException.ThrowIfNullOrWhiteSpace` for strings and null-guard checks for value types.
- Expose `OnCreated()` and `OnUpdated()` methods that call `RaiseDomainEvent(...)`.
- Mark the class `sealed`.

```csharp
using <ModuleName>.Domain.Events;
using <ModuleName>.Domain.ValueObjects;
using SharedKernel;

namespace <ModuleName>.Domain.Entities;

public sealed class <TableName> : AggregateRoot<<TableName>Id>
{
	// --- properties (one per non-Id column) ---
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
		// add guards appropriate to the type
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

## Step-by-Step Instructions

1. **Ask** the user for the module name (e.g. `Cart`, `Catalog`) and the table/entity name if not already provided.
2. **Parse** each non-empty row: extract `ColumnName`, `DataType`, `Nullable` flag.
3. **Map** each SQL type to a C# type using the table above. Apply `?` for nullable columns.
4. **Identify** foreign-key columns (name ends with `Id`, type `uniqueidentifier`); reference typed IDs where possible.
5. **Determine** required (NOT NULL, non-Id) columns — these become constructor parameters.
6. **Generate** all four files in order: ID ValueObject → Domain Event → Entity → Repository Interface.
7. **Do not** create project files, migrations, EF configurations, or application-layer files unless the user explicitly asks.
8. **Follow** all conventions from `.github/copilot-instructions.md` (sealed classes, private constructors, `ArgumentException.ThrowIfNullOrWhiteSpace`, `IReadOnlyCollection<T>`, no EF/MediatR in Domain).

## Example

**Input:**

```
Id            uniqueidentifier  Unchecked
UserId        uniqueidentifier  Checked
CreatedDate   datetime          Checked
Status        char(20)          Checked
```

**Module:** `Cart`  **Entity name:** `Order`

**Generated files:**

- `Cart.Domain/ValueObjects/OrderId.cs`
- `Cart.Domain/Events/OrderDomainEvents.cs`
- `Cart.Domain/Entities/Order.cs`
- `Cart.Domain/Repositories/IOrderRepository.cs`

The `Order` entity will have:
- `Id` → `OrderId` (aggregate identity)
- `UserId` → `Guid? UserId` (nullable Guid; if a `UserId` ValueObject exists in the module, use that type instead)
- `CreatedDate` → `DateTime? CreatedDate`
- `Status` → `string? Status`
