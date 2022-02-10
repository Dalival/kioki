﻿using Encryption;

const string message = "Егорка и Алинка из 972301/2 на страже безопасности!";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("EgorAndMadina"),
    new MatrixEncryptor((0, 0), (1, 3), (3, 1), (2,2)),
    new CaesarEncryptor(12),
    new AsciiCaesarEncryptor(13),
    new MultiCaesarEncryptor(5)
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
