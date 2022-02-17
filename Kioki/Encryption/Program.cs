using Encryption;
using Encryption.Lab1;
using Encryption.Lab2;
using Encryption.Lab3;

const string message = "Egor and Alina are copilots. We are working together to achieve better results.";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("FedorenkoBartsevich"),
    new MatrixEncryptor((0, 0), (1, 3), (3, 1), (2, 2)),
    new CaesarEncryptor(12),
    new AsciiCaesarEncryptor(13),
    new MultiCaesarEncryptor(5),
    new SdesEncryptor(new List<byte> { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0 }),
    new RsaEncryptor()
};

foreach (var encryptor in encryptors)
{
    var encrypted = encryptor.Encrypt(message);
    var decrypted = encryptor.Decrypt(encrypted);

    PrintLine(encryptor.GetType().Name, ConsoleColor.Blue);
    Console.WriteLine(message);
    PrintLine(encrypted, ConsoleColor.DarkGray);
    Console.WriteLine(decrypted);
    if (decrypted == message)
    {
        PrintLine("SUCCESS", ConsoleColor.Green);
    }
    else if (decrypted == message.ToLower())
    {
        PrintLine("CASE LOSS", ConsoleColor.Yellow);
    }
    else
    {
        PrintLine("FAILED", ConsoleColor.Red);
    }

    Console.WriteLine('\n');
}

Console.ReadKey();

void PrintLine(object value, ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.WriteLine(value);
    Console.ForegroundColor = ConsoleColor.Gray;
}
