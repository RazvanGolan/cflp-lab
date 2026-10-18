namespace Charts;

public class Song
{
    public string Title { get; }
    public string Artist { get; }
    public int Year { get; }
    public int Seconds { get; }
    public string Genre { get; }

    public Song(string title, string artist, int year, int seconds, string genre)
    {
        Title = title;
        Artist = artist;
        Year = year;
        Seconds = seconds;
        Genre = genre;
    }

    public static Song Parse(string line)
    {
        string[] parts = line.Split(',');
        return new Song(parts[0], parts[1],
            int.Parse(parts[2]), int.Parse(parts[3]), parts[4]);
    }

    public override string ToString()
    {
        return $"{Title}, {Artist} ({Year}, {Seconds / 60}:{Seconds % 60:D2})";
    }
}
