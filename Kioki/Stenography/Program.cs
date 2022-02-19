using Stenography;

const string message = "Egor and Alina are copilots. We are working together to achieve better results.";

LsbStenographer.HideMessage(message, "image.jpg", "image_hided.png");
var extractedMsg = LsbStenographer.ExtractMessage("image_hided.png");

Console.WriteLine("ORIGINAL:  " + message);
Console.WriteLine("EXTRACTED: " + extractedMsg);
Console.WriteLine("\nARE SAME:  " + (message == extractedMsg));

Console.ReadKey();
