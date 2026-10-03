namespace TaskBoard;

public class TaskItem
{
    private static int nextId = 1;

    private int estimate;

    public int Id { get; }
    public string Title { get; }
    public TaskState State { get; private set; }

    public int Estimate
    {
        get { return estimate; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("The estimate cannot be negative.");
            }
            estimate = value;
        }
    }

    public TaskItem(string title, int estimate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A task needs a title.");
        }
        Id = nextId++;
        Title = title;
        Estimate = estimate;
        State = TaskState.Todo;
    }

    public TaskItem(string title) : this(title, 1)
    {
    }

    public bool Start()
    {
        if (State != TaskState.Todo)
        {
            return false;
        }
        State = TaskState.InProgress;
        return true;
    }

    public bool Complete()
    {
        if (State != TaskState.InProgress)
        {
            return false;
        }
        State = TaskState.Done;
        return true;
    }

    public override string ToString()
    {
        return $"#{Id} {Title}, {State}, {Estimate}h";
    }
}
