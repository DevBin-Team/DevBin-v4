namespace Common.Utils;

public static class StringExtensions
{
    public static string Truncate(this string text, int maxLength)
    {
        return text[0..Math.Min(maxLength, text.Length)];
    }
}
