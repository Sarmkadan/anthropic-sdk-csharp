using Anthropic;
using Anthropic.Models.Messages;

// BasicUsage.cs - Minimal setup and first call to Anthropic API
// This example demonstrates the simplest way to get started with the Anthropic SDK

// Initialize the client with default configuration
// The client will automatically read from ANTHROPIC_API_KEY environment variable
var client = new AnthropicClient();

// Create a simple message request
MessageCreateParams parameters = new()
{
    MaxTokens = 1024,
    Messages = [
        new()
        {
            Role = Role.User,
            Content = "Write a short haiku about winter"
        }
    ],
    Model = "claude-3-5-sonnet-20241022",
};

// Make the API call
var response = await client.Messages.Create(parameters);

// Process the response
var messageContent = string.Join(
    "",
    response.Content
        .Select(content => content.Value)
        .OfType<TextBlock>()
        .Select(textBlock => textBlock.Text)
);

Console.WriteLine("Response:");
Console.WriteLine(messageContent);

// Key points:
// 1. Single line initialization with default client
// 2. Minimal required parameters: MaxTokens, Messages, Model
// 3. Simple response processing for text content