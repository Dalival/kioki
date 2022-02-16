using System.Numerics;
using Hashing;

namespace Signature;

public class RsaSigner
{
    private readonly Random _random = new();

    private readonly int _e;
    private readonly int _d;
    private readonly int _r;

    public RsaSigner(int p, int q)
    {
        if (p == q | !IsPrimeNumber(p) || !IsPrimeNumber(q))
        {
            throw new ArgumentException("Arguments p and q must be unequal prime numbers.");
        }

        (_e, _d, _r) = GenerateKeys(p, q);
    }

    public BigInteger GetSignature(string message)
    {
        var hash = Pjw32Hasher.GetHash(message) % _r;
        var signature = BigInteger.Pow(hash, _d) % _r;

        return signature;
    }

    public bool CheckSignature(string message, BigInteger signature)
    {
        var messageHash = Pjw32Hasher.GetHash(message) % _r;
        var signatureHash = BigInteger.Pow(signature, _e) % _r;

        return signatureHash == messageHash;
    }

    private (int e, int d, int r) GenerateKeys(int p, int q)
    {
        var r = p * q;
        var fi = (p - 1) * (q - 1);
        var e = _random.Next(1, fi);
        while (!IsPrimeNumber(e) || !AreCoprimeNumbers(e, fi) || e == 2)
        {
            e = _random.Next(1, fi);
        }

        var d = EuclidExtended(fi, e).Y;
        if (d < 0)
        {
            d += fi;
        }

        return (e, d, r);
    }

    private static bool AreCoprimeNumbers(int a, int b)
    {
        return a == b
            ? a == 1
            : a > b
                ? AreCoprimeNumbers(a - b, b)
                : AreCoprimeNumbers(b - a, a);
    }

    private static bool IsPrimeNumber(int n)
    {
        var result = true;

        if (n > 1)
        {
            for (var i = 2u; i < n; i++)
            {
                if (n % i == 0)
                {
                    result = false;
                    break;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static (int X, int Y, int D) EuclidExtended(int a, int b)
    {
        if (!(a > b))
        {
            throw new ArgumentException("Must be: a > b");
        }

        var d0 = a;
        var d1 = b;
        var x0 = 1;
        var x1 = 0;
        var y0 = 0;
        var y1 = 1;
        while (d1 > 1)
        {
            var q = d0 / d1;
            var d2 = d0 % d1;
            var x2 = x0 - q * x1;
            var y2 = y0 - q * y1;
            d0 = d1;
            d1 = d2;
            x0 = x1;
            x1 = x2;
            y0 = y1;
            y1 = y2;
        }

        //для нашего алгоритма нужно y1
        return (x1, y1, d1);
    }
}
