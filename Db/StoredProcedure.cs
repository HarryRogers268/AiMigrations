namespace Redgate.Text_Migrations_v2.Db;

public class StoredProcedure
{
    
    public string name;
    public string text;

    public StoredProcedure(string name, string text)
    {
        this.name = name;
        this.text = text;
    }
    
}