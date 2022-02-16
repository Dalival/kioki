using Hashing;

var message = File.OpenText("message.txt").ReadToEnd();
var hash = Pjw32Hasher.GetHash(message);
Console.WriteLine("MESSAGE: " + message);
Console.WriteLine("HASH: " + hash);
Console.WriteLine("HEX HASH: 0x" + hash.ToString("X"));
