# Skills

Each skill lives in its own file under `.github/skills/`.
Add new skills by creating `<SkillName>.skill.md` in that folder.

## GenerateDddEntityFromTableSchema

See [GenerateDddEntityFromTableSchema.skill.md](skills/GenerateDddEntityFromTableSchema.skill.md)

Generate a complete set of DDD domain files from a SQL Server table schema pasted in tab-separated format (`ColumnName | DataType | Nullable`).
Produces **five** files per entity: typed ID ValueObject, Domain Events, Entity/Aggregate Root (with `[MaxLength]`/`[Precision]` annotations and private setter guards), Repository Interface, and EF Core Configuration.
Defaults to `AggregateRoot<TId>`; override to `Entity<TId>` for child entities (domain events file is skipped automatically).
