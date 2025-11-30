namespace FileSorter.Sorter;

public interface IFileSorter
{
    /// <summary>
    /// Sort file implementation.
    /// </summary>
    /// <param name="token">Cancelation token</param>
    /// <returns></returns>
    Task SortFileAsync(CancellationToken token);
}
