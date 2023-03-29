using Redgate.Text_Migrations_v2.OpenAi;

namespace Redgate.Text_Migrations_v2.Db;

public class MigrationBuilder
{

    private DbConnection from;
    private DbConnection to;

    public MigrationBuilder(string from, string to)
    {
        this.from = new DbConnection(DbConnection.LocalHost(from));
        this.to = new DbConnection(DbConnection.LocalHost(to));
    }

    public async Task<string> Build()
    {
        var dbTablesFrom = DbUtility.GetTables(from);
        var dbTablesTo = DbUtility.GetTables(to);

        var prompt = new PromptBuilder(new ChatGptMessage("user",
            "Your task is to generate a SQL migration script given two database schemas."));
        //change
        prompt.SetSystem(new ChatGptMessage("system", "Respond as per requests. "));
        prompt.AddContext(new ChatGptMessage("user", "It should be valid for Microsoft SQLServer."));
        prompt.AddContext(new ChatGptMessage("user", "The code should be embedded in SQL markdown tags."));
        prompt.AddContext(new ChatGptMessage("user", "Data should be preserved where possible, infer from the context of the column names."));
        prompt.AddContext(new ChatGptMessage("user", "Do not prefix or suffix code with any additional explanation."));
        
        prompt.SetDataFormat(new ChatGptMessage("user", "The two schemas are provided below. " +
                                                        "It contains table names, columns names and their data types." +
                                                        " The format that they are shown in are as follows:\n" +
                                                        "Database 1\n" +
                                                        "Table 1:\n" +
                                                        "- Column 1, datatype\n" +
                                                        "- Column 2, datatype\n" +
                                                        "Table 2:\n" +
                                                        "- Column 1, datatype\n" +
                                                        "- Column 2, datatype\n" +
                                                        "- Column 2, datatype\n"));
        
        prompt.AddData(new ChatGptMessage("user", DbUtility.FormatData(dbTablesFrom, "The following is the first schema")));
        prompt.AddData(new ChatGptMessage("user", DbUtility.FormatData(dbTablesTo, "The following is the second schema")));
        
        var openAiClient = new OpenAiClient();
        var response = await openAiClient.ChatGpt(prompt.Build());
        
        return response;
    }

}