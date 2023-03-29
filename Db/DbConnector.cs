using System.Data.SqlClient;

namespace Redgate.Text_Migrations_v2.Db;

public static class DbConnector
{
    
    public static List<string[]> ExecuteSql(DbConnection dbConnection, string sql)
    {
        var output = new List<string[]>();

        var sqlConnection = new SqlConnection(dbConnection.connectionString);
        
        sqlConnection.Open();

        var command = new SqlCommand(sql, sqlConnection);

        var dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            var row = new string[dataReader.FieldCount];
            for (int i = 0; i < row.Length; i++)
            {
                row[i] = dataReader.GetValue(i).ToString()!;
            }
            output.Add(row);
        }
        
        sqlConnection.Close();
        return output;
    }

    public static void ExecuteSqlNoReturn(DbConnection dbConnection, string sql)
    {
        var sqlConnection = new SqlConnection(dbConnection.connectionString);
        
        sqlConnection.Open();

        var command = new SqlCommand(sql, sqlConnection);
        command.ExecuteReader();
        
        sqlConnection.Close();
    }
}