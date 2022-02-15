namespace Encryption;

public interface IEncryptor
{
    string Encrypt(string message);
    string Decrypt(string encryptedMsg);
}
