namespace FileSorter.Sorter;

public interface IFileSorter
{
    /// <summary>
    /// Sort file implementation.
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    Task SortFileAsync(long offset);
}
