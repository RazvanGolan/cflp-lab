foreach (int number in Countdown(3))
{
    Console.WriteLine(number);
}
Console.WriteLine();

foreach (int square in Squares().Take(5))
{
    Console.Write($"{square} ");
}
Console.WriteLine();
Console.WriteLine();

List<int> scores = [4, 9, 6];
IEnumerable<int> passed = scores.Where(n =>
{
    Console.WriteLine($"  checking {n}");
    return n >= 5;
});
Console.WriteLine("Query written");
scores.Add(8);

foreach (int score in passed)
{
    Console.WriteLine(score);
}
Console.WriteLine();

List<int> marks = [4, 9, 6];
IEnumerable<int> query = marks.Where(n => n >= 5);
List<int> saved = query.ToList();
marks.Add(8);
Console.WriteLine($"query: {query.Count()}, saved: {saved.Count}");

IEnumerable<int> Countdown(int from)
{
    for (int i = from; i > 0; i--)
    {
        Console.WriteLine($"  making {i}");
        yield return i;
    }
    Console.WriteLine("  done");
}

IEnumerable<int> Squares()
{
    int n = 1;
    while (true)
    {
        yield return n * n;
        n++;
    }
}
