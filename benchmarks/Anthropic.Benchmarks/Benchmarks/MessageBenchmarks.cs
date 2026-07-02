using Anthropic;
using Anthropic.Models.Messages;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace Anthropic.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class MessageBenchmarks
{
    private AnthropicClient _client;
    private MessageCreateParams _simpleParams;
    private MessageCreateParams _complexParams;
    private MessageCreateParams _largeParams;

    [GlobalSetup]
    public void Setup()
    {
        // Initialize client without API key for serialization/deserialization benchmarks
        _client = new AnthropicClient(new AnthropicClientOptions
        {
            ApiKey = "test-key-for-benchmarks"
        });

        // Simple message benchmark
        _simpleParams = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 1024,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = "Hello, how are you?"
                }
            ]
        };

        // Complex message with multiple content blocks
        _complexParams = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 2048,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = [
                        new TextBlockParam { Text = "Analyze this code:" },
                        new TextBlockParam { Text = "\n```csharp\npublic class Example { public void Method() { } }\n```" },
                        new TextBlockParam { Text = "\nWhat are the issues?" }
                    ]
                }
            ]
        };

        // Large message with many content blocks
        var contentBlocks = new List<ContentBlockParam>();
        for (int i = 0; i < 50; i++)
        {
            contentBlocks.Add(new TextBlockParam { Text = $"This is content block {i} with some sample text to make it realistic." });
        }

        _largeParams = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 4096,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = contentBlocks
                }
            ]
        };
    }

    [Benchmark]
    public void CreateSimpleMessageParams()
    {
        var parameters = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 1024,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = "Hello, how are you?"
                }
            ]
        };
    }

    [Benchmark]
    public void CreateComplexMessageParams()
    {
        var parameters = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 2048,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = [
                        new TextBlockParam { Text = "Analyze this code:" },
                        new TextBlockParam { Text = "\n```csharp\npublic class Example { public void Method() { } }\n```" },
                        new TextBlockParam { Text = "\nWhat are the issues?" }
                    ]
                }
            ]
        };
    }

    [Benchmark]
    public void SerializeSimpleMessage()
    {
        var parameters = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 1024,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = "Hello, how are you?"
                }
            ]
        };

        var json = System.Text.Json.JsonSerializer.Serialize(parameters);
    }

    [Benchmark]
    public void DeserializeSimpleMessage()
    {
        var json = "{\n            \"model\": \"claude-3-5-sonnet-20241022\",
            \"max_tokens\": 1024,
            \"messages\": [
                {
                    \"role\": \"user\",
                    \"content\": \"Hello, how are you?\"
                }
            ]
        }";

        var parameters = System.Text.Json.JsonSerializer.Deserialize<MessageCreateParams>(json);
    }

    [Benchmark]
    public void SerializeComplexMessage()
    {
        var parameters = new MessageCreateParams
        {
            Model = "claude-3-5-sonnet-20241022",
            MaxTokens = 2048,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = [
                        new TextBlockParam { Text = "Analyze this code:" },
                        new TextBlockParam { Text = "\n```csharp\npublic class Example { public void Method() { } }\n```" },
                        new TextBlockParam { Text = "\nWhat are the issues?" }
                    ]
                }
            ]
        };

        var json = System.Text.Json.JsonSerializer.Serialize(parameters);
    }

    [Benchmark]
    public void CountTokensSimpleMessage()
    {
        var parameters = new MessageCountTokensParams
        {
            Model = "claude-3-5-sonnet-20241022",
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = "Hello, how are you?"
                }
            ]
        };

        var count = MessageTokensCount.CountTokens(parameters);
    }

    [Benchmark]
    public void CountTokensComplexMessage()
    {
        var parameters = new MessageCountTokensParams
        {
            Model = "claude-3-5-sonnet-20241022",
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = [
                        new TextBlockParam { Text = "Analyze this code:" },
                        new TextBlockParam { Text = "\n```csharp\npublic class Example { public void Method() { } }\n```" },
                        new TextBlockParam { Text = "\nWhat are the issues?" }
                    ]
                }
            ]
        };

        var count = MessageTokensCount.CountTokens(parameters);
    }

    [Benchmark]
    public void CreateMessageParam()
    {
        var param = new MessageParam
        {
            Role = Role.User,
            Content = "Test message content"
        };
    }

    [Benchmark]
    public void CreateTextBlockParam()
    {
        var block = new TextBlockParam
        {
            Text = "This is a sample text block for benchmarking"
        };
    }

    [Benchmark]
    public void CreateContentBlockUnion()
    {
        var block = new ContentBlockParam
        {
            Type = "text",
            Text = "Sample text content"
        };
    }
}
