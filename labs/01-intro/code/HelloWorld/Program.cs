// Placeholder demo: proves the slides can import regions from real, compiling code.
// Replace with the actual lab 1 demo.

#region greeting
Console.Write("What is your name? ");
string? name = Console.ReadLine();

Console.WriteLine($"Hello, {name}!");
#endregion

#region sum
int[] numbers = [3, 1, 4, 1, 5];
int sum = 0;
foreach (int n in numbers)
{
    sum += n;
}

Console.WriteLine($"Sum = {sum}");
#endregion
