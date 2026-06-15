# Anthropic SDK for C#

A .NET library for interacting with the Anthropic API.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Build](https://github.com/sarmkadan/anthropic-sdk-csharp/actions/workflows/build.yml/badge.svg)

## Installation

```bash
dotnet add package Anthropic
```

## Usage

```csharp
using Anthropic;
using Anthropic.Models.Messages;

AnthropicClient client = new();

MessageCreateParams parameters = new()
{
    MaxTokens = 1024,
    Messages =
    [
        new()
        {
            Role = Role.User,
            Content = "Hello",
        },
    ],
    Model = "model-name",
};

var message = await client.Messages.Create(parameters);
```

## Configuration

The client can be configured using environment variables or by passing an `AnthropicClientOptions` object.

## Contributing

See [CONTRIBUTING.md](./CONTRIBUTING.md).
