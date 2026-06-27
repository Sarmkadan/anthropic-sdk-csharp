using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

// IntegrationExample.cs - Integration with ASP.NET Core Dependency Injection
// This example demonstrates how to integrate the Anthropic SDK into an ASP.NET Core application

// Example 1: Basic DI setup in a console application
Console.WriteLine("=== Example 1: Basic DI Setup ===");

// Create a simple console application with DI
var services = new ServiceCollection();

// Register the Anthropic client as a singleton
services.AddSingleton<IAnthropicClient>(provider => new AnthropicClient());

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Resolve and use the client
var client = serviceProvider.GetRequiredService<IAnthropicClient>();

var response = await client.Messages.Create(new MessageCreateParams
{
    MaxTokens = 1024,
    Messages = [
        new()
        {
            Role = Role.User,
            Content = "Explain the concept of dependency injection in simple terms"
        }
    ],
    Model = "claude-3-5-sonnet-20241022"
});

var messageContent = string.Join(
    "",
    response.Content
        .Select(content => content.Value)
        .OfType<TextBlock>()
        .Select(textBlock => textBlock.Text)
);

Console.WriteLine(messageContent);

// Example 2: Configuration-based setup
Console.WriteLine("\n=== Example 2: Configuration-based Setup ===");

// Create configuration
var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string>
    {
        ["Anthropic:ApiKey"] = "your-api-key-from-config",
        ["Anthropic:BaseUrl"] = "https://api.anthropic.com",
        ["Anthropic:DefaultModel"] = "claude-3-5-sonnet-20241022",
        ["Anthropic:Timeout"] = "30"
    })
    .Build();

var configServices = new ServiceCollection();

// Register configuration
configServices.AddSingleton<IConfiguration>(configuration);

// Register Anthropic client with configuration
configServices.AddSingleton<IAnthropicClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiKey = config["Anthropic:ApiKey"] ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");

    var options = new AnthropicClientOptions
    {
        ApiKey = apiKey,
        BaseUrl = config["Anthropic:BaseUrl"],
        DefaultModel = config["Anthropic:DefaultModel"],
        Timeout = TimeSpan.FromSeconds(int.Parse(config["Anthropic:Timeout"] ?? "30"))
    };

    return new AnthropicClient(options);
});

var configServiceProvider = configServices.BuildServiceProvider();
var configClient = configServiceProvider.GetRequiredService<IAnthropicClient>();

var configResponse = await configClient.Messages.Create(new MessageCreateParams
{
    MaxTokens = 1024,
    Messages = [
        new()
        {
            Role = Role.User,
            Content = "What are the benefits of using dependency injection?"
        }
    ]
});

var configMessageContent = string.Join(
    "",
    configResponse.Content
        .Select(content => content.Value)
        .OfType<TextBlock>()
        .Select(textBlock => textBlock.Text)
);

Console.WriteLine(configMessageContent);

// Example 3: ASP.NET Core Web Application integration
Console.WriteLine("\n=== Example 3: ASP.NET Core Web App Integration ===");

// This would typically be in Program.cs of an ASP.NET Core application
// For demonstration, we'll show the pattern

var webAppServices = new ServiceCollection();

// Register Anthropic client with options pattern
webAppServices.AddOptions<AnthropicClientOptions>()
    .Bind(configuration.GetSection("Anthropic"))
    .ValidateOnStart();

webAppServices.AddSingleton<IAnthropicClient>(provider =>
{
    var options = provider.GetRequiredService<IOptions<AnthropicClientOptions>>().Value;
    return new AnthropicClient(options);
});

// You can also register it as a hosted service if needed
webAppServices.AddHostedService<AnthropicBackgroundService>();

// Example of a background service that uses Anthropic
var webAppProvider = webAppServices.BuildServiceProvider();

// Example 4: Using the client in a controller/service
Console.WriteLine("\n=== Example 4: Service Pattern ===");

// Create a service that uses Anthropic
var chatService = new ChatService(client);
var chatResponse = await chatService.GetAssistantResponse("Tell me a joke about programming");
Console.WriteLine(chatResponse);

// Example 5: Multiple clients for different purposes
Console.WriteLine("\n=== Example 5: Multiple Clients ===");

var servicesForMultipleClients = new ServiceCollection();

// Create different clients for different use cases
servicesForMultipleClients.AddSingleton<IAnthropicClient>(_ => new AnthropicClient(
    new AnthropicClientOptions
    {
        DefaultModel = "claude-3-5-sonnet-20241022",
        DefaultMaxTokens = 2048
    }
));

servicesForMultipleClients.AddSingleton<IAnthropicClient>(_ => new AnthropicClient(
    new AnthropicClientOptions
    {
        DefaultModel = "claude-3-haiku-20240307",
        DefaultMaxTokens = 512
    }
));

var multipleClientsProvider = servicesForMultipleClients.BuildServiceProvider();
var creativeClient = multipleClientsProvider.GetServices<IAnthropicClient>().First();
var fastClient = multipleClientsProvider.GetServices<IAnthropicClient>().Last();

Console.WriteLine("Using creative model:");
var creativeResponse = await creativeClient.Messages.Create(new MessageCreateParams
{
    Messages = [new() { Role = Role.User, Content = "Write a poem about autumn" }],
    MaxTokens = 1024
});
Console.WriteLine(string.Join("", creativeResponse.Content.OfType<TextBlock>().Select(t => t.Text)));

Console.WriteLine("\nUsing fast model:");
var fastResponse = await fastClient.Messages.Create(new MessageCreateParams
{
    Messages = [new() { Role = Role.User, Content = "Summarize this text in one sentence" }],
    MaxTokens = 512
});
Console.WriteLine(string.Join("", fastResponse.Content.OfType<TextBlock>().Select(t => t.Text)));

// Supporting classes for the examples
public interface IAnthropicClient
{
    Task<MessageResponse> Messages.Create(MessageCreateParams parameters);
}

public class ChatService
{
    private readonly IAnthropicClient _client;

    public ChatService(IAnthropicClient client)
    {
        _client = client;
    }

    public async Task<string> GetAssistantResponse(string userMessage)
    {
        var response = await _client.Messages.Create(new MessageCreateParams
        {
            MaxTokens = 1024,
            Messages = [new() { Role = Role.User, Content = userMessage }],
            Model = "claude-3-5-sonnet-20241022"
        });

        return string.Join("", response.Content.OfType<TextBlock>().Select(t => t.Text));
    }
}

public class AnthropicBackgroundService : BackgroundService
{
    private readonly IAnthropicClient _client;

    public AnthropicBackgroundService(IAnthropicClient client)
    {
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Example: Periodic tasks using Anthropic
        while (!stoppingToken.IsCancellationRequested)
        {
            // Perform periodic operations
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

// Key points:
// 1. Basic DI registration of AnthropicClient
// 2. Configuration-based setup with IConfiguration
// 3. ASP.NET Core integration patterns
// 4. Service pattern for encapsulating Anthropic usage
// 5. Multiple client instances for different use cases
// 6. Options pattern for configuration
// 7. Background service integration