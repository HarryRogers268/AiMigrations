namespace Redgate.Text_Migrations_v2.OpenAi;

public readonly record struct ChatGptResponse(Choice[] choices);

public readonly record struct Choice(ChatGptMessage message);