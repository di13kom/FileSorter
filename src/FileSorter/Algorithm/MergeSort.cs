
using System.Diagnostics.CodeAnalysis;
using FileSorter.Comparer;

public class MergeSort : ICanSort
{
    private readonly ICustomLineComparer LineComparer;

    public MergeSort(ICustomLineComparer lineComparer)
    {
        LineComparer = lineComparer ?? throw new ArgumentNullException(nameof(lineComparer));
    }

    public int Compare(string string0, string string1) => LineComparer.Compare(string0, string1);

    public List<string> Sort([DisallowNull] List<string> stringsQueue)
    {
        ArgumentNullException.ThrowIfNull(stringsQueue);

        if (stringsQueue.Count <= 1)
            return stringsQueue;

        var length = stringsQueue.Count;
        int middle = length / 2;

        if (length == 1)
        {
            return stringsQueue;
        }
        if (length == 2)
        {
            if (LineComparer.Compare(stringsQueue[0], stringsQueue[1]) > 0)
            {
                return [stringsQueue[1], stringsQueue[0]];
            }
            else
            {
                return [stringsQueue[0], stringsQueue[1]];
            }
        }

        var subList0 = Sort(stringsQueue[..middle]);
        var subList1 = Sort(stringsQueue[middle..]);

        return MergeLists(subList0, subList1);
    }

    private List<string> MergeLists(List<string> stringsList0, List<string> stringsList1)
    {
        var length = stringsList0.Count + stringsList1.Count;
        List<string> retVal = [];
        int curlist0Idx = 0;
        int curlist1Idx = 0;

        for (int i = 0; i < length; i++)
        {
            if (curlist0Idx == stringsList0.Count)
            {
                retVal.Add(stringsList1.ElementAt(curlist1Idx++));
            }
            else if (curlist1Idx == stringsList1.Count)
            {
                retVal.Add(stringsList0.ElementAt(curlist0Idx++));
            }
            else if (LineComparer.Compare(stringsList0.ElementAt(curlist0Idx), stringsList1.ElementAt(curlist1Idx)) > 0)
            {
                retVal.Add(stringsList1.ElementAt(curlist1Idx++));
            }
            else
            {
                retVal.Add(stringsList0.ElementAt(curlist0Idx++));
            }
        }
        return retVal;
    }
}