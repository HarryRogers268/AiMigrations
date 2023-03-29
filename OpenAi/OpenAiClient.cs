using System.Net.Http.Headers;
using System.Text.Json;

namespace Redgate.Text_Migrations_v2.OpenAi;

public class OpenAiClient
{

    public async Task<string> ChatGpt(ChatGptMessage prompt)
    {
        return await ChatGpt(Enumerable.Repeat(prompt, 1));
    }
    
    public async Task<string> ChatGpt(IEnumerable<ChatGptMessage> prompt)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.openai.com/v1/"),
        };

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAiToken.Get());

        var content = new
        {
            model = "gpt-3.5-turbo",
            messages = prompt,
            temperature = 0
        };

        var options = new JsonSerializerOptions();
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        var json = JsonSerializer.Serialize(content, options);

        var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("chat/completions", jsonContent);

        var chatGptResponseString = await response.Content.ReadAsStringAsync();
        var chatGptResponse = JsonSerializer.Deserialize<ChatGptResponse>(chatGptResponseString, options);
        
        return chatGptResponse.choices[0].message.Content.Trim();
    }
    
}