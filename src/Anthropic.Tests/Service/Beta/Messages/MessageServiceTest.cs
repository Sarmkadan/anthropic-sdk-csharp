using Beta = Anthropic.Models.Beta;
using BetaCacheControlEphemeralProperties = Anthropic.Models.Beta.Messages.BetaCacheControlEphemeralProperties;
using BetaMessageParamProperties = Anthropic.Models.Beta.Messages.BetaMessageParamProperties;
using BetaToolProperties = Anthropic.Models.Beta.Messages.BetaToolProperties;
using Json = System.Text.Json;
using MessageCreateParamsProperties = Anthropic.Models.Beta.Messages.MessageCreateParamsProperties;
using Messages = Anthropic.Models.Beta.Messages;
using Messages1 = Anthropic.Models.Messages;
using Tasks = System.Threading.Tasks;
using Tests = Anthropic.Tests;

namespace Anthropic.Tests.Service.Beta.Messages;

public class MessageServiceTest : Tests::TestBase
{
    [Fact]
    public async Tasks::Task Create_Works()
    {
        var betaMessage = await this.client.Beta.Messages.Create(
            new Messages::MessageCreateParams()
            {
                MaxTokens = 1024,
                Messages =
                [
                    new Messages::BetaMessageParam()
                    {
                        Content = "string",
                        Role = BetaMessageParamProperties::Role.User,
                    },
                ],
                Model = Messages1::Model.Claude3_7SonnetLatest,
                Container = "container",
                MCPServers =
                [
                    new Messages::BetaRequestMCPServerURLDefinition()
                    {
                        Name = "name",
                        URL = "url",
                        AuthorizationToken = "authorization_token",
                        ToolConfiguration = new Messages::BetaRequestMCPServerToolConfiguration()
                        {
                            AllowedTools = ["string"],
                            Enabled = true,
                        },
                    },
                ],
                Metadata = new Messages::BetaMetadata()
                {
                    UserID = "13803d75-b4b5-4c3e-b2a2-6f21399b021b",
                },
                ServiceTier = MessageCreateParamsProperties::ServiceTier.Auto,
                StopSequences = ["string"],
                Stream = true,
                System =
                [
                    new Messages::BetaTextBlockParam()
                    {
                        Text = "Today's date is 2024-06-01.",
                        CacheControl = new Messages::BetaCacheControlEphemeral()
                        {
                            TTL = BetaCacheControlEphemeralProperties::TTL.TTL5m,
                        },
                        Citations =
                        [
                            new Messages::BetaCitationCharLocationParam()
                            {
                                CitedText = "cited_text",
                                DocumentIndex = 0,
                                DocumentTitle = "x",
                                EndCharIndex = 0,
                                StartCharIndex = 0,
                            },
                        ],
                    },
                ],
                Temperature = 1,
                Thinking = new Messages::BetaThinkingConfigEnabled() { BudgetTokens = 1024 },
                ToolChoice = new Messages::BetaToolChoiceAuto() { DisableParallelToolUse = true },
                Tools =
                [
                    new Messages::BetaTool()
                    {
                        InputSchema = new BetaToolProperties::InputSchema()
                        {
                            Properties1 = Json::JsonSerializer.Deserialize<Json::JsonElement>(
                                "{\"location\":{\"description\":\"The city and state, e.g. San Francisco, CA\",\"type\":\"string\"},\"unit\":{\"description\":\"Unit for the output - one of (celsius, fahrenheit)\",\"type\":\"string\"}}"
                            ),
                            Required = ["location"],
                        },
                        Name = "name",
                        CacheControl = new Messages::BetaCacheControlEphemeral()
                        {
                            TTL = BetaCacheControlEphemeralProperties::TTL.TTL5m,
                        },
                        Description = "Get the current weather in a given location",
                        Type = BetaToolProperties::Type.Custom,
                    },
                ],
                TopK = 5,
                TopP = 0.7,
                Betas = [Beta::AnthropicBeta.MessageBatches2024_09_24],
            }
        );
        betaMessage.Validate();
    }

    [Fact]
    public async Tasks::Task CountTokens_Works()
    {
        var betaMessageTokensCount = await this.client.Beta.Messages.CountTokens(
            new Messages::MessageCountTokensParams()
            {
                Messages =
                [
                    new Messages::BetaMessageParam()
                    {
                        Content = "string",
                        Role = BetaMessageParamProperties::Role.User,
                    },
                ],
                Model = Messages1::Model.Claude3_7SonnetLatest,
                MCPServers =
                [
                    new Messages::BetaRequestMCPServerURLDefinition()
                    {
                        Name = "name",
                        URL = "url",
                        AuthorizationToken = "authorization_token",
                        ToolConfiguration = new Messages::BetaRequestMCPServerToolConfiguration()
                        {
                            AllowedTools = ["string"],
                            Enabled = true,
                        },
                    },
                ],
                System =
                [
                    new Messages::BetaTextBlockParam()
                    {
                        Text = "Today's date is 2024-06-01.",
                        CacheControl = new Messages::BetaCacheControlEphemeral()
                        {
                            TTL = BetaCacheControlEphemeralProperties::TTL.TTL5m,
                        },
                        Citations =
                        [
                            new Messages::BetaCitationCharLocationParam()
                            {
                                CitedText = "cited_text",
                                DocumentIndex = 0,
                                DocumentTitle = "x",
                                EndCharIndex = 0,
                                StartCharIndex = 0,
                            },
                        ],
                    },
                ],
                Thinking = new Messages::BetaThinkingConfigEnabled() { BudgetTokens = 1024 },
                ToolChoice = new Messages::BetaToolChoiceAuto() { DisableParallelToolUse = true },
                Tools =
                [
                    new Messages::BetaTool()
                    {
                        InputSchema = new BetaToolProperties::InputSchema()
                        {
                            Properties1 = Json::JsonSerializer.Deserialize<Json::JsonElement>(
                                "{\"location\":{\"description\":\"The city and state, e.g. San Francisco, CA\",\"type\":\"string\"},\"unit\":{\"description\":\"Unit for the output - one of (celsius, fahrenheit)\",\"type\":\"string\"}}"
                            ),
                            Required = ["location"],
                        },
                        Name = "name",
                        CacheControl = new Messages::BetaCacheControlEphemeral()
                        {
                            TTL = BetaCacheControlEphemeralProperties::TTL.TTL5m,
                        },
                        Description = "Get the current weather in a given location",
                        Type = BetaToolProperties::Type.Custom,
                    },
                ],
                Betas = [Beta::AnthropicBeta.MessageBatches2024_09_24],
            }
        );
        betaMessageTokensCount.Validate();
    }
}
