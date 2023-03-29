namespace Redgate.Text_Migrations_v2.Db;

public class Table
{

    public string name;
    // [0] = column name, [1] = data type, [2] = is PK, [3] = FK
    public List<string[]> columns;

    public Table(string name)
    {
        this.name = name;
        columns = new List<string[]>();
    }

    public Table(string name, string[] firstColumn)
    {
        this.name = name;
        columns = new List<string[]>();
        columns.Add(firstColumn);
    }

}