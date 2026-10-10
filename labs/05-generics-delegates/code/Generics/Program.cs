using Generics;

History<string> searches = new History<string>(3);
searches.Add("list");
searches.Add("dictionary");
searches.Add("lambda");
searches.Add("generics");
searches.Print();
Console.WriteLine($"Last search: {searches.Last()}");
Console.WriteLine();

History<int> pages = new History<int>(2);
pages.Add(10);
pages.Add(42);
Console.WriteLine($"Last page: {pages.Last()}");
Console.WriteLine();

Console.WriteLine(Max(3, 9, 4));
Console.WriteLine(Max("pear", "apple", "plum"));

T Max<T>(params T[] values) where T : IComparable<T>
{
    T max = values[0];
    foreach (T v in values)
    {
        if (v.CompareTo(max) > 0)
        {
            max = v;
        }
    }
    return max;
}
