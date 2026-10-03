Divide(17, 5, out int quotient, out int remainder);
Console.WriteLine($"17 = 5 * {quotient} + {remainder}");

Console.Write("Start time (hh:mm): ");
string text = Console.ReadLine() ?? "";

if (TryParseTime(text, out int h, out int m))
{
    int total = h * 60 + m;
    Console.WriteLine($"{total} minutes");
}
else
{
    Console.WriteLine("Use hh:mm, like 14:30");
}

void Divide(int a, int b, out int quotient, out int remainder)
{
    quotient = a / b;
    remainder = a % b;
}

bool TryParseTime(string text,
                  out int hour, out int minute)
{
    hour = 0;
    minute = 0;
    string[] parts = text.Split(':');
    if (parts.Length != 2
        || !int.TryParse(parts[0], out int h)
        || !int.TryParse(parts[1], out int m)
        || h < 0 || h > 23 || m < 0 || m > 59)
    {
        return false;
    }
    hour = h;
    minute = m;
    return true;
}
