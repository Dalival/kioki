using System.Text;

namespace Encryption.Lab1;

public class CaesarEncryptor : IEncryptor
{
    private const string AllowedSymbols = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
                                          + "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                          + "abcdefghijklmnopqrstuvwxyz"
                                          + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                          + "1234567890"
                                          + "-=+_)(*&^%$#@!~`;:\"',.<>/?|\\ \n";

    private readonly int _key;

    public CaesarEncryptor(int key)
    {
        if (key == 0)
        {
            throw new ArgumentException("The key cannot be 0.");
        }

        _key = key;
    }

    public string Encrypt(string message) => ShiftCharacters(message, _key);

    public string Decrypt(string encryptedMsg) => ShiftCharacters(encryptedMsg, -_key);

    private static string ShiftCharacters(string message, int key)
    {
        var strBuilder = new StringBuilder();
        foreach (var symbol in message)
        {
            var foundSymbol = symbol;
            var positionInAlphabet = AllowedSymbols.IndexOf(foundSymbol);
            if (positionInAlphabet < 0)
            {
                strBuilder.Append(foundSymbol);
            }
            else
            {
                var newPosition = (positionInAlphabet + key) % AllowedSymbols.Length;
                if (newPosition < 0)
                {
                    newPosition = AllowedSymbols.Length + newPosition;
                }

                foundSymbol = AllowedSymbols[newPosition];
                strBuilder.Append(foundSymbol);
            }
        }

        return strBuilder.ToString();
    }
}
