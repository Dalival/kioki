using Encryption;

const string message = "Федоренко Егор и Мадина Ахриева из 972301 – на страже безпасности!";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("EgorAndMadina"),
    new MatrixEncryptor((0, 0), (1, 3), (3, 1), (2,2))
};

foreach (var encryptor in encryptors)
{
    var encrypted = encryptor.Encrypt(message);
    var decrypted = encryptor.Decrypt(encrypted);

    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(encrypted);
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(decrypted);
    Console.WriteLine('\n');
}

Console.ReadKey();
