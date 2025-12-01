using FileSorter.Comparer;
using FileSorter.Sorter;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FileSorter.UnitTests.Sorter;

public class MergeFileSorterUnitTests
{
    private readonly string MergePrefix = "ms_";
    private readonly string MergeSortedSuffix = ".tmp";
    private readonly string PatternFileName = "sampleTestFile.txt";
    private readonly string PatternSortedFileName = "sampleTestFile_sorted.txt";
    private string MsSortedIntermediateFiles() => $"{MergePrefix}{PatternFileName}*{MergeSortedSuffix}";
    private string GetPath(string fileName) => Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);

    [SetUp]
    public void Setup()
    {
        File.Copy(GetPath(PatternFileName), GetPath($"{MergePrefix}{PatternFileName}"), true);
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

        IFileSorter fileSorter = new MergeFileSorter(GetPath($"{MergePrefix}{PatternFileName}"), sorter);

        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);

        var sortedFiles = Directory.GetFiles(GetPath(string.Empty), MsSortedIntermediateFiles(), SearchOption.AllDirectories);

        FileAssert.AreEqual(sortedFiles.First(), GetPath(PatternSortedFileName));
    }
}
