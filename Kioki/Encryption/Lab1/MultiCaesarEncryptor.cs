using System.Text;

namespace Encryption.Lab1;

public class MultiCaesarEncryptor : IEncryptor
{
    private const string AllowedSymbols = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
                                          + "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                          + "abcdefghijklmnopqrstuvwxyz"
                                          + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                          + "1234567890"
                                          + "-=+_)(*&^%$#@!~`;:\"',.<>/?|\\ \n";

    private readonly Random _random = new();

    private readonly int _key;
    private readonly int _secondKey;

    public MultiCaesarEncryptor(int key)
    {
        if (!IsKeyValid(key))
        {
            throw new ArgumentException("The key does not fit rules. Please try another key.");
        }

        _key = key;
        _secondKey = GenerateSecondKey();
    }

    public string Encrypt(string message) => ShiftCharacters(message, _key);

    public string Decrypt(string encryptedMsg) => ShiftCharacters(encryptedMsg, _secondKey);

    private static string ShiftCharacters(string message, int key)
    {
        var strBuilder = new StringBuilder();
        foreach (var symbol in message)
        {
            var positionInAlphabet = AllowedSymbols.IndexOf(symbol);
            if (positionInAlphabet < 0)
            {
                strBuilder.Append(symbol);
            }
            else
            {
                var newPosition = positionInAlphabet * key % AllowedSymbols.Length;
                strBuilder.Append(AllowedSymbols[newPosition]);
            }
        }

        return strBuilder.ToString();
    }

    private static bool IsKeyValid(int key) => AreCoprime(key, AllowedSymbols.Length);

    private static bool AreCoprime(int a, int b)
    {
        while (true)
        {
            if (a == b)
            {
                return a == 1;
            }

            if (a > b)
            {
                a -= b;
                continue;
            }

            var newA = b - a;
            b = a;
            a = newA;
        }
    }

    private int GenerateSecondKey()
    {
        double secondKey;
        do
        {
            secondKey = (AllowedSymbols.Length * _random.Next(1, 1000) + 1) / (double) _key;
        } while (secondKey % 1 != 0);

        return (int) secondKey;
    }
}
