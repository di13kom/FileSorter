using FileSorter.Comparer;
using FileSorter.Extension;

namespace FileSorter.Sorter;

public class MergeFileSorter : IFileSorter
{
    /// <summary>
    /// Whether create subdirectories for temp file or not.
    /// </summary>
    private readonly bool IsUseDirectoryToSaveTempFiles = true;

    /// <summary>
    /// File name for sorting. Input parameter.
    /// </summary>
    private readonly string FileName;

    private readonly ICanSort Sorter;

    /// <summary>
    /// Count lines include to file on first iteration.
    /// </summary>
    private const int SplitFileLength = 1024;

    private string PartialFileName => $"{FileName}_part_";
    private string GetTempLevelDirectory(int level) => $"level{level}";

    public MergeFileSorter(string fileName, ICanSort sorter)
    {
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        Sorter = sorter ?? throw new ArgumentNullException(nameof(sorter));
    }

    public async Task SortFileAsync(long offset, CancellationToken token)
    {
        await SplitFile(SplitFileLength);

        await MergeSortFiles(0, token);
    }

    private async Task MergeSortFiles(int level, CancellationToken token)
    {
        string directoryPattern = IsUseDirectoryToSaveTempFiles ? Path.Combine(Path.GetDirectoryName(FileName), $"{GetTempLevelDirectory(level)}") : Path.GetDirectoryName(FileName);
        var targetFiles = Directory.GetFiles(directoryPattern, $"{Path.GetFileName(PartialFileName)}*");

        if (targetFiles.Length == 1)
            return;

        if (IsUseDirectoryToSaveTempFiles)
            Directory.CreateDirectory($"{GetTempLevelDirectory(level + 1)}");

        int halfLength = targetFiles.Length / 2;
        // for (int i = 0; i < halfLength; i++)
        await Parallel.ForAsync(0, halfLength, async (i, token) =>
        {
            string file0 = targetFiles[i * 2];
            string file1 = targetFiles[i * 2 + 1];
            await MergeFilesAsync(file0, file1, i, level);
            if (!IsUseDirectoryToSaveTempFiles)
            {
                File.Delete(file0);
                File.Delete(file1);
            }
        }
        );
        if (targetFiles.Length % 2 != 0)
        {
            MoveFileToLevelUpDirecotry(targetFiles[^1], level + 1);//last file when odd files count must be moved to level+1 directory.
        }

        if (IsUseDirectoryToSaveTempFiles)
        {
            Directory.Delete($"{GetTempLevelDirectory(level)}", true);
        }
        await MergeSortFiles(++level, token);
    }

    /// <summary>
    /// Move file to level+1 directory, when odd files count.
    /// </summary>
    /// <param name="targetFileName">File name.</param>
    /// <param name="level">Level name.</param>
    private void MoveFileToLevelUpDirecotry(string targetFileName, int level)
    {
        var path = Path.GetDirectoryName(targetFileName);
        var fileName = Path.GetFileName(targetFileName);
        var fullPath = IsUseDirectoryToSaveTempFiles ? Path.Combine(path[..path.LastIndexOf(Path.DirectorySeparatorChar)], GetTempLevelDirectory(level), fileName) : fileName;

        File.Move(targetFileName, fullPath);
    }

    private async Task MergeFilesAsync(string file0, string file1, int idx, int level)
    {
        using var streamReaderfile0 = new StreamReader(file0);
        using var streamReaderfile1 = new StreamReader(file1);
        string fileName = $"{PartialFileName}_level_{level + 1}_part_{idx}.tmp";
        string fullPath = IsUseDirectoryToSaveTempFiles ? Path.Combine(GetTempLevelDirectory(level + 1), fileName) : fileName;
        using var streamWriter = new StreamWriter(fullPath);

        string file0String = await streamReaderfile0.ReadLineAsync();
        string file1String = await streamReaderfile1.ReadLineAsync();

        while (file0String != null || file1String != null)
        {
            if (file0String == null)
            {
                await streamWriter.WriteLineAsync(file1String);
                file1String = await streamReaderfile1.ReadLineAsync();
            }
            else if (file1String == null)
            {
                await streamWriter.WriteLineAsync(file0String);
                file0String = await streamReaderfile0.ReadLineAsync();
            }
            else if (Sorter.Compare(file0String, file1String) > 0)
            {
                await streamWriter.WriteLineAsync(file1String);
                file1String = await streamReaderfile1.ReadLineAsync();
            }
            else
            {
                await streamWriter.WriteLineAsync(file0String);
                file0String = await streamReaderfile0.ReadLineAsync();
            }
        }
    }

    private async Task SplitFile(uint linesToSplit)
    {
        List<string> stringsQueue = [];
        using var streamReader = new StreamReader(FileName, new FileStreamOptions { Share = FileShare.None });//Exclusive access
        string? line;
        uint currentLineNumber = 0;
        int i = 0;
        if (IsUseDirectoryToSaveTempFiles)
        {
            Directory.CreateDirectory("level0");
        }

        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            stringsQueue.Add(line);
            if (++currentLineNumber % linesToSplit == 0)
            {
                await WriteStringsToFile(stringsQueue, i++);
                stringsQueue.Clear();
            }
        }
        if (stringsQueue.Count > 0)
        {
            await WriteStringsToFile(stringsQueue, i++);
        }
    }

    private async Task WriteStringsToFile(List<string> stringsQueue, int idx)
    {
        var sortedList = Sorter.Sort(stringsQueue);

        var fileName = $"{PartialFileName}{idx}.tmp";
        var fullPath = IsUseDirectoryToSaveTempFiles ? Path.Combine(GetTempLevelDirectory(0), fileName) : fileName;

        using var streamWriter = new StreamWriter(fullPath);
        {
            foreach (var item in sortedList)
            {

                await streamWriter.WriteLineAsync(item);
            }
            await streamWriter.FlushAsync();
        }
    }


}