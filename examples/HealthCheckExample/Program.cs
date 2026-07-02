using Anthropic;

// Simple health check endpoint for Docker container
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/health", () => "Healthy");

app.MapGet("/api-key", (IConfiguration config) =>
    string.IsNullOrEmpty(config["ANTHROPIC_API_KEY"])
        ? "No API key configured"
        : "API key configured"
);

app.Run("http://+:8080");
