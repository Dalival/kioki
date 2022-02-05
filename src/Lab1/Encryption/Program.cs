using Encryption;

const string message = "Федоренко Егор и Мадина Ахриева – на страже безпасности!";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("EgorAndMadina")
};

foreach (var encryptor in encryptors)
{
    var encrypted = encryptor.Encrypt(message);
    var original = encryptor.Decrypt(encrypted);

    Console.WriteLine(message);
    Console.WriteLine(encrypted);
    Console.WriteLine(original);
    Console.WriteLine();
}

Console.ReadKey();
