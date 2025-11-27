using System;

namespace FileSorter.Comparer;

public class CustomLineComparer : ICustomLineComparer
{
    private const string DotSpacePattern = ". ";

    public bool IsReplaceRequired(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1)
    {
        var idx0 = str0.IndexOf(DotSpacePattern);
        var idx1 = str1.IndexOf(DotSpacePattern);

        var numberStr0 = str0[0..idx0];
        var numberStr1 = str1[0..idx1];

        var wordsStr0 = str0[(idx0 + 2)..];
        var wordsStr1 = str1[(idx1 + 2)..];

        var result = CompareWords(wordsStr0, wordsStr1);

        if (result == -1)
            return false;
        if (result == 1)
            return true;
        else
        {
            return CompareNumbers(numberStr0, numberStr1);
        }

    }

    private bool CompareNumbers(ReadOnlySpan<char> numberStr0, ReadOnlySpan<char> numberStr1)
    {
        if (int.TryParse(numberStr0, out int number0) && int.TryParse(numberStr1, out int number1))
        {
            if (number0 > number1)
                return true;
            else
                return false;
        }

        return false;
    }

    /// <summary>
    /// Compares two specified strings alphabetically.
    /// </summary>
    /// <param name="str0"></param>
    /// <param name="str1"></param>
    /// <returns></returns>
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