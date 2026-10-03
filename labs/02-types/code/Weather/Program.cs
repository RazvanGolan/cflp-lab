Weather today = Weather.Rainy;
Console.WriteLine(today);
Console.WriteLine((int)today);
Console.WriteLine();

foreach (Weather w in Enum.GetValues<Weather>())
{
    Console.WriteLine($"{(int)w}. {w}");
}

Console.Write("What is the weather like? ");
string text = Console.ReadLine() ?? "";

if (Enum.TryParse(text, ignoreCase: true, out Weather weather)
    && Enum.IsDefined(weather))
{
    Console.WriteLine(Advice(weather));
}
else
{
    Console.WriteLine("Unknown weather");
}

string Advice(Weather weather)
{
    return weather switch
    {
        Weather.Sunny => "Take sunglasses",
        Weather.Rainy => "Take an umbrella",
        Weather.Snowy => "Wear boots",
        _ => "Nothing special today"
    };
}
