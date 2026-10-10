List<int> scores = [7, 3, 9, 3];
scores.Add(5);
scores.Remove(3);
scores.RemoveAt(0);
Console.WriteLine(string.Join(", ", scores));
Console.WriteLine(scores.Contains(9));
Console.WriteLine(scores.IndexOf(5));
Console.WriteLine();

string text = "to be or not to be";
Dictionary<string, int> counts = [];

foreach (string word in text.Split(' '))
{
    if (counts.TryGetValue(word, out int count))
    {
        counts[word] = count + 1;
    }
    else
    {
        counts[word] = 1;
    }
}

foreach (KeyValuePair<string, int> pair in counts)
{
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}

// A key that is not in the dictionary throws KeyNotFoundException.
// Console.WriteLine(counts["bird"]);
Console.WriteLine();

HashSet<string> fruits = ["apple", "plum", "kiwi"];
Console.WriteLine(fruits.Add("pear"));
Console.WriteLine(fruits.Add("plum"));
Console.WriteLine(fruits.Contains("kiwi"));
Console.WriteLine(fruits.Count);
