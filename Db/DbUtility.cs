namespace Redgate.Text_Migrations_v2.Db;

public static class DbUtility
{
     public static List<Table> GetTables(DbConnection dbConnection)
    {
        var dbColumns = DbConnector.ExecuteSql(dbConnection, Sql.GetAllColumns());
        var pks = DbConnector.ExecuteSql(dbConnection, Sql.getAllPks());
        var tables = new List<Table>();

        foreach (var column in dbColumns)
        {
            var found = false;
            var isPk = "0";
            foreach (var pk in pks)
            {
                if (pk[0] == column[0] && pk[2] == column[1])
                {
                    isPk = "1";
                    break;
                }
            }
            foreach (var table in tables)
            {
                if (table.name == column[0])
                {
                    found = true;
                    table.columns.Add(new [] {column[1], column[2], isPk});
                    break;
                }
            }

            if (!found)
            {
                tables.Add(new Table(column[0], new [] {column[1], column[2], isPk}));
            }
        }
        
        return tables;
    }

    public static List<StoredProcedure> GetStoredProcedures(DbConnection dbConnection, string dbName)
    {
        var dbProcs = DbConnector.ExecuteSql(dbConnection, Sql.GetUserStoredProcedure(dbName));
        var storedProcs = new List<StoredProcedure>();

        foreach (var proc in dbProcs)
        {
            var storedProcTextSplit = DbConnector.ExecuteSql(dbConnection, Sql.GetStoredProcedureText(proc[0]));
            var storedProcText = "";
            foreach (var line in storedProcTextSplit)
            {
                storedProcText += line[0].Trim() + " ";
            }
            storedProcs.Add(new StoredProcedure(proc[0], storedProcText));  
        }

        return storedProcs;
    }
    
    public static string FormatData(List<Table> db, string starter = "The following are the tables within the database")
    {
        string output = starter + "\n";
        foreach (var table in db)
        {
            output += table.name + ":\n";
            foreach (var column in table.columns)
            {
                output += "- " + column[0] + ", " + column[1] + (column[2] == "1" ? " is primary key" : "") + "\n";
            }
        }
        
        return output;
    }
    
    public static string FormatData(List<StoredProcedure> db)
    {
        string output = "The following are the stored procedures contained within the database\n";
        foreach (var storedProcedure in db)
        {
            output += storedProcedure.name + ": " + storedProcedure.text + "\n";
        }

        return output;
    }
    
}