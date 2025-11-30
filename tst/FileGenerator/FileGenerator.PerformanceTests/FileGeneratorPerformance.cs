using BenchmarkDotNet.Attributes;
using FileGenerator.StringCreator;

namespace FileGenerator.PerformanceTests;

[MemoryDiagnoser]
public class FileGeneratorPerformanceBenchmark
{

    public FileGeneratorPerformanceBenchmark()
    {
    }

    [Benchmark]
    public async Task FileGeneratorBenchmarkWith100MFileSize()
    {
        string parsedFile = "fileGenerator_testFile_100M.txt";
        int bytesCount = 100_000_000;

        IStringCreator randomStringCreator = new OneByteCharRandomStringCreator(bytesCount);

        var writer = new Generator.RandomStringFileGenerator(randomStringCreator, parsedFile);

        await writer.WriteFileAsync();
    }

    [Benchmark]
    public async Task FileGeneratorBenchmarkWith1MFileSize()
    {
        string parsedFile = "fileGenerator_testFile1M.txt";
        int bytesCount = 1_000_000;

        IStringCreator randomStringCreator = new OneByteCharRandomStringCreator(bytesCount);

        var writer = new Generator.RandomStringFileGenerator(randomStringCreator, parsedFile);

        await writer.WriteFileAsync();
    }
}
