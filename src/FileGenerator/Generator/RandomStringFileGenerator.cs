using System;
using FileGenerator.StringCreator;

namespace FileGenerator.Generator;

public class RandomStringFileGenerator
{
    /// <summary>
    /// String creator.
    /// </summary>
    private readonly IStringCreator _stringCreator;

    /// <summary>
    /// File name for sorting. Input parameter.
    /// </summary>
    private readonly string _fileName;

    /// <summary>
    /// buffering. Experimental.
    /// </summary>
    const int BufferFlushSum = 200;

    public RandomStringFileGenerator(IStringCreator stringCreator, string fileName)
    {
        _stringCreator = stringCreator;
        _fileName = fileName;
    }

    public async Task WriteFileAsync()
    {
        int i = 1;
        using FileStream fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.Write);
        fileStream.Seek(0, SeekOrigin.Begin);

        using StreamWriter streamWriter = new StreamWriter(fileStream);

        foreach (var item in _stringCreator.GetLines())
        {
            await streamWriter.WriteLineAsync(item);

            if (i++ % BufferFlushSum == 0)
            {
                await streamWriter.FlushAsync();
            }
        }
    }
}
