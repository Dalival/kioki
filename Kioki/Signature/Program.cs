using Signature;

var message = File.OpenText("message.txt").ReadToEnd();

var signer = new RsaSigner(89, 97);
var signature = signer.GetSignature(message);
var isSignedCorrect = signer.CheckSignature(message, signature);

Console.WriteLine($"Message: {message}");
Console.WriteLine("Signature: " + signature);
Console.Write("Is signature correct: ");
Console.ForegroundColor = isSignedCorrect ? ConsoleColor.Green : ConsoleColor.Red;
Console.WriteLine(isSignedCorrect.ToString().ToUpper());

Console.ReadKey();
