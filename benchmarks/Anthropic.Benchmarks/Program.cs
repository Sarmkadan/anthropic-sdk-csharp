using BenchmarkDotNet.Running;
using Anthropic.Benchmarks.Benchmarks;

var switcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);

if (args.Length == 0)
{
    Console.WriteLine("Available benchmarks:");
    Console.WriteLine("1. MessageBenchmarks - Benchmarks for message creation and serialization");
    Console.WriteLine("2. ClientBenchmarks - Benchmarks for client initialization and configuration");
    Console.WriteLine("\nRun specific benchmark with: dotnet run -- --filter *BenchmarkClassName*");
    Console.WriteLine("Run all benchmarks with: dotnet run -- --all");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  dotnet run -- --filter *MessageBenchmarks*");
    Console.WriteLine("  dotnet run -- --filter *ClientBenchmarks*");
    return;
}

switcher.Run(args);
