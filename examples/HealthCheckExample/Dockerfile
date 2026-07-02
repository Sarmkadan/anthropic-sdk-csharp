# Dockerfile for HealthCheckExample web application

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY HealthCheckExample.csproj ./
RUN dotnet restore "HealthCheckExample.csproj"

# Copy source code
COPY . .

# Build the application
RUN dotnet build "HealthCheckExample.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "HealthCheckExample.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Install curl for health checks
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080 \
    ANTHROPIC_API_KEY="your-api-key-here" \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Health check
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost:8080/health || exit 1

# Expose port
EXPOSE 8080

# Entry point
ENTRYPOINT ["dotnet", "HealthCheckExample.dll"]