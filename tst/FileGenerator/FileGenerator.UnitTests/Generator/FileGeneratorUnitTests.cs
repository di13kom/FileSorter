using System.Data.SqlTypes;
using FileGenerator.StringCreator;

namespace FileGenerator.UnitTests.Generator;

public class FileGeneratorUnitTests
{
    private readonly string FileName = "fileGeneratorTestFile.txt";

    [SetUp]
    public void Setup()
    {
        if (File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, FileName)))
        {
            File.Delete(Path.Combine(TestContext.CurrentContext.WorkDirectory, FileName));
        }
    }

    [Test]
    public async Task WriteFile_Should_create_file_with_size_not_less_specified()
    {
        string parsedFile = FileName;
        int bytesCount = 50_000;

        IStringCreator randomStringCreator = new OneByteCharRandomStringCreator(bytesCount);

        var writer = new FileGenerator.Generator.RandomStringFileGenerator(randomStringCreator, parsedFile);

        await writer.WriteFileAsync();


        DirectoryInfo di = new DirectoryInfo(TestContext.CurrentContext.WorkDirectory);
        FileInfo[] fiArr = di.GetFiles(FileName);
        Assert.Multiple(() =>
        {
            Assert.That(Path.Combine(TestContext.CurrentContext.WorkDirectory, FileName), Does.Exist);
            Assert.That(fiArr.FirstOrDefault()!.CreationTime, Is.GreaterThanOrEqualTo(DateTime.Now.Subtract(TimeSpan.FromMinutes(1))));
            Assert.That(fiArr.FirstOrDefault()!.Length, Is.GreaterThanOrEqualTo(bytesCount));
        });

    }
}
