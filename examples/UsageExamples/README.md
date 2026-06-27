# Usage Examples

This directory contains practical usage examples for the Anthropic SDK for C#.

## BasicUsage.cs
Minimal setup and first call to Anthropic API. Demonstrates the simplest way to get started with just a few lines of code.

## AdvancedUsage.cs
Advanced configuration, custom options, and error handling. Shows how to configure the client, use structured messages, handle errors, work with multi-turn conversations, and use tools.

## IntegrationExample.cs
Integration with ASP.NET Core Dependency Injection. Demonstrates how to integrate the Anthropic SDK into an ASP.NET Core application using dependency injection patterns.

## Running the Examples

These examples are standalone C# files that demonstrate specific patterns. To run them:

1. Navigate to the example directory:
   ```bash
   cd examples/UsageExamples
   ```

2. Create a new console project:
   ```bash
   dotnet new console -n UsageExamplesDemo
   cd UsageExamplesDemo
   ```

3. Copy the example file you want to run into this directory

4. Add the Anthropic SDK package:
   ```bash
   dotnet add package Anthropic
   ```

5. Update the Program.cs file with the example code

6. Set your API key:
   ```bash
   export ANTHROPIC_API_KEY=your-api-key-here
   ```

7. Run the example:
   ```bash
   dotnet run
   ```

For more comprehensive examples, see the other example directories in the `examples/` folder.