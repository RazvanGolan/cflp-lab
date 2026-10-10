using Playlist;

List<Song> songs =
[
    new Song("Bohemian Rhapsody", "Queen", 354),
    new Song("Imagine", "John Lennon", 183),
    new Song("Hey Jude", "The Beatles", 431),
    new Song("Yesterday", "The Beatles", 125),
    new Song("Hotel California", "Eagles", 391),
];

Console.WriteLine(CountSongs(songs, IsLong));

Func<Song, bool> byBeatles = s => s.Artist == "The Beatles";
Console.WriteLine(CountWhere(songs, byBeatles));
Console.WriteLine(CountWhere(songs, IsLong));
Console.WriteLine(TotalOf(songs, s => s.Seconds));

int limit = 200;
Console.WriteLine(
    CountWhere(songs, s => s.Seconds < limit));

Func<Song, bool> overSix = LongerThan(360);
Console.WriteLine(CountWhere(songs, overSix));

Action<string> title = text => Console.WriteLine($"== {text} ==");
title("Shortest first");

songs.Sort((a, b) =>
    a.Seconds.CompareTo(b.Seconds));
foreach (Song song in songs)
{
    Console.WriteLine(song);
}

int removed =
    songs.RemoveAll(s => s.Seconds > 400);
Console.WriteLine($"Removed {removed}");

bool IsLong(Song song) => song.Seconds > 300;

int CountSongs(List<Song> playlist, SongTest test)
{
    int count = 0;
    foreach (Song song in playlist)
    {
        if (test(song))
        {
            count++;
        }
    }
    return count;
}

int CountWhere<T>(List<T> items,
                  Func<T, bool> test)
{
    int count = 0;
    foreach (T item in items)
    {
        if (test(item))
        {
            count++;
        }
    }
    return count;
}

int TotalOf<T>(List<T> items,
               Func<T, int> value)
{
    int total = 0;
    foreach (T item in items)
    {
        total += value(item);
    }
    return total;
}

Func<Song, bool> LongerThan(int seconds)
{
    return song => song.Seconds > seconds;
}

delegate bool SongTest(Song song);
