using Encryption;

// const string message = "Федоренко Егор и Мадина Ахриева – на страже безпасности!";
const string message = "ЭТОЛЕКЦИЯПОКРИПТ";

var encryptors = new List<IEncryptor>
{
    new FenceEncryptor(5),
    new KeywordEncryptor("EgorAndMadina"),
    new MatrixEncryptor((0, 0), (1, 3), (3, 1), (2,2))
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
