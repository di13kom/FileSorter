using BenchmarkDotNet.Running;
using FileSorter.PerformanceTests;

internal class Program
{
    private static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FileSorterPerformanceBenchmark>();
    }
}
