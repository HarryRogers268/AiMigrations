namespace Redgate.Text_Migrations_v2.Db;

public static class Sql
{

    public static string GetAllDatabases()
    {
        return "SELECT name FROM sys.databases;";
    }
    
    public static string GetAllColumns()
    {
        return "SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE " +
               "FROM INFORMATION_SCHEMA.COLUMNS";
    }

    public static string GetUserStoredProcedure(string dbName)
    {
        return "SELECT name " +
               "FROM sys.objects " +
               "WHERE type = 'P' AND is_ms_shipped = 0 " +
               "ORDER BY name;";
    }
    
    public static string GetStoredProcedureText(string procName)
    {
        return "sp_helptext " + procName;
    }
    
    public static string getAllPks()
    {
        return "SELECT " +
               "TABLE_NAME = t.name, " +
               "CONSTRAINT_NAME = pk.name, " +
               "COLUMN_NAME = c.name " +
               "FROM " +
               "sys.tables t " +
               "INNER JOIN sys.indexes pk ON t.object_id = pk.object_id " +
               "INNER JOIN sys.index_columns ic ON ic.object_id = t.object_id AND ic.index_id = pk.index_id " +
               "INNER JOIN sys.columns c ON c.object_id = t.object_id AND c.column_id = ic.column_id " +
               "WHERE " +
               "pk.is_primary_key = 1 " +
               "ORDER BY " +
               "t.name, " +
               "pk.name, " +
               "ic.key_ordinal";
    }
}