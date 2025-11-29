using System.CommandLine;
using FileGenerator.Generator;
using FileGenerator.StringCreator;

internal partial class Program
{
    const int DefaultBytesValue = 1_000_000;
    private static int Main(string[] args)
    {
        Option<string> fileToWrite = new Option<string>("--file", ["-f", "/f"])
        {
            Description = "Sample filename.",
            Required = true
        };
        Option<int> bytesToWrite = new Option<int>("--bytes", ["-b", "/b"])
        {
            Description = "bytes write to file.",
            Required = false,
            DefaultValueFactory = parseResult => DefaultBytesValue
        };

        RootCommand rootCommand = new RootCommand("Generate sample file app.");
        rootCommand.Options.Add(fileToWrite);
        rootCommand.Options.Add(bytesToWrite);


        rootCommand.SetAction(async parseResult =>
        {
            string parsedFile = parseResult.GetValue(fileToWrite)!;//Option is required. not null
            int bytesCount = parseResult.GetValue(bytesToWrite);

            IStringCreator randomStringCreator = new OneByteCharRandomStringCreator(bytesCount);

            var writer = new RandomStringFileGenerator(randomStringCreator, parsedFile);

            await writer.WriteFileAsync();
            return 0;
        });

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

}