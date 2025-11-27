using System.CommandLine;
using FileSorter.Comparer;
using FileSorter.Sorter;

internal class Program
{
    private static int Main(string[] args)
    {
        Option<string> fileToWrite = new Option<string>("--file", ["-f", "/f"])
        {
            Description = "Sample filename.",
            Required = true
        };

        RootCommand rootCommand = new RootCommand("Sort sample file app.");
        rootCommand.Options.Add(fileToWrite);

        rootCommand.SetAction(async parseResult =>
        {
            string parsedFile = parseResult.GetValue(fileToWrite)!;//Option is required. not null
            if (!File.Exists(parsedFile))
            {
                Console.Error.WriteLine("File not found.");
                return -1;
            }
            var comparer = new CustomLineComparer();
            IFileSorter sortFile = new BubbleFileSorter(parsedFile, comparer);

            await sortFile.SortFileAsync(0);
            return 0;
        });

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

}