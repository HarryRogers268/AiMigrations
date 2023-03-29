namespace Redgate.Text_Migrations_v2.Core;

public interface IDatabaseRepository
{
    IReadOnlyCollection<Database> GetDatabases();
}