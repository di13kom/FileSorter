using System;
using FileGenerator.StringCreator;

namespace FileGenerator.Generator;

public class RandomStringFileGenerator
{
    /// <summary>
    /// String creator.
    /// </summary>
    private readonly IStringCreator StringCreator;

    /// <summary>
    /// File name for sorting. Input parameter.
    /// </summary>
    private readonly string FileName;

    /// <summary>
    /// buffering. Experimental.
    /// </summary>
    const int BufferFlushSum = 200;

    public RandomStringFileGenerator(IStringCreator stringCreator, string fileName)
    {
        StringCreator = stringCreator;
        FileName = fileName;
    }

    public async Task WriteFileAsync()
    {
        int i = 1;
        using FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
        fileStream.Seek(0, SeekOrigin.Begin);

        using StreamWriter streamWriter = new StreamWriter(fileStream);

        foreach (var item in StringCreator.GetLines())
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
