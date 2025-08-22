using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ContentBlockVariants = Anthropic.Models.Messages.ContentBlockVariants;

namespace Anthropic.Models.Messages;

[JsonConverter(typeof(ContentBlockConverter))]
public abstract record class ContentBlock
{
    internal ContentBlock() { }

    public static implicit operator ContentBlock(TextBlock value) =>
        new ContentBlockVariants::TextBlock(value);

    public static implicit operator ContentBlock(ThinkingBlock value) =>
        new ContentBlockVariants::ThinkingBlock(value);

    public static implicit operator ContentBlock(RedactedThinkingBlock value) =>
        new ContentBlockVariants::RedactedThinkingBlock(value);

    public static implicit operator ContentBlock(ToolUseBlock value) =>
        new ContentBlockVariants::ToolUseBlock(value);

    public static implicit operator ContentBlock(ServerToolUseBlock value) =>
        new ContentBlockVariants::ServerToolUseBlock(value);

    public static implicit operator ContentBlock(WebSearchToolResultBlock value) =>
        new ContentBlockVariants::WebSearchToolResultBlock(value);

    public bool TryPickTextBlock([NotNullWhen(true)] out TextBlock? value)
    {
        value = (this as ContentBlockVariants::TextBlock)?.Value;
        return value != null;
    }

    public bool TryPickThinkingBlock([NotNullWhen(true)] out ThinkingBlock? value)
    {
        value = (this as ContentBlockVariants::ThinkingBlock)?.Value;
        return value != null;
    }

    public bool TryPickRedactedThinkingBlock([NotNullWhen(true)] out RedactedThinkingBlock? value)
    {
        value = (this as ContentBlockVariants::RedactedThinkingBlock)?.Value;
        return value != null;
    }

    public bool TryPickToolUseBlock([NotNullWhen(true)] out ToolUseBlock? value)
    {
        value = (this as ContentBlockVariants::ToolUseBlock)?.Value;
        return value != null;
    }

    public bool TryPickServerToolUseBlock([NotNullWhen(true)] out ServerToolUseBlock? value)
    {
        value = (this as ContentBlockVariants::ServerToolUseBlock)?.Value;
        return value != null;
    }

    public bool TryPickWebSearchToolResultBlock(
        [NotNullWhen(true)] out WebSearchToolResultBlock? value
    )
    {
        value = (this as ContentBlockVariants::WebSearchToolResultBlock)?.Value;
        return value != null;
    }

    public void Switch(
        Action<ContentBlockVariants::TextBlock> textBlock,
        Action<ContentBlockVariants::ThinkingBlock> thinkingBlock,
        Action<ContentBlockVariants::RedactedThinkingBlock> redactedThinkingBlock,
        Action<ContentBlockVariants::ToolUseBlock> toolUseBlock,
        Action<ContentBlockVariants::ServerToolUseBlock> serverToolUseBlock,
        Action<ContentBlockVariants::WebSearchToolResultBlock> webSearchToolResultBlock
    )
    {
        switch (this)
        {
            case ContentBlockVariants::TextBlock inner:
                textBlock(inner);
                break;
            case ContentBlockVariants::ThinkingBlock inner:
                thinkingBlock(inner);
                break;
            case ContentBlockVariants::RedactedThinkingBlock inner:
                redactedThinkingBlock(inner);
                break;
            case ContentBlockVariants::ToolUseBlock inner:
                toolUseBlock(inner);
                break;
            case ContentBlockVariants::ServerToolUseBlock inner:
                serverToolUseBlock(inner);
                break;
            case ContentBlockVariants::WebSearchToolResultBlock inner:
                webSearchToolResultBlock(inner);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public T Match<T>(
        Func<ContentBlockVariants::TextBlock, T> textBlock,
        Func<ContentBlockVariants::ThinkingBlock, T> thinkingBlock,
        Func<ContentBlockVariants::RedactedThinkingBlock, T> redactedThinkingBlock,
        Func<ContentBlockVariants::ToolUseBlock, T> toolUseBlock,
        Func<ContentBlockVariants::ServerToolUseBlock, T> serverToolUseBlock,
        Func<ContentBlockVariants::WebSearchToolResultBlock, T> webSearchToolResultBlock
    )
    {
        return this switch
        {
            ContentBlockVariants::TextBlock inner => textBlock(inner),
            ContentBlockVariants::ThinkingBlock inner => thinkingBlock(inner),
            ContentBlockVariants::RedactedThinkingBlock inner => redactedThinkingBlock(inner),
            ContentBlockVariants::ToolUseBlock inner => toolUseBlock(inner),
            ContentBlockVariants::ServerToolUseBlock inner => serverToolUseBlock(inner),
            ContentBlockVariants::WebSearchToolResultBlock inner => webSearchToolResultBlock(inner),
            _ => throw new InvalidOperationException(),
        };
    }

    public abstract void Validate();
}

sealed class ContentBlockConverter : JsonConverter<ContentBlock>
{
    public override ContentBlock? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var json = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
        string? type;
        try
        {
            type = json.GetProperty("type").GetString();
        }
        catch
        {
            type = null;
        }

        switch (type)
        {
            case "text":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<TextBlock>(json, options);
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::TextBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            case "thinking":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<ThinkingBlock>(json, options);
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::ThinkingBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            case "redacted_thinking":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<RedactedThinkingBlock>(
                        json,
                        options
                    );
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::RedactedThinkingBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            case "tool_use":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<ToolUseBlock>(json, options);
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::ToolUseBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            case "server_tool_use":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<ServerToolUseBlock>(
                        json,
                        options
                    );
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::ServerToolUseBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            case "web_search_tool_result":
            {
                List<JsonException> exceptions = [];

                try
                {
                    var deserialized = JsonSerializer.Deserialize<WebSearchToolResultBlock>(
                        json,
                        options
                    );
                    if (deserialized != null)
                    {
                        return new ContentBlockVariants::WebSearchToolResultBlock(deserialized);
                    }
                }
                catch (JsonException e)
                {
                    exceptions.Add(e);
                }

                throw new AggregateException(exceptions);
            }
            default:
            {
                throw new Exception();
            }
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        ContentBlock value,
        JsonSerializerOptions options
    )
    {
        object variant = value switch
        {
            ContentBlockVariants::TextBlock(var textBlock) => textBlock,
            ContentBlockVariants::ThinkingBlock(var thinkingBlock) => thinkingBlock,
            ContentBlockVariants::RedactedThinkingBlock(var redactedThinkingBlock) =>
                redactedThinkingBlock,
            ContentBlockVariants::ToolUseBlock(var toolUseBlock) => toolUseBlock,
            ContentBlockVariants::ServerToolUseBlock(var serverToolUseBlock) => serverToolUseBlock,
            ContentBlockVariants::WebSearchToolResultBlock(var webSearchToolResultBlock) =>
                webSearchToolResultBlock,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
        JsonSerializer.Serialize(writer, variant, options);
    }
}
