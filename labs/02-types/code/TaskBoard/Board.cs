namespace TaskBoard;

public class Board
{
    private readonly List<TaskItem> tasks = [];

    public void Add(TaskItem task)
    {
        tasks.Add(task);
    }

    public TaskItem? Find(int id)
    {
        foreach (TaskItem task in tasks)
        {
            if (task.Id == id)
            {
                return task;
            }
        }
        return null;
    }

    public int CountIn(TaskState state)
    {
        int count = 0;
        foreach (TaskItem task in tasks)
        {
            if (task.State == state)
            {
                count++;
            }
        }
        return count;
    }

    public void Print()
    {
        foreach (TaskItem task in tasks)
        {
            Console.WriteLine(task);
        }
    }
}
