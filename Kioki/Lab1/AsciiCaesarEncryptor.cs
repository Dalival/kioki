namespace Lab1;

public class AsciiCaesarEncryptor : IEncryptor
{
    private readonly int _key;

    public AsciiCaesarEncryptor(int key)
    {
        if (key == 0)
        {
            throw new ArgumentException("The key cannot be 0.");
        }

        _key = key;
    }

    public string Encrypt(string message) => string.Concat(message.Select(c => (char)(c + _key)));

    public string Decrypt(string encryptedMsg) => string.Concat(encryptedMsg.Select(c => (char)(c - _key)));
}
