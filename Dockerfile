FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY . ./

# Restore dependencies
RUN dotnet restore "src/Anthropic/Anthropic.csproj"

# Build the library
RUN dotnet build "src/Anthropic/Anthropic.csproj" -c Release -o /app/build

# Create a simple test app to verify the library builds correctly
RUN mkdir -p /app/test && \
    printf 'using Anthropic;\nvar client = new AnthropicClient();\nConsole.WriteLine("Library loaded successfully");\n' > /app/test/Program.cs

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS test
WORKDIR /app/test
COPY --from=build /app/build/*.dll ./
COPY --from=build /app/test/Program.cs ./

RUN dotnet run

# Final stage - just the built library
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS final
WORKDIR /app

# Copy the built library
COPY --from=build /app/build/Anthropic.dll ./
COPY --from=build /app/build/Anthropic.pdb ./

# Optional: Copy the entire project for reference
COPY src/Anthropic/ ./src/Anthropic/

# The library is ready to be packaged or used
ENTRYPOINT ["echo", "Anthropic SDK library built successfully"]
