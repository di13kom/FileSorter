using FileSorter.Comparer;
using FileSorter.Sorter;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FileSorter.UnitTests.Sorter;

public class BubbleFileSorterUnitTests
{
    private readonly string _bubblePrefix = "bb_";
    private readonly string _patternFileName = "sampleTestFile.txt";
    private readonly string _patternSortedFileName = "sampleTestFile_sorted.txt";
    private string GetPath(string fileName) => Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);
    [SetUp]
    public void Setup()
    {
        File.Copy(GetPath(_patternFileName), GetPath($"{_bubblePrefix}{_patternFileName}"), true);
    }

    [Test]
    public async Task SortFileAsync_Should_sort_file()
    {
        ICustomLineComparer comparer = new CustomLineComparer();
        var ctx = new CancellationTokenSource().Token;
        IFileSorter fileSorter = new BubbleFileSorter(GetPath($"{_bubblePrefix}{_patternFileName}"), comparer);
        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);

        FileAssert.AreEqual(GetPath($"{_bubblePrefix}{_patternFileName}"), GetPath(_patternSortedFileName));
    }
}
