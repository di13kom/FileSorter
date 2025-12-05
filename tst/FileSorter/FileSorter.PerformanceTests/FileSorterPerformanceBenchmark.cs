using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using FileSorter.Comparer;
using FileSorter.Sorter;

namespace FileSorter.PerformanceTests;

[MemoryDiagnoser]
public class FileSorterPerformanceBenchmark
{
    private readonly string _patternFileName = "sampleTestFile.txt";
    private string _mergeSortPatterFileName => $"mergeSort_{_patternFileName}";
    private string _bubbleSortPatterFileName => $"bubbleSort_{_patternFileName}";
    private string GetFullPath(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    private readonly ICustomLineComparer _comparer = new CustomLineComparer();

    #region Bubble Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithBubbleSortFileSorter))]
    public void GlobalSetupForBubbleSort()
    {
        var srcPath = _patternFileName;
        var bblSrtdstPath = GetFullPath(_bubbleSortPatterFileName);

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
        File.Delete(GetFullPath(_bubbleSortPatterFileName));
    }

    [Benchmark]
    public async Task Sort10KFileWithBubbleSortFileSorter()
    {
        var ctx = new CancellationTokenSource().Token;
        IFileSorter fileSorter = new BubbleFileSorter(GetFullPath(_bubbleSortPatterFileName), _comparer);
        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);
    }
    #endregion

    #region Merge Sort Setup&CleanUp
    [IterationSetup(Target = nameof(Sort10KFileWithMergeSortFileSorter))]
    public void GlobalSetupForMergeSort()
    {
        var srcPath = _patternFileName;
        var mrgSrtdstPath = GetFullPath(_mergeSortPatterFileName);

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
        foreach (var fl in Directory.GetFiles(GetFullPath(""), $"{_patternFileName}*", SearchOption.AllDirectories))
        {
            File.Delete(fl);
        }
    }

    #endregion


    [Benchmark]
    public async Task Sort10KFileWithMergeSortFileSorter()
    {
        ICanSort sorter = new MergeSort(_comparer);
        var ctx = new CancellationTokenSource().Token;

        IFileSorter fileSorter = new MergeFileSorter(GetFullPath(_mergeSortPatterFileName), sorter);

        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);
    }

}