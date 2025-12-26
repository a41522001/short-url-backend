using System.Text;

namespace ShortUrlPJ.Utils;

public static class Base62
{
    public static string Encode(int value)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var result = "";
        while (value > 0)
        {
            result = chars[(int)(value % 62)] + result;
            value /= 62;
        }
        return result;
    }
    public static int Decode(string value)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        if (string.IsNullOrEmpty(value)) return 0;

        int result = 0;
        foreach (char c in value)
        {
            int index = chars.IndexOf(c);
            if (index == -1)
            {
                throw new ArgumentException($"Invalid character found: {c}");
            }
            result = result * 62 + index;
        }
        return result;
    }
}
