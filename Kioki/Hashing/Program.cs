using Hashing;

var message = File.OpenText("message.txt").ReadToEnd();
var hash = Pjw32Hasher.GetHash(message);
Console.WriteLine("MESSAGE: " + message);
Console.WriteLine("HASH: " + hash);
Console.WriteLine("HEX HASH: 0x" + hash.ToString("X"));

Console.WriteLine("\nLet's hash another instance of string but with the same content:");
var messageCopy = new string(message);
var hashOfCopy = Pjw32Hasher.GetHash(messageCopy);
Console.WriteLine("HASH: " + hashOfCopy);

Console.Write("\nAre hashes the same: ");
Console.ForegroundColor = hash == hashOfCopy ? ConsoleColor.Green : ConsoleColor.Red;
Console.WriteLine((hash == hashOfCopy).ToString().ToUpper());

Console.ReadKey();
