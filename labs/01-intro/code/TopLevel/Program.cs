Console.WriteLine("Statements run from top to bottom.");

int total = Add(2, 3);
Console.WriteLine($"2 + 3 = {total}");

Console.WriteLine($"You passed {args.Length} argument(s)");

int Add(int a, int b)
{
    return a + b;
}
