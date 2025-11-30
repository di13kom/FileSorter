using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using FileSorter.Comparer;
using FileSorter.Sorter;

namespace FileSorter.PerformanceTests;

[MemoryDiagnoser]
public class FileSorterPerformanceBenchmark
{
    private readonly string PatternFileName = "sampleTestFile.txt";
    private string MergeSortPatterFileName => $"mergeSort_{PatternFileName}";
    private string BubbleSortPatterFileName => $"bubbleSort_{PatternFileName}";
    private string GetFullPath(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    private readonly ICustomLineComparer comparer = new CustomLineComparer();

    #region Bubble Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithBubbleSortFileSorter))]
    public void GlobalSetupForBubbleSort()
    {
        var srcPath = PatternFileName;
        var bblSrtdstPath = GetFullPath(BubbleSortPatterFileName);

        if (File.Exists(srcPath))
        {
            File.Copy(srcPath, bblSrtdstPath, true);
        }
        else
        {
            Console.Error.WriteLine($"File {srcPath} is not exists");
        }
    }

    [IterationCleanup(Target = nameof(Sort10KFileWithBubbleSortFileSorter))]
    public void CleanupForBubbleSort()
    {
        File.Delete(GetFullPath(BubbleSortPatterFileName));
    }

    [Benchmark]
    public async Task Sort10KFileWithBubbleSortFileSorter()
    {
        var ctx = new CancellationTokenSource().Token;
        IFileSorter fileSorter = new BubbleFileSorter(GetFullPath(BubbleSortPatterFileName), comparer);
        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);
    }
    #endregion

    #region Merge Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithMergeSortFileSorter))]
    public void GlobalSetupForMergeSort()
    {
        var srcPath = PatternFileName;
        var mrgSrtdstPath = GetFullPath(MergeSortPatterFileName);

        if (File.Exists(srcPath))
        {
            File.Copy(srcPath, mrgSrtdstPath, true);
        }
        else
        {
            Console.Error.WriteLine($"File {srcPath} is not exists");
        }
    }

    [IterationCleanup(Target = nameof(Sort10KFileWithMergeSortFileSorter))]
    public void CleanupForMergeSort()
    {
        foreach (var fl in Directory.GetFiles(GetFullPath(""), $"{PatternFileName}*", SearchOption.AllDirectories))
        {
            File.Delete(fl);
        }
    }

    #endregion


    [Benchmark]
    public async Task Sort10KFileWithMergeSortFileSorter()
    {
        ICanSort sorter = new MergeSort(comparer);
        var ctx = new CancellationTokenSource().Token;

        IFileSorter fileSorter = new MergeFileSorter(GetFullPath(MergeSortPatterFileName), sorter);

        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);
    }

}