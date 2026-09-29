namespace Library;

public class Book
{
    #region properties
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    #endregion

    #region constructor
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }
    #endregion

    #region computed
    public int Century => (Year - 1) / 100 + 1;
    #endregion

    #region tostring
    public override string ToString()
    {
        return $"{Title} by {Author}, {Year} (century {Century})";
    }
    #endregion
}
