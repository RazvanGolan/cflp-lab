namespace Streaming;

public class Movie : Media, IDownloadable
{
    public int Length { get; }

    public Movie(string title, int year,
                 int length)
        : base(title, year)
    {
        Length = length;
    }

    public override int Minutes => Length;

    public int SizeInMb => Length * 30;
}
