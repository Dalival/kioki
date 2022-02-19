using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

// the project can be running on windows only
#pragma warning disable CA1416

namespace Stenography;

public static class LsbStenographer
{
    private enum State
    {
        Hiding,
        FillingWithZeros
    };

    public static void HideMessage(string message, string sourceFileName, string resultFileName)
    {
        var bitmap = (Bitmap) Image.FromFile(sourceFileName);

        var state = State.Hiding;
        var charIndex = 0;
        var charValue = 0;
        long pixelElementIndex = 0;
        var zeros = 0;

        for (var i = 0; i < bitmap.Height; i++)
        {
            for (var j = 0; j < bitmap.Width; j++)
            {
                var pixel = bitmap.GetPixel(j, i);

                var red = pixel.R - pixel.R % 2;
                var green = pixel.G - pixel.G % 2;
                var blue = pixel.B - pixel.B % 2;

                for (var n = 0; n < 3; n++)
                {
                    if (pixelElementIndex % 8 == 0)
                    {
                        if (state == State.FillingWithZeros && zeros == 8)
                        {
                            if ((pixelElementIndex - 1) % 3 < 2)
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                            }

                            bitmap.Save(resultFileName, ImageFormat.Png);
                            return;
                        }

                        if (charIndex >= message.Length)
                        {
                            state = State.FillingWithZeros;
                        }
                        else
                        {
                            charValue = message[charIndex++];
                        }
                    }

                    switch (pixelElementIndex % 3)
                    {
                        case 0:
                            if (state == State.Hiding)
                            {
                                red += charValue % 2;
                                charValue /= 2;
                            }

                            break;
                        case 1:
                            if (state == State.Hiding)
                            {
                                green += charValue % 2;
                                charValue /= 2;
                            }

                            break;
                        case 2:
                            if (state == State.Hiding)
                            {
                                blue += charValue % 2;
                                charValue /= 2;
                            }

                            bitmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                            break;
                    }

                    pixelElementIndex++;

                    if (state == State.FillingWithZeros)
                    {
                        zeros++;
                    }
                }
            }
        }

        bitmap.Save(resultFileName, ImageFormat.Png);
    }

    public static string ExtractMessage(string fileName)
    {
        var bitmap = (Bitmap) Image.FromFile(fileName);

        var colorUnitIndex = 0;
        var charValue = 0;

        var extractedMsg = new StringBuilder();

        for (var i = 0; i < bitmap.Height; i++)
        {
            for (var j = 0; j < bitmap.Width; j++)
            {
                var pixel = bitmap.GetPixel(j, i);

                for (var n = 0; n < 3; n++)
                {
                    charValue = (colorUnitIndex % 3) switch
                    {
                        0 => charValue * 2 + pixel.R % 2,
                        1 => charValue * 2 + pixel.G % 2,
                        2 => charValue * 2 + pixel.B % 2,
                        _ => charValue
                    };

                    colorUnitIndex++;

                    if (colorUnitIndex % 8 == 0)
                    {
                        // we need reverse since each time the process occurs on the right (for simplicity)
                        charValue = ReverseBits(charValue);

                        if (charValue == 0)
                        {
                            return extractedMsg.ToString();
                        }

                        var symbol = (char) charValue;
                        extractedMsg.Append(symbol);
                    }
                }
            }
        }

        return extractedMsg.ToString();
    }

    private static int ReverseBits(int n)
    {
        var result = 0;

        for (var i = 0; i < 8; i++)
        {
            result = result * 2 + n % 2;
            n /= 2;
        }

        return result;
    }
}
