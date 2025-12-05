using FileSorter.Comparer;
using FileSorter.Sorter;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FileSorter.UnitTests.Sorter;

public class MergeFileSorterUnitTests
{
    private readonly string _mergePrefix = "ms_";
    private readonly string _mergeSortedSuffix = ".tmp";
    private readonly string _patternFileName = "sampleTestFile.txt";
    private readonly string _patternSortedFileName = "sampleTestFile_sorted.txt";
    private string MsSortedIntermediateFiles() => $"{_mergePrefix}{_patternFileName}*{_mergeSortedSuffix}";
    private string GetPath(string fileName) => Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);

    [SetUp]
    public void Setup()
    {
        File.Copy(GetPath(_patternFileName), GetPath($"{_mergePrefix}{_patternFileName}"), true);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var item in Directory.GetFiles(GetPath(string.Empty), MsSortedIntermediateFiles(), SearchOption.AllDirectories))
        {
            File.Delete(item);
        }
    }


    [Test]
    public async Task SortFileAsync_Should_sort_file()
    {
        ICustomLineComparer comparer = new CustomLineComparer();
        ICanSort sorter = new MergeSort(comparer);
        var ctx = new CancellationTokenSource().Token;

        IFileSorter fileSorter = new MergeFileSorter(GetPath($"{_mergePrefix}{_patternFileName}"), sorter);

        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);

        var sortedFiles = Directory.GetFiles(GetPath(string.Empty), MsSortedIntermediateFiles(), SearchOption.AllDirectories);

        FileAssert.AreEqual(sortedFiles.First(), GetPath(_patternSortedFileName));
    }
}
