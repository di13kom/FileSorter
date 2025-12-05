namespace FileGenerator.StringCreator;

using System.Text;

/// <summary>
/// Random string creator, using one byte ASCII characters.
/// </summary>
public class OneByteCharRandomStringCreator : IStringCreator
{
    /// <summary>
    /// Line format regex.
    /// </summary>
    public static string LineFormat => @"\d{1,}\.\s\w+";

    /// <summary>
    /// Frequency repeating operation.
    /// </summary>
    private const int RepeatFrequency = 233;

    /// <summary>
    /// Quantity elements participating in repeat operation.
    /// </summary>
    private const int RepeatCountElements = 5;

    /// <summary>
    /// Count bytes.
    /// </summary>
    private readonly int _bytesCount;
    private readonly Random _rndDevice;
    
    /// <summary>
    /// Repeated words queue.
    /// </summary>
    private readonly List<string> _repeatedWordsQueue;

    public OneByteCharRandomStringCreator(int bytesCount)
    {
        _bytesCount = bytesCount;
        _rndDevice = new Random();
        _repeatedWordsQueue = [];
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetLines()
    {
        int currentBytes = 0;
        StringBuilder sb = new StringBuilder();
        int i = 1;
        do
        {
            sb.Clear();
            CreateRandomString(sb, i);

            sb.Insert(0, $"{i++}. ");//Insert line number.
            currentBytes += sb.Length + Environment.NewLine.Length;//sb.length bytes + \r\n symbols
            yield return sb.ToString();
        }
        while (currentBytes <= _bytesCount);
    }

    /// <summary>
    /// Create string with repetition. 
    /// </summary>
    /// <param name="sb">String builder where string created.</param>
    /// <param name="idx">Current index for word repeating process.</param>
    private void CreateRandomString(StringBuilder sb, int idx)
    {
        #region Repetition strings
        if (idx % RepeatFrequency == 0)
        {
            var cnt = _rndDevice.Next(0, RepeatCountElements);
            sb.Append(_repeatedWordsQueue.ElementAt(cnt));
            return;
        }
        #endregion

        var vl = _rndDevice.NextInt64(1, long.MaxValue);
        while (vl > 0)
        {
            var curValue = vl % 100;//
            sb.Append((char)('a' + curValue % ('Z' - 'A' + 1)));//select a-z chars 
            vl /= 100;
        }
        if (idx <= RepeatCountElements)//push RepeatCountElements to RepeatWordQueue
        {
            _repeatedWordsQueue.Add(sb.ToString());
        }
    }
}