using Rhythm;

Metronome metronome = new Metronome(120);
metronome.Beat += beat => Console.WriteLine(beat % 4 == 1 ? "TOCK" : "tick");

BarCounter counter = new BarCounter(4);
metronome.Beat += counter.OnBeat;
metronome.Stopped += () => Console.WriteLine($"Stopped after {counter.Bars} full bars");

CancellationTokenSource source = new CancellationTokenSource();
Task running = metronome.StartAsync(source.Token);

Console.WriteLine("Press Enter to stop");
Console.ReadLine();
source.Cancel();
await running;
Console.WriteLine();

// Two metronomes at once, stopped after 3 seconds.
Metronome slow = new Metronome(60);
Metronome fast = new Metronome(180);
slow.Beat += beat => Console.WriteLine($"slow {beat}");
fast.Beat += beat => Console.WriteLine($"    fast {beat}");

CancellationTokenSource both = new CancellationTokenSource();
both.CancelAfter(TimeSpan.FromSeconds(3));
await Task.WhenAll(slow.StartAsync(both.Token), fast.StartAsync(both.Token));
Console.WriteLine("Both stopped");
