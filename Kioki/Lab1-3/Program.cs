using Lab1;

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

Console.ReadKey();
