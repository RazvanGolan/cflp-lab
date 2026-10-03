namespace Streaming;

public class Catalog
{
    private readonly List<Media> items = [];

    public void Add(Media item)
    {
        items.Add(item);
    }

    public int TotalMinutes()
    {
        int total = 0;
        foreach (Media item in items)
        {
            total += item.Minutes;
        }
        return total;
    }

    public Media? Longest()
    {
        Media? longest = null;
        foreach (Media item in items)
        {
            if (longest == null
                || item.Minutes > longest.Minutes)
            {
                longest = item;
            }
        }
        return longest;
    }
}
