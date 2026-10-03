namespace Streaming;

public abstract class Media
{
    public string Title { get; }
    public int Year { get; }

    protected Media(string title, int year)
    {
        Title = title;
        Year = year;
    }

    public abstract int Minutes { get; }

    public override string ToString()
    {
        return $"{Title} ({Year}), {Minutes} min";
    }
}
