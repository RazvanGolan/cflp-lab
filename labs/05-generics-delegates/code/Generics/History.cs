namespace Generics;

public class History<T>
{
    private readonly List<T> items = [];
    private readonly int capacity;

    public History(int capacity)
    {
        this.capacity = capacity;
    }

    public int Count => items.Count;

    public void Add(T item)
    {
        if (items.Count == capacity)
        {
            items.RemoveAt(0);
        }
        items.Add(item);
    }

    public T Last()
    {
        return items[items.Count - 1];
    }

    public void Print()
    {
        foreach (T item in items)
        {
            Console.WriteLine(item);
        }
    }
}
