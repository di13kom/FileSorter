namespace FileSorter.Comparer;

public interface ICustomLineComparer
{
    bool IsReplaceRequired(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1);
}
