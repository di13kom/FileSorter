using BenchmarkDotNet.Attributes;
using FileSorter.Comparer;
using FileSorter.Sorter;

namespace FileSorter.PerformanceTests;

[MemoryDiagnoser]
public class FileSorterPerformanceBenchmark
{
    private readonly string _patternFileName = "sampleTestFile.txt";
    private string SourceFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _patternFileName);
    private string BubbleSortTestFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"bubble_test_{_patternFileName}");
    private string MergeSortTestFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"merge_test_{_patternFileName}");
    private readonly ICustomLineComparer _comparer = new CustomLineComparer();

    #region Bubble Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithBubbleSortFileSorter))]
    public void GlobalSetupForBubbleSort()
    {
        File.Copy(SourceFilePath, BubbleSortTestFile, overwrite: true);
    }

    [IterationCleanup(Target = nameof(Sort10KFileWithBubbleSortFileSorter))]
    public void CleanupForBubbleSort()
    {
        if (File.Exists(BubbleSortTestFile))
            File.Delete(BubbleSortTestFile);
    }

    [Benchmark]
    public async Task Sort10KFileWithBubbleSortFileSorter()
    {
        using var cts = new CancellationTokenSource();
        IFileSorter sorter = new BubbleFileSorter(BubbleSortTestFile, _comparer);
        await sorter.SortFileAsync(cts.Token).ConfigureAwait(false);
    }
    #endregion

    #region Merge Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithMergeSortFileSorter))]
    public void GlobalSetupForMergeSort()
    {
        File.Copy(SourceFilePath, MergeSortTestFile, overwrite: true);
    }

    [IterationCleanup(Target = nameof(Sort10KFileWithMergeSortFileSorter))]
    public void CleanupForMergeSort()
    {
        // Delete test file and all merge temporary files
        var directory = Path.GetDirectoryName(MergeSortTestFile)!;
        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(MergeSortTestFile);

        foreach (var file in Directory.GetFiles(directory, $"{fileNameWithoutExt}*", SearchOption.TopDirectoryOnly))
        {
            try { File.Delete(file); } catch { /* Ignore cleanup errors */ }
        }
    }

    #endregion


    [Benchmark]
    public async Task Sort10KFileWithMergeSortFileSorter()
    {
        using var cts = new CancellationTokenSource();
        var mergeSorter = new MergeSort(_comparer);
        var sorter = new MergeFileSorter(MergeSortTestFile, mergeSorter);
        await sorter.SortFileAsync(cts.Token).ConfigureAwait(false);
    }
}