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

    public RandomStringFileGenerator(IStringCreator stringCreator, string fileName)
    {
        _stringCreator = stringCreator ?? throw new ArgumentNullException(nameof(stringCreator));
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
    }

    public async Task WriteFileAsync(CancellationToken cancellationToken = default)
    {
        using var fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);
        using var streamWriter = new StreamWriter(fileStream);
        
        foreach (var line in _stringCreator.GetLines())
        {
            cancellationToken.ThrowIfCancellationRequested();
            await streamWriter.WriteLineAsync(line);
        }
    }
}