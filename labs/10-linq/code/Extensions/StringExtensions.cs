namespace Extensions;

public static class StringExtensions
{
    public static int WordCount(this string text)
    {
        string[] words = text.Split(' ',
            StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    public static string Shorten(this string text, int length)
    {
        if (text.Length <= length)
        {
            return text;
        }
        return text.Substring(0, length) + "...";
    }

    public static string Joined(this IEnumerable<string> items)
    {
        return string.Join(", ", items);
    }
}
