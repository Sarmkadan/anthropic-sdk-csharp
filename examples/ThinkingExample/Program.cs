using System;
using Anthropic;
using Anthropic.Models.Messages;
using Anthropic.Models.Messages.ContentBlockVariants;
using Anthropic.Models.Messages.MessageParamProperties;

// Configured using the ANTHROPIC_API_KEY, ANTHROPIC_AUTH_TOKEN and ANTHROPIC_BASE_URL environment variables
AnthropicClient client = new();

MessageCreateParams parameters = new()
{
    MaxTokens = 2048,
    Messages = [new() { Content = "Tell me a story about building the best SDK!", Role = Role.User }],
    Model = Model.Claude4Sonnet20250514,
    Thinking = new ThinkingConfigEnabled()
    {
        BudgetTokens = 1024
    }
};

var response = await client.Messages.Create(parameters);

foreach(ContentBlock block in response.Content)
{
    if (block.TryPickThinkingBlockVariant(out ThinkingBlock thinking))
    {
        Console.WriteLine($"Thinking: {thinking.Thinking}");
    }
    else if (block.TryPickTextBlockVariant(out TextBlock text))
    {
        Console.WriteLine($"Text: {text.Text}");
    }
}

var message = String.Join("", response.Content
    .OfType<TextBlockVariant>()
    .Select((textBlock) => textBlock.Value.Text));

Console.WriteLine(message);
