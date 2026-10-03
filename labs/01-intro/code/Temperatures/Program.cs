Console.Write("How many days? ");
int n = int.Parse(Console.ReadLine() ?? "");

double[] temperatures = new double[n];
for (int i = 0; i < n; i++)
{
    Console.Write($"Day {i + 1}: ");
    temperatures[i] = double.Parse(Console.ReadLine() ?? "");
}

double min = temperatures[0];
double max = temperatures[0];
double sum = 0;

foreach (double t in temperatures)
{
    if (t < min) min = t;
    if (t > max) max = t;
    sum += t;
}

Console.WriteLine($"Min: {min}, max: {max}, average: {sum / n:F1}");
