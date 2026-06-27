using Anthropic;
using Anthropic.Models.Messages;
using Anthropic.Models;

// AdvancedUsage.cs - Advanced configuration, custom options, and error handling
// This example demonstrates advanced features of the Anthropic SDK

// Example 1: Custom client configuration
Console.WriteLine("=== Example 1: Custom Configuration ===");

var customOptions = new AnthropicClientOptions
{
    ApiKey = "your-api-key-here", // Can also use ANTHROPIC_API_KEY environment variable
    BaseUrl = "https://api.anthropic.com", // Custom base URL if needed
    MaxRetryAttempts = 3, // Configure retry policy
    Timeout = TimeSpan.FromSeconds(30), // Custom timeout
    DefaultModel = "claude-3-5-sonnet-20241022", // Default model for all requests
    DefaultMaxTokens = 2048, // Default max tokens for all requests
};

var customClient = new AnthropicClient(customOptions);

// Example 2: Structured messages with system prompt
Console.WriteLine("\n=== Example 2: Structured Messages ===");

var structuredParameters = new MessageCreateParams
{
    MaxTokens = 2048,
    Model = "claude-3-5-sonnet-20241022",
    System = [
        new()
        {
            Type = SystemContentType.Text,
            Text = "You are a helpful assistant that responds in JSON format"
        }
    ],
    Messages = [
        new()
        {
            Role = Role.User,
            Content = "Create a user profile with name, email, and age fields"
        }
    ],
    Temperature = 0.7f, // Control response creativity
    TopP = 0.9f, // Alternative to temperature
};

try
{
    var structuredResponse = await customClient.Messages.Create(structuredParameters);
    var structuredContent = string.Join(
        "",
        structuredResponse.Content
            .Select(content => content.Value)
            .OfType<TextBlock>()
            .Select(textBlock => textBlock.Text)
    );
    Console.WriteLine(structuredContent);
}
catch (Exception ex) when (ex is AnthropicException or HttpRequestException)
{
    Console.WriteLine($"Error: {ex.Message}");
    // Handle different types of errors appropriately
    if (ex is AnthropicException anthropicEx)
    {
        Console.WriteLine($"Error Code: {anthropicEx.ErrorCode}");
        Console.WriteLine($"Error Type: {anthropicEx.ErrorType}");
    }
}

// Example 3: Multi-turn conversation
Console.WriteLine("\n=== Example 3: Multi-turn Conversation ===");

var conversationClient = new AnthropicClient();
var conversationHistory = new List<MessageParam>();

// First message
conversationHistory.Add(new()
{
    Role = Role.User,
    Content = "I need help with my C# code"
});

var firstResponse = await conversationClient.Messages.Create(new MessageCreateParams
{
    Messages = conversationHistory,
    Model = "claude-3-5-sonnet-20241022",
    MaxTokens = 2048,
});

var firstMessageContent = string.Join(
    "",
    firstResponse.Content
        .Select(content => content.Value)
        .OfType<TextBlock>()
        .Select(textBlock => textBlock.Text)
);
Console.WriteLine(firstMessageContent);

// Second message (continuing conversation)
conversationHistory.Add(new()
{
    Role = Role.Assistant,
    Content = firstMessageContent
});

conversationHistory.Add(new()
{
    Role = Role.User,
    Content = "Can you show me a better way to write this code?"
});

var secondResponse = await conversationClient.Messages.Create(new MessageCreateParams
{
    Messages = conversationHistory,
    Model = "claude-3-5-sonnet-20241022",
    MaxTokens = 2048,
});

var secondMessageContent = string.Join(
    "",
    secondResponse.Content
        .Select(content => content.Value)
        .OfType<TextBlock>()
        .Select(textBlock => textBlock.Text)
);
Console.WriteLine(secondMessageContent);

// Example 4: Using tools (functions)
Console.WriteLine("\n=== Example 4: Using Tools ===");

var toolsClient = new AnthropicClient();

var toolsParameters = new MessageCreateParams
{
    MaxTokens = 2048,
    Model = "claude-3-5-sonnet-20241022",
    Messages = [
        new()
        {
            Role = Role.User,
            Content = "What's the weather in London?"
        }
    ],
    Tools = [
        new()
        {
            Name = "get_weather",
            Description = "Get weather information for a location",
            InputSchema = new JsonSchema
            {
                Type = "object",
                Properties = new Dictionary<string, JsonSchemaProperty>
                {
                    ["location"] = new()
                    {
                        Type = "string",
                        Description = "The city and country to get weather for"
                    }
                },
                Required = ["location"]
            }
        }
    ],
    ToolChoice = new ToolChoice
    {
        Type = ToolChoiceType.Auto
    }
};

var toolsResponse = await toolsClient.Messages.Create(toolsParameters);

// Check if the model wants to use a tool
if (toolsResponse.StopReason == StopReason.ToolUse)
{
    Console.WriteLine("Model requested to use a tool");

    // In a real application, you would execute the tool here
    // and then continue the conversation with the tool result
}

// Example 5: Streaming response
Console.WriteLine("\n=== Example 5: Streaming Response ===");

var streamingClient = new AnthropicClient();

var streamingResponse = streamingClient.Messages.CreateStreaming(
    new MessageCreateParams
    {
        MaxTokens = 2048,
        Messages = [
            new()
            {
                Role = Role.User,
                Content = "Write a 3-paragraph story about a robot discovering emotions"
            }
        ],
        Model = "claude-3-5-sonnet-20241022"
    }
);

await foreach (var messageStreamEvent in streamingResponse)
{
    switch (messageStreamEvent)
    {
        case RawContentBlockDeltaEvent deltaEvent:
            if (deltaEvent.Delta is TextDelta textDelta)
            {
                Console.Write(textDelta.Text);
            }
            break;

        case RawMessageStopEvent _:
            Console.WriteLine();
            Console.WriteLine("\n=== Stream complete ===");
            break;
    }
}

// Key points:
// 1. Custom client configuration with multiple options
// 2. Structured messages with system prompts
// 3. Multi-turn conversation handling
// 4. Error handling with specific exception types
// 5. Tool usage patterns
// 6. Streaming responses for real-time feedback