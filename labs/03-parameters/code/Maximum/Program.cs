Console.WriteLine(Max(3, 7, 2));
Console.WriteLine(Max(10));
int[] scores = [4, 9, 1];
Console.WriteLine(Max(scores));

// Max() with no arguments throws the ArgumentException below.

int Max(params int[] values)
{
    if (values.Length == 0)
    {
        throw new ArgumentException("Max needs at least one value.");
    }

    int max = values[0];
    foreach (int v in values)
    {
        if (v > max)
        {
            max = v;
        }
    }
    return max;
}
