using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab2Modified;

public class SdesEncryptor
{
    private readonly List<byte> _firstKey;
    private readonly List<byte> _secondKey;

    public SdesEncryptor(List<byte> key)
    {
        if (key?.Count != 10 || key.Any(x => x != 1 && x != 0))
        {
            throw new ArgumentException("Key must be 10 values. Each is either 1 or 0.");
        }

        (_firstKey, _secondKey) = GenerateKeys(key);
    }

    public string Encrypt(string message)
    {
        var allBits = StringToBits(message);
        var byteChunks = allBits.Chunk(8).ToList();
        var encryptedBits = new List<byte>();
        foreach (var bits in byteChunks)
        {
            var iterationOne = EncryptingIteration(bits.ToList(), _firstKey, false);
            var iterationTwo = EncryptingIteration(iterationOne, _secondKey, true);
            encryptedBits.AddRange(iterationTwo);
        }

        var encryptedMsg = BitsToString(encryptedBits);

        return encryptedMsg;
    }

    public string Decrypt(string encryptedMessage)
    {
        var allBits = StringToBits(encryptedMessage);
        var byteChunks = allBits.Chunk(8).ToList();
        var decryptedBits = new List<byte>();
        foreach (var bits in byteChunks)
        {
            var iterationOne = EncryptingIteration(bits.ToList(), _secondKey, false);
            var iterationTwo = EncryptingIteration(iterationOne, _firstKey, true);
            //var result = ApplyIP8(iterationTwo);
            //decryptedBits.AddRange(result);
            decryptedBits.AddRange(iterationTwo);
        }

        var encryptedMsg = BitsToString(decryptedBits);

        return encryptedMsg;
    }

    //todo remove key parameter
    private List<byte> EncryptingIteration(List<byte> bits, List<byte> key, bool secondTime)
    {
        var bitsIp8 = secondTime ? bits : ApplyIP8(bits);

        var leftHalf = bitsIp8.Take(4).ToList();
        var rightHalf = bitsIp8.TakeLast(4).ToList();

        var rightEp = ApplyEP(rightHalf);
        var xorResult = Xor(rightEp, key);

        var xorLeftHalf = xorResult.Take(4).ToList();
        var xorRightHalf = xorResult.TakeLast(4).ToList();

        var xorLeftS0 = ApplyS0(xorLeftHalf); // 1 0
        var xorRightS1 = ApplyS1(xorRightHalf); // 0 0

        var p4 = ApplyP4(xorLeftS0.Concat(xorRightS1).ToList());
        var xorWithInitialLeft = Xor(p4, leftHalf);

        // second: 0 0 1 0 0 1 1 1
        var combined = xorWithInitialLeft.Concat(rightHalf).ToList();

        if (secondTime)
        {
            var p1 = ApplyP1(combined);

            return p1;
        }

        var combinedLeftHalf = combined.Take(4).ToList();
        var combinedRightHalf = combined.TakeLast(4).ToList();

        var swapped = combinedRightHalf.Concat(combinedLeftHalf).ToList();

        return swapped;
    }

    private static (List<byte>, List<byte>) GenerateKeys(IReadOnlyList<byte> initialKey)
    {
        var keyP10 = ApplyP10(initialKey);

        var leftHalf = keyP10.Take(5).ToList();
        var rightHalf = keyP10.TakeLast(5).ToList();

        var leftShifted = ShiftLeft(leftHalf, 1);
        var rightShifted = ShiftLeft(rightHalf, 1);

        var firstKey = ApplyP8(leftShifted.Concat(rightShifted).ToList());

        var leftTripleShifted = ShiftLeft(leftShifted, 2);
        var rightTripleShifted = ShiftLeft(rightShifted, 2);

        var secondKey = ApplyP8(leftTripleShifted.Concat(rightTripleShifted).ToList());

        return (firstKey, secondKey);
    }

