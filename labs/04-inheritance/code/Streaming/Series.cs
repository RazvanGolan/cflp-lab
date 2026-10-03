namespace Streaming;

public class Series : Media
{
    public int Episodes { get; }
    public int EpisodeLength { get; }

    public Series(string title, int year,
                  int episodes, int episodeLength)
        : base(title, year)
    {
        Episodes = episodes;
        EpisodeLength = episodeLength;
    }

    public override int Minutes
        => Episodes * EpisodeLength;

    public override string ToString()
    {
        return $"{base.ToString()}, {Episodes} episodes";
    }
}
