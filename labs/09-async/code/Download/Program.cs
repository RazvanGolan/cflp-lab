CancellationTokenSource source = new CancellationTokenSource();
Task download = DownloadAsync("lecture-09.mp4", 10, source.Token);

Console.WriteLine("Press Enter to cancel");
Console.ReadLine();
source.Cancel();

try
{
    await download;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Download cancelled");
}
Console.WriteLine();

// A time limit: the source cancels itself after 2 seconds.
CancellationTokenSource timeout = new CancellationTokenSource();
timeout.CancelAfter(TimeSpan.FromSeconds(2));
try
{
    await DownloadAsync("lecture-10.mp4", 10, timeout.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Took too long");
}

async Task DownloadAsync(string file, int parts, CancellationToken token)
{
    for (int part = 1; part <= parts; part++)
    {
        await Task.Delay(500, token);
        Console.WriteLine($"{file}: {part * 100 / parts}%");
    }
    Console.WriteLine($"{file}: done");
}
