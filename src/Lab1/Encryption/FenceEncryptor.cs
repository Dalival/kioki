using System.Text;

namespace Encryption;

public class FenceEncryptor
{
    private const char HookSymbol = '~';

    private readonly int _key;

    public FenceEncryptor(int fenceHeight)
    {
        if (fenceHeight <= 0)
        {
            throw new ArgumentException("Hedge height must be a positive value.");
        }

        _key = fenceHeight;
    }

    public string Encrypt(string message)
    {
        var symbols = message.ToCharArray().ToList();
        var map = CreateMap(message.Length);

        var column = 0;
        var downwardDirection = false;
        for (var i = 0; i < symbols.Count; i++)
        {
            map[column][i] = symbols[i];
            if (column == _key - 1 || column == 0)
            {
                downwardDirection = !downwardDirection;
            }

            column = downwardDirection ? column + 1 : column - 1;
        }

        PrintMap(map);

        var encryptedMessage = MapToString(map);

        return encryptedMessage;
    }

    public List<int> EncryptInt(List<int> numbers)
    {
        var map = CreateMapInt(numbers.Count);

        var column = 0;
        var downwardDirection = false;
        for (var i = 0; i < numbers.Count; i++)
        {
            map[column][i] = numbers[i];
            if (column == _key - 1 || column == 0)
            {
                downwardDirection = !downwardDirection;
            }

            column = downwardDirection ? column + 1 : column - 1;
        }

        PrintMapInt(map);

        var encryptedMessage = MapToListInt(map);

        return encryptedMessage;
    }

    public string Decrypt(string encryptedMsg)
    {
        var range = Enumerable.Range(0, encryptedMsg.Length).ToList();
        var pos = EncryptInt(range);
        var str = "";
        foreach (var n in range)
        {
            var index = pos.IndexOf(n);
            var part = encryptedMsg[index];
            str += part;
        }

        return str;
    }

    private List<List<char>> CreateMap(int msgLength)
    {
        var symbolsMap = new List<List<char>>();
        for (var i = 0; i < _key; i++)
        {
            var row = new List<char>();
            row.AddRange(Enumerable.Repeat(HookSymbol, msgLength));
            symbolsMap.Add(row);
        }

        return symbolsMap;
    }

    private List<List<int>> CreateMapInt(int msgLength)
    {
        var symbolsMap = new List<List<int>>();
        for (var i = 0; i < _key; i++)
        {
            var row = new List<int>();
            row.AddRange(Enumerable.Repeat(-1, msgLength));
            symbolsMap.Add(row);
        }

        return symbolsMap;
    }

    private string MapToString(List<List<char>> map)
    {
        var symbols = map.SelectMany(x => x);
        var message = new string(symbols.ToArray());
        var normalizedMessage = message.Replace(HookSymbol.ToString(), "");

        return normalizedMessage;
    }

    private List<int> MapToListInt(List<List<int>> map)
    {
        var symbols = map.SelectMany(x => x).ToList();

        while (true)
        {
            var index = symbols.FindIndex(s => s == -1);
            if (index != -1)
            {
                symbols.RemoveAt(index);
                continue;
            }

            break;
        }

        return symbols;
    }

    private void PrintMap(List<List<char>> map)
    {
        var builder = new StringBuilder();
        foreach (var row in map)
        {
            foreach (var symbol in row)
            {
                builder.Append(symbol);
            }

            builder.Append('\n');
        }

        builder.Replace(HookSymbol, ' ');
        Console.WriteLine($"{builder}\n");
    }

    private void PrintMapInt(List<List<int>> map)
    {
        var builder = new StringBuilder();
        foreach (var row in map)
        {
            foreach (var symbol in row)
            {
                builder.Append(symbol);
            }

            builder.Append('\n');
        }

        builder.Replace("-1", " ");
        Console.WriteLine($"{builder}\n");
    }
}
