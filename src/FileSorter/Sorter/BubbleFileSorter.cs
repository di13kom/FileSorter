using System.Text;
using FileSorter.Comparer;

namespace FileSorter.Sorter;

/// <summary>
/// Sort file with bubble sorting algorithm.Sort file in-place.
/// </summary>
public class BubbleFileSorter : IFileSorter
{
    /// <summary>
    /// File name for sorting.
    /// </summary>
    private readonly string FileName;

    /// <summary>
    /// Start file position offset.
    /// </summary>
    private readonly int StartFileOffset = 0;

    private readonly ICustomLineComparer LineComparer;

    public BubbleFileSorter(string parsedFile, ICustomLineComparer lineComparer)
    {
        FileName = parsedFile ?? throw new ArgumentNullException(nameof(parsedFile));
        LineComparer = lineComparer ?? throw new ArgumentNullException(nameof(lineComparer));
    }

    /// <inheritdoc/>
    public async Task SortFileAsync(CancellationToken token)
    {
        bool IsReplaceOccured = true;
        using FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        fileStream.Seek(StartFileOffset, SeekOrigin.Begin);

        using StreamReader streamReader = new StreamReader(fileStream);
        using StreamWriter streamWriter = new StreamWriter(fileStream);

        long preString0position = StartFileOffset;
        long preString1Position = StartFileOffset;
        long lastSortedPosition = -1;

        preString0position = fileStream.Position;
        var string0 = await streamReader.ReadLineAsync(token);
        if (string0 is null)
            return;
        preString1Position = preString0position + string0.Length + Environment.NewLine.Length;
        while (true)
        {
            var string1 = await streamReader.ReadLineAsync(token);
            var curPos = preString1Position + string1?.Length + Environment.NewLine.Length ?? 0;

            if (string1 is null || curPos == lastSortedPosition)
            {
                if (IsReplaceOccured == true)
                {
                    lastSortedPosition = preString1Position;
                    IsReplaceOccured = false;
                    fileStream.Seek(StartFileOffset, SeekOrigin.Begin);
                    streamReader.DiscardBufferedData();

                    string0 = await streamReader.ReadLineAsync(token);
                    preString0position = StartFileOffset;
                    preString1Position = preString0position + string0.Length + Environment.NewLine.Length;
                    continue;
                }
                else
                {
                    break;
                }

            }

            if (LineComparer.Compare(string0, string1) > 0)
            {
                IsReplaceOccured = true;
                fileStream.Seek(preString0position, SeekOrigin.Begin);

                await streamWriter.WriteLineAsync(string1);
                await streamWriter.WriteLineAsync(string0);
                await streamWriter.FlushAsync(token);
                if (string1.Length > string0.Length)
                {
                    preString1Position += string1.Length - string0.Length;
                }
                else if (string1.Length < string0.Length)
                {
                    preString1Position -= string0.Length - string1.Length;
                }
                string1 = string0;
                streamReader.DiscardBufferedData();
            }
            string0 = string1;
            preString0position = preString1Position;
            preString1Position = curPos;
        }
    }
}