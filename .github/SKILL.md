# Skills

Each skill lives in its own file under `.github/skills/`.
Add new skills by creating `<SkillName>.skill.md` in that folder.

## GenerateDddEntityFromTableSchema

See [GenerateDddEntityFromTableSchema.skill.md](skills/GenerateDddEntityFromTableSchema.skill.md)

Generate a complete set of DDD domain files (`<Entity>Id`, `<Entity>DomainEvents`, `<Entity>`, `I<Entity>Repository`)
from a SQL Server table schema pasted in tab-separated format (`ColumnName | DataType | Nullable`).
