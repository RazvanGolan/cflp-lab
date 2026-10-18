using Extensions;

string line = "to be or not to be";
Console.WriteLine(line.WordCount());
Console.WriteLine(StringExtensions.WordCount(line));

string title = "Smells Like Teen Spirit";
Console.WriteLine(title.Shorten(10));
Console.WriteLine(title.Shorten(50));

List<string> bands = ["Queen", "Eagles", "Abba"];
string[] genres = ["rock", "pop", "jazz"];
Console.WriteLine(bands.Joined());
Console.WriteLine(genres.Joined());

// Where comes from System.Linq and works on any IEnumerable<T> in the same way.
Console.WriteLine(bands.Where(b => b.Length > 5).Joined());
