using BenchmarkDotNet.Running;

namespace FileGenerator.PerformanceTests;

internal class Program
{
    private static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FileGeneratorPerformanceBenchmark>();
    }
}