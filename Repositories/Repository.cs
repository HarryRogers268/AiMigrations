using Redgate.Text_Migrations_v2.Core;
using Redgate.Text_Migrations_v2.Db;

namespace Redgate.Text_Migrations_v2.Respositories;

public class Repository : IDatabaseRepository
{
    public IReadOnlyCollection<Database> GetDatabases()
    {
        var connection = new DbConnection(DbConnection.LocalServer());

        var databases = DbConnector.ExecuteSql(connection, Sql.GetAllDatabases());

        var toReturn = new List<Database>();

        foreach (var database in databases)
        {
            var newDb = new Database();
            newDb.Name = database[0];
            toReturn.Add(newDb);
        }
        
        return toReturn;
    }
}