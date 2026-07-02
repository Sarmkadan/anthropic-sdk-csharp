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

// Initialize the client
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
    Model = "claude-3-5-sonnet-20241022",
};

// Create a message
var message = await client.Messages.Create(parameters);
```

For more examples, see the [examples/UsageExamples](examples/UsageExamples) directory.

## Configuration

The client can be configured using environment variables or by passing an `AnthropicClientOptions` object.

## Contributing

See [CONTRIBUTING.md](./CONTRIBUTING.md).

## Docker Support

The Anthropic SDK can be built and tested using Docker for a consistent development environment.


### Building with Docker


```bash
# Build the Docker image
docker compose build anthropic-sdk

# Run the build inside the container
docker compose run --rm anthropic-sdk dotnet build src/Anthropic/Anthropic.csproj -c Release

# Run tests
docker compose run --rm anthropic-sdk dotnet test src/Anthropic.Tests/Anthropic.Tests.csproj
```

### Using the library in a Docker container


To use the Anthropic SDK in your own Dockerized application:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY . ./

# Restore and build your application
RUN dotnet add package Anthropic
RUN dotnet build

# Your application can now use the Anthropic SDK
```

### Development with Docker


For development, you can use the provided docker-compose.yml:


```bash
# Start a development container
docker compose up -d anthropic-sdk

# Execute commands in the container
docker compose exec anthropic-sdk dotnet --version
```


See [.dockerignore](./.dockerignore) for files excluded from Docker builds.
