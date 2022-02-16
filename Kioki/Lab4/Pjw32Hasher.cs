namespace Lab4;

public static class Pjw32Hasher
{
    public static uint GetHash(string message)
    {
        uint hash = 0;
        foreach (var symbol in message)
        {
            hash = (hash << 4) + (byte)symbol;
            var h1 = hash & 4026531840;
            if (h1 != 0)
            {
                hash = (hash ^ (h1 >> 24)) & 268435455;
            }
        }

        return hash;
    }
}
