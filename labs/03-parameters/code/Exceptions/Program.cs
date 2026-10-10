try
{
    string text = File.ReadAllText("notes.txt");
    Console.WriteLine(text);
}
catch (FileNotFoundException)
{
    Console.WriteLine("No notes yet.");
}

Console.WriteLine("Done");

// Create notes.txt in the folder you run from, then run again.