    private static List<byte> ApplyP10(IReadOnlyList<byte> values)
    {
        const int size = 10;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ApplyP8(IReadOnlyList<byte> values)
    {
        const int size = 10;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 6, 3, 7, 4, 8, 5, 10, 9 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ApplyIP8(IReadOnlyList<byte> values)
    {
        const int size = 8;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 2, 6, 3, 1, 4, 8, 5, 7 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ApplyEP(IReadOnlyList<byte> values)
    {
        const int size = 4;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 4, 1, 2, 3, 2, 3, 4, 1 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ApplyS0(IReadOnlyList<byte> values)
    {
        const int size = 4;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var s0 = new List<List<byte>>
        {
            new() { 1, 0, 3, 2 },
            new() { 3, 2, 1, 0 },
            new() { 0, 2, 1, 3 },
            new() { 3, 1, 3, 2 }
        };

        var row = (values[0], values[3]) switch
        {
            (0, 0) => 0,
            (0, 1) => 1,
            (1, 0) => 2,
            (1, 1) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        var column = (values[1], values[2]) switch
        {
            (0, 0) => 0,
            (0, 1) => 1,
            (1, 0) => 2,
            (1, 1) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        var cellValue = s0[row][column];
        var result = cellValue switch
        {
            0 => new List<byte> { 0, 0 },
            1 => new List<byte> { 0, 1 },
            2 => new List<byte> { 1, 0 },
            3 => new List<byte> { 1, 1 },
            _ => throw new ArgumentOutOfRangeException()
        };

        return result;
    }

    private static List<byte> ApplyS1(IReadOnlyList<byte> values)
    {
        const int size = 4;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var s1 = new List<List<byte>>
        {
            new() { 0, 1, 2, 3 },
            new() { 2, 0, 1, 3 },
            new() { 3, 0, 1, 0 },
            new() { 2, 1, 0, 3 }
        };

        var row = (values[0], values[3]) switch
        {
            (0, 0) => 0,
            (0, 1) => 1,
            (1, 0) => 2,
            (1, 1) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        var column = (values[1], values[2]) switch
        {
            (0, 0) => 0,
            (0, 1) => 1,
            (1, 0) => 2,
            (1, 1) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        var cellValue = s1[row][column];
        var result = cellValue switch
        {
            0 => new List<byte> { 0, 0 },
            1 => new List<byte> { 0, 1 },
            2 => new List<byte> { 1, 0 },
            3 => new List<byte> { 1, 1 },
            _ => throw new ArgumentOutOfRangeException()
        };

        return result;
    }

    private static List<byte> ApplyP4(IReadOnlyList<byte> values)
    {
        const int size = 4;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 2, 4, 3, 1 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ApplyP1(IReadOnlyList<byte> values)
    {
        const int size = 8;

        if (values.Count != size)
        {
            throw new ArgumentException($"Argument must contain {size} values.");
        }

        var indexes = new List<byte> { 4, 1, 3, 5, 7, 2, 8, 6 };
        var newList = indexes.Select(i => values[i - 1]).ToList();

        return newList;
    }

    private static List<byte> ShiftLeft(IReadOnlyList<byte> values, int shiftValue)
    {
        var newValues = new List<byte>(values);

        for (var i = 0; i < shiftValue; i++)
        {
            var firstValue = newValues[0];
            newValues = newValues.TakeLast(values.Count - 1).ToList();
            newValues.Add(firstValue);
        }

        return newValues;
    }

    private static List<byte> StringToBits(string message)
    {
        var bytes = Encoding.Default.GetBytes(message);
        var bitArray = new BitArray(bytes);
        var bitList = bitArray
            .Cast<bool>()
            .Select(bit => bit ? (byte) 1 : (byte) 0)
            .ToList();

        return bitList;
    }

    private static string BitsToString(List<byte> bits)
    {
        var builder = new StringBuilder();
        foreach (var chunk in bits.Chunk(8))
        {
            var bools = chunk.Select(x => x != 0).ToArray();
            var bitArray = new BitArray(bools);
            var bytes = new byte[1];
            bitArray.CopyTo(bytes, 0);
            var symbol = (char) bytes[0];
            builder.Append(symbol);
        }

        return builder.ToString();
    }

    private static List<byte> Xor(IReadOnlyList<byte> first, IReadOnlyList<byte> second)
    {
        var result = first
            .Select((t, i) => t == second[i] ? (byte) 0 : (byte) 1)
            .ToList();

        return result;
    }
}
