namespace Music;

public class Song
{
    public string Title { get; }
    public string Artist { get; }
    public int Seconds { get; }

    public Song(string title, string artist, int seconds)
    {
        Title = title;
        Artist = artist;
        Seconds = seconds;
    }

    public override string ToString()
    {
        return $"{Title}, {Artist} ({Seconds / 60}:{Seconds % 60:D2})";
    }
}
