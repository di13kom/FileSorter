namespace FileGenerator.StringCreator;

public interface IStringCreator
{
    /// <summary>
    /// Create line for writing to a file.
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> GetLines();
}
