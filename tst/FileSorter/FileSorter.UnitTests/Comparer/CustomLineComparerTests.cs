using FileSorter.Comparer;

namespace FileSorter.UnitTests.Comparer;

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
    public void Compare_Should_Return_true_If_string_match_but_str1_number_less_than_str0_number(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.EqualTo(1));
    }

    [TestCase("1. wohhxhskr", "6. wohhxhskr")]
    [TestCase("598. tkmxuyvhre", "608. tkmxuyvhre")]
    public void Compare_Should_Return_false_If_string_match_but_str1_number_greater_than_str0_number(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.LessThanOrEqualTo(0));
    }

    [TestCase("33. aaxtrwjrff", "433. acgjuvbpsf")]
    [TestCase("6. txhmsfnkbe", "9. txoicifmfh")]
    [TestCase("566. txhmsfnkbe", "749. txhmsfnkbz")]
    [TestCase("56. txhmsfnkbe", "79. txhmxfnkbz")]
    [TestCase("56. fcffffffff", "79. ffffffffff")]
    [TestCase("56. ffafffffff", "79. ffffffffff")]
    [TestCase("56. fffaffffff", "79. ffffffffff")]
    [TestCase("56. ffffafffff", "79. ffffffffff")]
    public void Compare_Should_Return_false_If_str0_less_str1_alphabetically(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.LessThanOrEqualTo(0));
    }

    [TestCase("5. amaxcobnsc", "256. almifvxdwi")]
    [TestCase("62. gijolfeplf", "151. gifdbxecif")]
    [TestCase("62. abaaaaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aacaaaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aaaraaaaaa", "151. aaaaaaaaaa")]
    [TestCase("62. aaaaiaaaaa", "151. aaaaaaaaaa")]
    public void Compare_Should_Return_true_If_str0_greater_str1_alphabetically(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.GreaterThan(0));
    }

    [TestCase("33. aaxtrwjrff", "43. aaxtrwjrffxxxxxxx")]
    [TestCase("13. aa", "23. aax")]
    [TestCase("33. ba", "43. bac")]
    public void Compare_Should_Return_false_If_str0_equals_str1_alphabetically_and_have_less_length(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.LessThanOrEqualTo(0));
    }

    [TestCase("33. aaxtrwjrffxxxxxxx", "43. aaxtrwjrff")]
    [TestCase("13. aax", "23. aa")]
    [TestCase("33. bac", "43. ba")]
    public void Compare_Should_Return_true_If_str0_equals_str1_alphabetically_and_have_greater_length(string str0, string str1)
    {
        Assert.That(_customLineComparer.Compare(str0, str1), Is.GreaterThan(0));
    }

    [TestCase("x. aaxtrwjrffxxxxxxx", "43. aaxtrwjrffxxxxxxx")]
    [TestCase("13. aax", "2y. aax")]
    [TestCase("Z3. bac", "MM. bac")]
    public void Compare_Should_throw_ArgumentException_if_word_part_match_and_digit_part_not_integer(string str0, string str1)
    {
        Assert.Throws(Is.TypeOf<ArgumentException>().And.Message.EqualTo("Error on parsing number part"), () => _customLineComparer.Compare(str0, str1));
    }

    [TestCase("x. aaxtrwjrffxxxxxxx", "")]
    [TestCase("", "2y. aax")]
    [TestCase("", "")]
    public void Compare_Should_throw_ArgumentException_if_str0_or_str1_is_empty(string str0, string str1)
    {
        Assert.Throws(Is.TypeOf<ArgumentException>().And.Message.EqualTo("Bad string"), () => _customLineComparer.Compare(str0, str1));
    }
}
