using Encryption;
using Encryption.Lab1;
using Encryption.Lab2;
using Encryption.Lab3;

const string message = "Egor and Alina are copilots. We are working together to achieve better results.";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("FedorenkoBartsevich"),
    new MatrixEncryptor((0, 0), (1, 3), (3, 1), (2,2)),
    new CaesarEncryptor(12),
    new AsciiCaesarEncryptor(13),
    new MultiCaesarEncryptor(5),
    new RsaEncryptor()
};

foreach (var encryptor in encryptors)
{
    var encrypted = encryptor.Encrypt(message);
    var decrypted = encryptor.Decrypt(encrypted);

    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(encrypted);
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(decrypted);
    if (decrypted == message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("SUCCESS");
    }
    else if (decrypted == message.ToLower())
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("CASE LOSS");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("FAILED");
    }
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine('\n');
}

var sdesEncryptor = new SdesEncryptor(new List<byte> { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0 });
var encrypt = sdesEncryptor.Encrypt(message);
var decrypt = sdesEncryptor.Decrypt(encrypt);

Console.WriteLine(message);
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine(encrypt);
Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine(decrypt);
if (decrypt == message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("SUCCESS");
}
else if (decrypt == message.ToLower())
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("CASE LOSS");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("FAILED");
}
Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine('\n');

Console.ReadKey();
