using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Encryption;

public class MatrixEncryptor : IEncryptor
{
    private const char HookSymbol = '`';

    private readonly int[][] _matrix =
    {
        new[] { 1, 2, 3, 1 },
        new[] { 3, 4, 4, 2 },
        new[] { 2, 4, 4, 3 },
        new[] { 1, 3, 2, 1 }
    };

    private readonly (int X, int Y)[] _keys;

    public MatrixEncryptor((int, int) first, (int, int) second, (int, int) third, (int, int) fourth)
    {
        ThrowOnInvalidKeys(first, second, third, fourth);
        _keys = new[] { first, second, third, fourth };
    }

    public string Encrypt(string message)
    {
        var normalizedMsg = Normalize(message);
        var msgParts = SplitMessage(normalizedMsg, _matrix.Length * _matrix.Length);
        var matrixes = new List<char[][]>();

        foreach (var msgPart in msgParts)
        {
            var matrix = GenerateCharMatrix();
            var lines = SplitMessage(msgPart, _matrix.Length);
            foreach (var line in lines)
            {
                for (var i = 0; i < _matrix.Length; i++)
                {
                    var X = _keys[i].X;
                    var Y = _keys[i].Y;
                    matrix[X][Y] = line[i];
                }

                matrix = RotateMatrix(matrix);
            }
            matrixes.Add(matrix);
        }

        var builder = new StringBuilder();
        foreach (var matrix in matrixes)
        {
            builder.Append(MatrixToString(matrix));
        }

        return builder.ToString();
    }

    public string Decrypt(string encryptedMsg)
    {
        return "Not implemented";
    }

    private void ThrowOnInvalidKeys(params (int X, int Y)[] indexes)
    {
        foreach (var (x, y) in indexes)
        {
            if (x is < 0 or > 3 || y is < 0 or > 3)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        var numbers = indexes
            .Select(i => _matrix[i.X][i.Y])
            .OrderBy(i => i)
            .ToList();

        if (!numbers.SequenceEqual(new List<int> { 1, 2, 3, 4 }))
        {
            throw new ArgumentException("Wrong keys! Indexes must point to each of the numbers: 1, 2, 3, 4.");
        }
    }

    private List<string> SplitMessage(string message, int substringLength)
    {
        var lines = new List<string>();
        for (var i = 0; i < message.Length; i++)
        {
            var line = string.Concat(message.Take(substringLength));
            lines.Add(line);
            message = message.Remove(0, substringLength);
        }

        return lines;
    }

    private string Normalize(string message)
    {
        while (message.Length % (_matrix.Length * _matrix.Length) != 0)
        {
            message += HookSymbol;
        }

        return message;
    }

    private char[][] GenerateCharMatrix()
    {
        var matrix = new List<List<char>>();
        for (var i = 0; i < _matrix.Length; i++)
        {
            var line = Enumerable.Repeat(HookSymbol, _matrix.Length).ToList();
            matrix.Add(line);
        }

        return matrix.Select(x => x.ToArray()).ToArray();
    }

    private static char[][] RotateMatrix(char[][] matrix)
    {
        var size = matrix.Length;
        //var layerCount = size / 2;

        //for (var i = 0; i < layerCount; i++)
        for (var i = 0; i < size; i++)
        {
            var first = i;
            var last = size - first - 1;
            for (var k = i; k < last; k++)
            {
                var offset = k - first;

                var top = matrix[first][k];
                var right = matrix[k][last];
                var bottom = matrix[last][last - offset];
                var left = matrix[last - offset][first];

                matrix[first][k] = left;
                matrix[k][last] = top;
                matrix[last][last - offset] = right;
                matrix[last - offset][first] = bottom;
            }
        }

        return matrix;
    }

    private static string MatrixToString(char[][] matrix)
    {
        var builder = new StringBuilder();
        foreach (var line in matrix)
        {
            builder.Append(string.Concat(line));
        }

        return builder.ToString();
    }
}

// [1,1] [1,2] [1,3] [1,4]
// [2,1] [2,2] [2,3] [2,4]
// [3,1] [3,2] [3,3] [3,4]
// [4,1] [4,2] [4,3] [4,4]
//
// [4,1] [3,1] [2,1] [1,1]
// [4,2] [3,2] [2,2] [1,2]
// [4,3] [3,3] [2,3] [1,3]
// [4,4] [3,4] [2,4] [1,4]
