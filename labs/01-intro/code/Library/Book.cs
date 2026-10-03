namespace Library;

public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public int Century => (Year - 1) / 100 + 1;

    public override string ToString()
    {
        return $"{Title} by {Author}, {Year} (century {Century})";
    }
}
