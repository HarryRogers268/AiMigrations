namespace Redgate.Text_Migrations_v2.OpenAi;

public class PromptBuilder
{
    private ChatGptMessage system;
    private ChatGptMessage job;
    private List<ChatGptMessage> context;
    private ChatGptMessage dataFormat;
    private List<ChatGptMessage> data;
    
    public PromptBuilder(ChatGptMessage job)
    {
        this.job = job;
        context = new List<ChatGptMessage>();
        data = new List<ChatGptMessage>();
    }

    public void SetSystem(ChatGptMessage systemMessage)
    {
        system = systemMessage;
    }
    
    public void AddContext(ChatGptMessage contextItem)
    {
        context.Add(contextItem);
    }

    public void SetDataFormat(ChatGptMessage dataFormat)
    {
        this.dataFormat = dataFormat;
    }

    public void AddData(ChatGptMessage dataItem)
    {
        data.Add(dataItem);
    }
    
    public List<ChatGptMessage> Build()
    {
        var fullPrompt = new List<ChatGptMessage>();
        if (system != null)
        {
            fullPrompt.Add(system);
        }
        fullPrompt.Add(job);
        foreach (var contextItem in context)
        {
            fullPrompt.Add(contextItem);
        }
        fullPrompt.Add(dataFormat);
        foreach (var dataItem in data)
        {
            fullPrompt.Add(dataItem);
        }

        return fullPrompt;
    }
}