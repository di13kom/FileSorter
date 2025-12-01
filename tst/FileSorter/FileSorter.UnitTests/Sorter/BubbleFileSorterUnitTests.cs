using FileSorter.Comparer;
using FileSorter.Sorter;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FileSorter.UnitTests.Sorter;

public class BubbleFileSorterUnitTests
{
    private readonly string bubblePrefix = "bb_";
    private readonly string PatternFileName = "sampleTestFile.txt";
    private readonly string PatternSortedFileName = "sampleTestFile_sorted.txt";
    private string GetPath(string fileName) => Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);
    [SetUp]
    public void Setup()
    {
        File.Copy(GetPath(PatternFileName), GetPath($"{bubblePrefix}{PatternFileName}"), true);
    }

    [Test]
    public async Task SortFileAsync_Should_sort_file()
    {
        ICustomLineComparer comparer = new CustomLineComparer();
        var ctx = new CancellationTokenSource().Token;
        IFileSorter fileSorter = new BubbleFileSorter(GetPath($"{bubblePrefix}{PatternFileName}"), comparer);
        await fileSorter.SortFileAsync(ctx).ConfigureAwait(false);

        FileAssert.AreEqual(GetPath($"{bubblePrefix}{PatternFileName}"), GetPath(PatternSortedFileName));
    }
}
