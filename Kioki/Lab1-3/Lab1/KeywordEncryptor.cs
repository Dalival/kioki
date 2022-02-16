using System.Text;

namespace Lab1;

public class KeywordEncryptor : IEncryptor
{
    private const char HookSymbol = '`';

    private readonly List<int> _indexes;

    public KeywordEncryptor(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            throw new ArgumentException("Key must be a non-empty string.");
        }

        if (keyword.Length < 2)
        {
            throw new ArgumentException("Key must contain at least 2 letters.");
        }

        if (keyword.ToUpper().Any(c => c is < 'A' or > 'Z'))
        {
            throw new ArgumentException("Key must contain latin letters only.");
        }

        _indexes = GetIndexes(keyword);
    }

    public string Encrypt(string message)
    {
        var normalizedMsg = Normalize(message);
        var lines = SplitOnLines(normalizedMsg);

        var builder = new StringBuilder();
        foreach (var line in lines)
        {
            for (var i = 0; i < _indexes.Count; i++)
            {
                builder.Append(line[_indexes.IndexOf(i)]);
            }
        }

        return builder.ToString();
    }

    public string Decrypt(string encryptedMsg)
    {
        var lines = SplitOnLines(encryptedMsg);

        var builder = new StringBuilder();
        foreach (var line in lines)
        {
            foreach (var t in _indexes)
            {
                builder.Append(line[t]);
            }
        }

        builder.Replace(HookSymbol.ToString(), "");

        return builder.ToString();
    }

    private string Normalize(string message)
    {
        while (message.Length % _indexes.Count != 0)
        {
            message += HookSymbol;
        }

        return message;
    }

    private List<string> SplitOnLines(string message)
    {
        var lines = new List<string>();
        for (var i = 0; i < message.Length; i++)
        {
            var line = string.Concat(message.Take(_indexes.Count));
            lines.Add(line);
            message = message.Remove(0, _indexes.Count);
        }

        return lines;
    }

    private static List<int> GetIndexes(string keyword)
    {
        keyword = keyword.ToLower();
        var orderedKeyword = keyword.OrderBy(c => c).ToList();
        var indexes = new List<int>(Enumerable.Range(0, keyword.Length));

        foreach (var letter in keyword.Distinct())
        {
            var indexInWord = 0;
            var order = 0;
            for (var i = 0; i < keyword.Count(c => c == letter); i++)
            {
                indexInWord = keyword.IndexOf(letter, indexInWord);
                order = orderedKeyword.IndexOf(letter, order);
                indexes[indexInWord] = order;

                indexInWord++;
                order++;
            }
        }

        return indexes;
    }
}
