namespace FileSorter.Comparer;

/// <summary>
/// Exposes a method that compares two objects.
/// </summary>
public interface ICustomLineComparer
{
    /// <summary>
    /// Parse string and returns a value indicating whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="str0">The first object to compare</param>
    /// <param name="str1">The second object to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative values of str0 and str1:
    /// - If less than 0, str0 is less than str1.
    /// - If 0, str0 equals str1.
    /// - If greater than 0, str0 is greater than str1.
    /// </returns>
    int Compare(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1);
}
