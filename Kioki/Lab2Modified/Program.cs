using Lab2Modified;

const string message = "This is a Egor's message.";
Console.WriteLine(message);
var encryptor = new SdesEncryptor(new List<byte> { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0 });
var cipher = encryptor.Encrypt(message);
var result = encryptor.Decrypt(cipher);

Console.Write("Encrypted: ");
foreach (var s in cipher)
{
    Console.Write(s);
}

Console.WriteLine();

Console.Write("Decrypted: ");
foreach (var s in result)
{
    Console.Write(s);
}

Console.ReadKey();
