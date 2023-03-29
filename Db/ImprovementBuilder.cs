using System.Collections.Immutable;
using Redgate.Text_Migrations_v2.OpenAi;

namespace Redgate.Text_Migrations_v2.Db;

public class ImprovementBuilder
{

    private DbConnection db;
    private string dbName;
    
    public ImprovementBuilder(string name)
    {
        db = new DbConnection(DbConnection.LocalHost(name));
        dbName = name;
    }

    public async Task<string> Build()
    {
        var tables = DbUtility.GetTables(db);
        var storedProcedures = DbUtility.GetStoredProcedures(db, dbName);
        
        var prompt = new PromptBuilder(new ChatGptMessage("user", "Your task is to give recommendations" +
                                                              " to improve a given database schema."));
        prompt.SetSystem(new ChatGptMessage("system", "You must reply in JSON format, with fields 'name', 'description', and 'sql'."));
        prompt.AddContext(new ChatGptMessage("user", "Improvement should make the database easier to use " +
                                                     "and run more quickly."));
        prompt.AddContext(new ChatGptMessage("user", "Any SQL snippets provided should be embedded in SQL markdown tags."));
        prompt.AddContext(new ChatGptMessage("user", "Output should be in JSON format, with each improvement having 'name', " +
                                                     "a brief name of the improvement, 'description', a detailed description of the improvement, and 'sql'," +
                                                     " the SQL server code that can be ran on the database to implement the improvement." +
                                                     "There should be no more tags than the ones previously specified"));
        prompt.SetDataFormat(new ChatGptMessage("user", "A representation of the database schema and " +
                                                        "stored procedures are provided below"));
        prompt.SetDataFormat(new ChatGptMessage("user", "For each improvement, recommend how it can be applied" +
                                                        " to the given database schema"));
        
        prompt.AddData(new ChatGptMessage("user", DbUtility.FormatData(tables)));
        prompt.AddData(new ChatGptMessage("user", DbUtility.FormatData(storedProcedures)));

        var openAiClient = new OpenAiClient();
        var response = await openAiClient.ChatGpt(prompt.Build());

        return response;
    }
    
}