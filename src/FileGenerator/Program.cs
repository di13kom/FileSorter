using System.CommandLine;
using System.Threading.Tasks;
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


        rootCommand.SetAction(parseResult =>
        {
            string parsedFile = parseResult.GetValue(fileToWrite)!;//Option is required. not null
            int bytesCount = parseResult.GetValue(bytesToWrite);

            OneByteCharRandomStringCreator randomStringCreator = new OneByteCharRandomStringCreator(bytesCount);

            WriteFile(parsedFile!, 0, randomStringCreator);
            return 0;
        });

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

    private static async Task WriteFile(string parsedFile, long offset, IStringCreator stringCreator)
    {
        int i = 1;
        const int BufferFlushSum = 200;//buffering. Experimental.
        using FileStream fileStream = new FileStream(parsedFile, FileMode.Create, FileAccess.Write, FileShare.Write);
        fileStream.Seek(offset, SeekOrigin.Begin);

        using StreamWriter streamWriter = new StreamWriter(fileStream);

        foreach (var item in stringCreator.GetLines())
        {
            await streamWriter.WriteLineAsync(item);

            //TODO: Fix
            if (i++ % BufferFlushSum == 0)
            {
                await streamWriter.FlushAsync();
            }
        }

    }
}