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

        var row = 0;
        var column = 0;
        var downwardDirection = false;
        for (var i = 0; i < message.Length; i++)
        {
            map[row][column] = symbols[i];
            column++;
            if (row == _key - 1 || row == 0)
            {
                downwardDirection = !downwardDirection;
            }

            row = downwardDirection ? row + 1 : row - 1;
        }

        PrintMap(map);

        var encryptedMessage = MapToString(map);

        return encryptedMessage;
    }

    public string Decrypt(string encryptedMsg)
    {
        return "Not implemented";
    }

    private List<List<char>> CreateMap(int messageLength)
    {
        var symbolsMap = new List<List<char>>();
        for (var i = 0; i < _key; i++)
        {
            var row = new List<char>();
            row.AddRange(Enumerable.Repeat(HookSymbol, messageLength));
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
}
