using Anthropic;
using Anthropic.Models.Messages;
using BenchmarkDotNet.Attributes;

namespace Anthropic.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ClientBenchmarks
{
    private AnthropicClient _client;
    private MessageCreateParams _parameters;

    [GlobalSetup]
    public void Setup()
    {
        _client = new AnthropicClient(new AnthropicClientOptions
        {
            ApiKey = "test-key-for-benchmarks"
        });

        _parameters = new MessageCreateParams
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
    public void InitializeClient()
    {
        var client = new AnthropicClient();
    }

    [Benchmark]
    public void InitializeClientWithOptions()
    {
        var client = new AnthropicClient(new AnthropicClientOptions
        {
            ApiKey = "test-key",
            MaxRetries = 3,
            Timeout = TimeSpan.FromSeconds(30)
        });
    }

    [Benchmark]
    public void InitializeClientWithCustomHttpClient()
    {
        var httpClient = new HttpClient();
        var client = new AnthropicClient(new AnthropicClientOptions
        {
            ApiKey = "test-key",
            HttpClient = httpClient
        });
    }

    [Benchmark]
    public void CreateMessageService()
    {
        var service = _client.Messages;
    }

    [Benchmark]
    public void ValidateMessageParameters()
    {
        var validator = new MessageCreateParamsValidator();
        validator.Validate(_parameters);
    }

    [Benchmark]
    public void BuildRequestUri()
    {
        var endpoint = new Uri("https://api.anthropic.com/v1/messages");
        var uri = new UriBuilder(endpoint);
    }
}
