using System;

namespace FileSorter.Comparer;

public class CustomLineComparer : ICustomLineComparer
{
    private const string DotSpacePattern = ". ";

    ///<inheritdoc/>
    public int Compare(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1)
    {
        var idx0 = str0.IndexOf(DotSpacePattern);
        var idx1 = str1.IndexOf(DotSpacePattern);

        if (idx0 < 0 || idx1 < 0)
            throw new ArgumentException("Bad string");

        var numberStr0 = str0[0..idx0];
        var numberStr1 = str1[0..idx1];

        var wordsStr0 = str0[(idx0 + 2)..];
        var wordsStr1 = str1[(idx1 + 2)..];

        var result = CompareWords(wordsStr0, wordsStr1);

        if (result == 0)
            return CompareNumbers(numberStr0, numberStr1);
        else
            return result;
    }

    /// <summary>
    /// Parse values to `Int32` and returns a value indicating whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="numberStr0">The first object to compare</param>
    /// <param name="numberStr1">The second object to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative values of numberStr0 and numberStr1:
    /// - If less than 0, numberStr0 is less than numberStr1.
    /// - If 0, numberStr0 equals numberStr1.
    /// - If greater than 0, numberStr0 is greater than numberStr1.
    /// </returns>
    /// <enumberStr0ception cref="ArgumentException">Exception on `Int32` parse error.</exception>
    private int CompareNumbers(ReadOnlySpan<char> numberStr0, ReadOnlySpan<char> numberStr1)
    {
        if (int.TryParse(numberStr0, out int number0) && int.TryParse(numberStr1, out int number1))
        {
            if (number0 < number1)
                return -1;
            else if (number0 == number1)
                return 0;
            else
                return 1;
        }

        throw new ArgumentException("Error on parsing number part");
    }

    /// <summary>
    /// Compares two specified strings alphabetically.
    /// </summary>
    /// <param name="str0">The first object to compare</param>
    /// <param name="str1">The second object to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative values of x and y:
    /// - If less than 0, x is less than y.
    /// - If 0, x equals y.
    ///- If greater than 0, x is greater than y.
    /// </returns>
    private int CompareWords(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1)
    {
        var length = str0.Length > str1.Length ? str1.Length : str0.Length;

        for (int i = 0; i < length; i++)
        {
            if (str0[i] > str1[i])
            {
                return 1;
            }
            else if (str0[i] < str1[i])
            {
                return -1;
            }
        }
        if (str0.Length > str1.Length)
        {
            return 1;
        }
        else if (str0.Length < str1.Length)
        {
            return -1;
        }
        return 0;
    }
}