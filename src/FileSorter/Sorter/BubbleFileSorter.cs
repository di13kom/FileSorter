using System.Text;
using FileSorter.Comparer;

namespace FileSorter.Sorter;

public class BubbleFileSorter : IFileSorter
{
    /// <summary>
    /// File name for sorting.
    /// </summary>
    private readonly string FileName;

    private readonly ICustomLineComparer LineComparer;

    public BubbleFileSorter(string parsedFile, ICustomLineComparer lineComparer)
    {
        FileName = parsedFile ?? throw new ArgumentNullException(nameof(parsedFile));
        LineComparer = lineComparer ?? throw new ArgumentNullException(nameof(lineComparer));
    }

    /// <inheritdoc/>
    public async Task SortFileAsync(long offset)
    {
        bool IsReplaceOccured = true;
        using FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        fileStream.Seek(offset, SeekOrigin.Begin);

        using StreamReader streamReader = new StreamReader(fileStream);
        using StreamWriter streamWriter = new StreamWriter(fileStream);

        //TODO:RemoveIt.
        const int bufferSize = 48; // read the file in 4KB chunks
        var builder = new StringBuilder();
        char[] buffer = new char[bufferSize];
        int bytesRead;
        //

        long position0 = 0;
        long position1 = 0;
        while (true)
        {
            position0 = fileStream.Position;
            var string0 = await streamReader.ReadLineAsync();
            position1 = position0 + string0.Length + Environment.NewLine.Length;

            //TODO:RemoveIt.
            // // bytesRead = await streamReader.ReadBlockAsync(buffer, 0, buffer.Length);
            // // var position1 = bytesRead;
            // builder.Append(buffer, 0, bytesRead);

            var string1 = await streamReader.ReadLineAsync();

            if (string1 is null)
            {
                if (IsReplaceOccured == true)
                {
                    IsReplaceOccured = false;
                    fileStream.Seek(0, SeekOrigin.Begin);
                    continue;
                }
                else
                {
                    break;
                }

            }

            if (LineComparer.IsReplaceRequired(string0.AsSpan(), string1.AsSpan()))
            {
                IsReplaceOccured = true;
                fileStream.Seek(position0, SeekOrigin.Begin);

                await streamWriter.WriteLineAsync(string1);
                await streamWriter.WriteLineAsync(string0);
                streamWriter.Flush();
                if (string1.Length > string0.Length)
                {
                    position1 += string1.Length - string0.Length;
                }
                else if (string1.Length < string0.Length)
                {
                    position1 -= string0.Length - string1.Length;
                }
            }
            fileStream.Seek(position1, SeekOrigin.Begin);
            streamReader.DiscardBufferedData();
            // fileStream.Position = position1;
        }
    }
}