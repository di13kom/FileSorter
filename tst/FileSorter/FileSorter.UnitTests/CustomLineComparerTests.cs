using FileSorter.Comparer;

namespace FileSorter.UnitTests;

public class CustomLineComparerTests
{
    private CustomLineComparer _customLineComparer;
    [SetUp]
    public void Setup()
    {
        _customLineComparer = new CustomLineComparer();
    }

    [TestCase("23. wvqsbppupi", "4. wvqsbppupi")]
    [TestCase("5. whaaagnjr", "3. whaaagnjr")]
    public void IsReplaceRequired_Should_Return_true_If_string_match_but_str1_number_less_than_str0_number(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(true));
    }

    [TestCase("1. wohhxhskr", "6. wohhxhskr")]
    [TestCase("598. tkmxuyvhre", "608. tkmxuyvhre")]
    public void IsReplaceRequired_Should_Return_false_If_string_match_but_str1_number_greater_than_str0_number(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(false));
    }

    [TestCase("33. aaxtrwjrff", "433. acgjuvbpsf")]
    [TestCase("6. txhmsfnkbe", "9. txoicifmfh")]
    [TestCase("566. txhmsfnkbe", "749. txhmsfnkbz")]
    [TestCase("56. txhmsfnkbe", "79. txhmxfnkbz")]
    [TestCase("56. fcffffffff", "79. ffffffffff")]
    [TestCase("56. ffafffffff", "79. ffffffffff")]
    [TestCase("56. fffaffffff", "79. ffffffffff")]
    [TestCase("56. ffffafffff", "79. ffffffffff")]
    public void IsReplaceRequired_Should_Return_false_If_str0_less_str1_alphabetically(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(false));
    }

    [TestCase("5. amaxcobnsc", "256. almifvxdwi")]
    [TestCase("62. gijolfeplf", "151. gifdbxecif")]
    [TestCase("62. abaaaaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aacaaaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aaaraaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aaaaiaaaaa", "151. aaaaaaaaaa")]
    public void IsReplaceRequired_Should_Return_true_If_str0_greater_str1_alphabetically(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(true));
    }

    [TestCase("33. aaxtrwjrff", "43. aaxtrwjrffxxxxxxx")]
    [TestCase("13. aa", "23. aax")]
    [TestCase("33. ba", "43. bac")]
    public void IsReplaceRequired_Should_Return_false_If_str0_equals_str1_alphabetically_and_have_less_length(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(false));
    }

    [TestCase("33. aaxtrwjrffxxxxxxx", "43. aaxtrwjrff")]
    [TestCase("13. aax", "23. aa")]
    [TestCase("33. bac", "43. ba")]
    public void IsReplaceRequired_Should_Return_true_If_str0_equals_str1_alphabetically_and_have_greater_length(string str0, string str1)
    {
        Assert.That(_customLineComparer.IsReplaceRequired(str0, str1), Is.EqualTo(true));
    }
}
