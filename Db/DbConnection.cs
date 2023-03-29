namespace Redgate.Text_Migrations_v2.Db;

public class DbConnection
{
    public string connectionString;

    public DbConnection(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public static string LocalHost(string dbName)
    {
        return "Data Source=localhost;Database=" + dbName + ";Integrated Security=sspi;";
    }

    public static string LocalServer()
    {
        return "Data Source=localhost;Integrated Security=sspi;";
    }
}