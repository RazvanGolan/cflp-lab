using Charts;

string path = Path.Combine(AppContext.BaseDirectory, "songs.csv");
List<Song> songs = File.ReadAllLines(path)
    .Skip(1)
    .Select(Song.Parse)
    .ToList();
Console.WriteLine($"{songs.Count} songs");
Console.WriteLine();

IEnumerable<Song> beatles = songs.Where(s => s.Artist == "The Beatles");
foreach (Song song in beatles)
{
    Console.WriteLine(song);
}
Console.WriteLine();

IEnumerable<string> titles = songs.Select(s => s.Title);
Console.WriteLine(string.Join(", ", titles.Take(4)));
Console.WriteLine();

List<string> longest = songs
    .OrderByDescending(s => s.Seconds)
    .Take(3)
    .Select(s => $"{s.Title} ({s.Seconds} s)")
    .ToList();
foreach (string line in longest)
{
    Console.WriteLine(line);
}
Console.WriteLine();

Console.WriteLine(songs.Count(s => s.Genre == "rock"));
Console.WriteLine(songs.Sum(s => s.Seconds));
Console.WriteLine(songs.Average(s => s.Seconds));
Console.WriteLine(songs.Min(s => s.Year));
Console.WriteLine(songs.MaxBy(s => s.Seconds));
Console.WriteLine(songs.First(s => s.Year > 2000));
Console.WriteLine(songs.FirstOrDefault(s => s.Year > 2020)?.Title ?? "none");
Console.WriteLine(songs.Any(s => s.Genre == "jazz"));
Console.WriteLine();

foreach (IGrouping<string, Song> genre in songs.GroupBy(s => s.Genre))
{
    double average = genre.Average(s => s.Seconds);
    Console.WriteLine($"{genre.Key}: {genre.Count()}, average {average:F0} s");
}
Console.WriteLine();

IEnumerable<string> oldestFirst = songs
    .Where(s => s.Genre == "rock")
    .OrderBy(s => s.Year)
    .ThenBy(s => s.Title)
    .Select(s => $"{s.Year} {s.Title}");
foreach (string line in oldestFirst)
{
    Console.WriteLine(line);
}
Console.WriteLine();

IEnumerable<string> sameQuery =
    from s in songs
    where s.Genre == "rock"
    orderby s.Year, s.Title
    select $"{s.Year} {s.Title}";
Console.WriteLine(sameQuery.SequenceEqual(oldestFirst));
