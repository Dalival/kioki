using System.Security.Cryptography;
using Encryption;

const string message = "Федоренко Егор и Мадина Ахриева – на страже безпасности!";

var encryptor = new FenceEncryptor(5);
var encryptedMsg = encryptor.Encrypt(message);
Console.WriteLine(encryptedMsg);
var decryptedMsg = encryptor.Decrypt(encryptedMsg);
Console.WriteLine(decryptedMsg);

Console.ReadKey();
