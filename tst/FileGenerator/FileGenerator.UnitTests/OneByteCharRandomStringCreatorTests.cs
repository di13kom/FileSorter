using System.Diagnostics;
using FileGenerator.StringCreator;

namespace FileGenerator.UnitTests;

public class OneByteCharRandomStringCreatorTests
{
    private const int BytesCount = 500;
    private IStringCreator _oneByteCharRandomStringCreator;

    [SetUp]
    public void Setup()
    {
        _oneByteCharRandomStringCreator = new OneByteCharRandomStringCreator(BytesCount);
    }

    [Test]
    public void GetLines_item_Should_match_format()
    {
        foreach (var item in _oneByteCharRandomStringCreator.GetLines())
        {
            Assert.That(item, Does.Match(OneByteCharRandomStringCreator.LineFormat));
            Debug.WriteLine(item);
        }
    }

}
