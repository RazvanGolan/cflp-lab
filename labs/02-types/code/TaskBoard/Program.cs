using TaskBoard;

Board board = new Board();
board.Add(new TaskItem("Write the README"));
board.Add(new TaskItem("Fix the login bug", 3));
board.Add(new TaskItem("Prepare the demo", 2));

TaskItem? bug = board.Find(2);
if (bug != null)
{
    bug.Start();
}

board.Print();
Console.WriteLine($"In progress: {board.CountIn(TaskState.InProgress)}");
