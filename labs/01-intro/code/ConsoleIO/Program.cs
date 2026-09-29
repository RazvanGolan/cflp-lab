#region output
string name = "Ana";
int year = 2;
double average = 9.456;

Console.WriteLine("Name: " + name);
Console.WriteLine($"{name} is in year {year}");
Console.WriteLine($"Average: {average:F2}");
#endregion

#region parse
Console.Write("How old are you? ");
string line = Console.ReadLine() ?? "";
int age = int.Parse(line);
Console.WriteLine($"Next year you will be {age + 1}");
#endregion

#region tryparse
Console.Write("How many siblings do you have? ");
int siblings;
while (!int.TryParse(Console.ReadLine(), out siblings))
{
    Console.Write("That is not a whole number. Try again: ");
}
Console.WriteLine($"You have {siblings} sibling(s)");
#endregion
