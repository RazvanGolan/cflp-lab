using Library;

Book[] books =
[
    new Book("War and Peace", "Leo Tolstoy", 1869),
    new Book("The Hobbit", "J. R. R. Tolkien", 1937),
    new Book("Dune", "Frank Herbert", 1965),
];

foreach (Book book in books)
{
    Console.WriteLine(book);
}

Book oldest = books[0];
foreach (Book book in books)
{
    if (book.Year < oldest.Year)
    {
        oldest = book;
    }
}

Console.WriteLine($"Oldest: {oldest.Title}");
