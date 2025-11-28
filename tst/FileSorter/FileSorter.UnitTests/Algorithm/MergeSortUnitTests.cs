using FileSorter.Comparer;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FileSorter.UnitTests;

public class MergeSortUnitTests
{
    private ICanSort _mergeSorter;
    [SetUp]
    public void Setup()
    {
        ICustomLineComparer lineComparer = new CustomLineComparer();
        _mergeSorter = new MergeSort(lineComparer);
    }

    [TestCaseSource(nameof(GetTestCaseDatas))]
    public void Sort_method_should_sort_as_expected((List<string> input, List<string> expected) td)
    {
        Assert.That(td.expected, Is.EqualTo(_mergeSorter.Sort(td.input)).AsCollection);
    }

    [Test]
    public void Sort_method_should_throw_ArgumentNullException_on_null_argument()
    {
        Assert.Throws<ArgumentNullException>(() => _mergeSorter.Sort(null));
    }

    private static IEnumerable<(List<string>, List<string>)> GetTestCaseDatas()
    {
        yield return ([], []);
        yield return (["1. lzetodxmsf", "2. jqtzmuwvbg", "3. halcueruae", "4. nnmwblpqr", "5. zdpjgccwbe",],
                        ["3. halcueruae", "2. jqtzmuwvbg", "1. lzetodxmsf", "4. nnmwblpqr", "5. zdpjgccwbe",]);
        yield return (["1. aaaaa", "2. aaaa", "3. aaa", "4. aa", "5. a"], ["5. a", "4. aa", "3. aaa", "2. aaaa", "1. aaaaa"]);
        yield return (["1. f", "2. e", "3. d", "4. c", "5. b", "6. a"], ["6. a", "5. b", "4. c", "3. d", "2. e", "1. f"]);
        yield return (["1. aaaaa", "2. aaaaa", "3. aaaaa", "4. aaaaa", "5. aaaaa"], ["1. aaaaa", "2. aaaaa", "3. aaaaa", "4. aaaaa", "5. aaaaa"]);
    }
}
