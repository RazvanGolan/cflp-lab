using Streaming;

List<Media> catalog =
[
    new Movie("Inception", 2010, 148),
    new Series("Chernobyl", 2019, 5, 65),
    new Movie("Spirited Away", 2001, 125),
];

int total = 0;
foreach (Media item in catalog)
{
    Console.WriteLine(item);
    total += item.Minutes;
}
Console.WriteLine($"Total: {total} min");
Console.WriteLine();

Catalog library = new Catalog();
foreach (Media item in catalog)
{
    library.Add(item);
}
Console.WriteLine($"Longest: {library.Longest()?.Title}");
Console.WriteLine($"Total: {library.TotalMinutes()} min");
Console.WriteLine();

Media second = catalog[1];

if (second is Movie movie)
{
    Console.WriteLine($"{movie.Title} is a movie of {movie.Length} min");
}

string kind = second switch
{
    Movie m => $"{m.Length} min",
    Series s => $"{s.Episodes} episodes",
    _ => "something else"
};
Console.WriteLine($"{second.Title}: {kind}");

Movie? asMovie = second as Movie;
Console.WriteLine($"as Movie gives {(asMovie == null ? "null" : asMovie.Title)}");

// The second item is a series, so this cast throws InvalidCastException.
// Movie wrong = (Movie)second;
Console.WriteLine();

IDownloadable download = new Movie("Inception", 2010, 148);
Console.WriteLine($"Download size: {download.SizeInMb} MB");
