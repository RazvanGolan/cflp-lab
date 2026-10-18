---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#0369a1'
title: "Lab 9: Async and await"
info: |
  CFLP laboratory, week 9.
layout: cover
---

# Lab 9: Async and await

Waiting for something without blocking the program, and stopping work nobody needs anymore

---

# Where we left off

In lab 6 the `Player` raised `SongStarted` for every song in its queue, and `PlayAll` finished in an instant.

A real player waits for each song to end. Programs wait all the time: for a file, for a server to answer, for a timer.

Today we write methods that wait with `await`, run several of them at once, and cancel them. At the end we put events and `await` together in a class that does something at a fixed interval. Test 2 asks for one.

---

# Waiting with `Thread.Sleep`

```cs
Console.WriteLine("Kettle on");
Thread.Sleep(3000);
Console.WriteLine("Water boiled");
```

`Thread.Sleep` stops the thread for three seconds. The thread does nothing during that time, and nothing else can use it.

In a desktop application the window freezes, because the same thread draws it. In a web server that thread cannot answer any other request.

It also means two things that wait cannot wait at the same time. Boiling water and making toast take 3 + 2 seconds, even though both are only waiting.

<!--
Ask: how long does breakfast take if you stand and stare at the kettle before you put the bread in the toaster?
-->

---
layout: two-cols
---

# An async method

```cs {1|4|8|13|all}
async Task BoilWaterAsync()
{
    Console.WriteLine("Kettle on");
    await Task.Delay(3000);
    Console.WriteLine("Water boiled");
}

async Task<int> ToastAsync(int slices)
{
    Console.WriteLine($"Toasting {slices} slices");
    await Task.Delay(2000);
    Console.WriteLine("Toast ready");
    return slices;
}
```

::right::

<div class="pl-6 pt-22">

`async` lets a method use `await`. It returns a `Task`, which stands for work that may not be finished yet.

`Task.Delay(3000)` waits three seconds without holding a thread. `await` waits for it to finish.

`Task<int>` is a task that ends with an `int`. The method returns `slices`, and the compiler wraps it in the task.

By convention the name of an async method ends in `Async`.

</div>

<!--
LIVE: the Kitchen project. Write BoilWaterAsync first, then ToastAsync.
A method that would return void returns Task when it is async. One that would return int returns Task<int>.
-->

---

# One after another

```cs
Stopwatch watch = Stopwatch.StartNew();
await BoilWaterAsync();
int slices = await ToastAsync(2);
Console.WriteLine($"{slices} slices after {watch.Elapsed.TotalSeconds:F1} s");
```

<div v-click>

```text
Kettle on
Water boiled
Toasting 2 slices
Toast ready
2 slices after 5.0 s
```

</div>

Each `await` waits for its task before the next line runs, so this still takes 3 + 2 seconds.

`await ToastAsync(2)` gives back the `int` inside the `Task<int>`.

<!--
Stopwatch is in System.Diagnostics. It measures how much time has passed since StartNew.
Top-level statements can use await directly. The compiler makes Main async for us.
-->

---

# Start first, await later

```cs
watch.Restart();
Task water = BoilWaterAsync();
Task<int> toast = ToastAsync(2);
await water;
slices = await toast;
Console.WriteLine($"{slices} slices after {watch.Elapsed.TotalSeconds:F1} s");
```

<div v-click>

```text
Kettle on
Toasting 2 slices
Toast ready
Water boiled
2 slices after 3.0 s
```

</div>

Calling an async method starts it and hands back its `Task` as soon as it reaches its first `await`. Both tasks are running before we wait for either of them.

The toast is ready first, while the program waits for the water.

<!--
Ask before revealing: in what order do the four lines print, and how long does it take?
-->

---

# Waiting for all of them

```cs
await Task.WhenAll(BoilWaterAsync(), ToastAsync(4), BoilWaterAsync());
Console.WriteLine($"All three after {watch.Elapsed.TotalSeconds:F1} s");
```

<div v-click>

```text
Kettle on
Toasting 4 slices
Kettle on
Toast ready
Water boiled
Water boiled
All three after 3.0 s
```

</div>

`Task.WhenAll` returns one task that finishes when every task you gave it has finished. The time is that of the slowest one.

---

# What `await` does

When a method reaches an `await` on a task that is not finished, it stops there and returns to its caller. The caller gets a task that is not finished either.

When the delay ends, a timer inside .NET runs the rest of the method from where it stopped. No thread sits waiting in between.

The compiler does this by rewriting the method into a small object that remembers how far it got and the values of its local variables.

<!--
DEMO: print Environment.CurrentManagedThreadId before and after an await. In a console app the number often changes: the rest of the method runs on any free thread from the thread pool.
If someone asks: the rewritten method is a state machine, a class with a field for each local variable and one for the current position. ILSpy or sharplab.io show it.
-->

---
layout: two-cols
---

# Exceptions

```cs
Task<int> nothing = ToastAsync(0);
Console.WriteLine("No exception yet");
try
{
    await nothing;
}
catch (ArgumentException e)
{
    Console.WriteLine($"Caught: {e.Message}");
}
```

```cs
if (slices <= 0)
{
    throw new ArgumentException("No bread");
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
No exception yet
Caught: No bread
```

</div>

An exception thrown inside an async method is kept in its task. It is thrown again at the `await`, so the `try` goes around the `await`.

That is one more reason to always await a task you start.

</div>

<!--
The throw is at the top of ToastAsync, before its first await, and it still does not come out of the call. Everything in an async method, including its first lines, reports through the task.
-->

---

# Easy mistakes

```cs
BoilWaterAsync();
```

Without `await`, the method starts and the program carries on. If `Main` ends first, the water never boils, and an exception inside it is lost. The compiler warns: `CS4014: Because this call is not awaited...`

```cs
ToastAsync(2).Wait();
int slices = ToastAsync(2).Result;
```

`Wait()` and `.Result` block the thread until the task is done, the same as `Thread.Sleep`. Use `await` instead.

`async void` is only for event handlers in desktop applications. Nobody can await it, and an exception inside it crashes the program. Return `Task`.

<!--
LIVE: uncomment the last line in the Kitchen project and show the warning. CI builds with warnings as errors, which is why the line is commented out.
-->

---
layout: two-cols
---

# Cancelling

```cs {1-2|6|all}
async Task DownloadAsync(string file, int parts,
                         CancellationToken token)
{
    for (int part = 1; part <= parts; part++)
    {
        await Task.Delay(500, token);
        Console.WriteLine(
            $"{file}: {part * 100 / parts}%");
    }
    Console.WriteLine($"{file}: done");
}
```

::right::

<div class="pl-6 pt-22">

A download that may take long gets a `CancellationToken`. The token says whether someone asked to stop. The method can check it but cannot cancel it.

`Task.Delay(500, token)` throws an `OperationCanceledException` as soon as the token is cancelled, even halfway through the wait.

</div>

<!--
LIVE: the Download project. The parts and the delay stand in for a real download.
-->

---

# Pressing Enter to cancel

```cs {1-2|4-6|8-15|all}
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
```

The `CancellationTokenSource` belongs to whoever decides to stop, here `Main`. The download only gets its `Token`.

<!--
Press Enter halfway through. Then run it again and wait until "done": Cancel after the task finished does nothing.
Console.ReadLine blocks the main thread, but the download keeps going, because its code runs on the thread pool after each await.
-->

---

# A time limit

```cs
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
```

<div class="grid grid-cols-2 gap-8">

<div v-click>

```text
lecture-10.mp4: 10%
lecture-10.mp4: 20%
lecture-10.mp4: 30%
Took too long
```

</div>

<div>

`CancelAfter` cancels the token by itself once the time is up.

Slow methods in .NET, such as reading a web page, take a `CancellationToken` the same way.

</div>

</div>

---
layout: two-cols
---

# On a timer

```cs {3|7-18|11|14-15|17|all}
public class Metronome
{
    public event Action<int>? Beat;
    public event Action? Stopped;
    // Tempo and the constructor

    public async Task StartAsync(
        CancellationToken token)
    {
        int count = 0;
        while (!token.IsCancellationRequested)
        {
            count++;
            Beat?.Invoke(count);
            await Task.Delay(60000 / Tempo);
        }
        Stopped?.Invoke();
    }
}
```

::right::

<div class="pl-6 pt-22">

A metronome beats `Tempo` times a minute. It does not know who listens, the same as the `Player` in lab 6.

`StartAsync` loops until it is cancelled. Each round it raises `Beat`, then waits for the next beat with `Task.Delay`.

The loop checks `IsCancellationRequested` itself, so stopping is the normal way out. `Stopped` is raised and nothing throws.

</div>

<!--
LIVE: the Rhythm project. This is the shape Test 2 asks for: a class with an event, raised in a loop in an async StartAsync method, with Task.Delay between rounds.
The token is not passed to Task.Delay here, so after Cancel the loop ends at the next beat. Passing it would stop at once, with an exception to catch.
-->

---

# Wiring it up

```cs
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
```

Subscribe first, then start. `BarCounter.OnBeat` adds a bar every fourth beat, like `PlayCounter` in lab 6.

<!--
The first TOCK prints before "Press Enter to stop": StartAsync runs like a normal method until its first await, and the first Beat comes before that.
-->

---

# Two at once

```cs
Metronome slow = new Metronome(60);
Metronome fast = new Metronome(180);
slow.Beat += beat => Console.WriteLine($"slow {beat}");
fast.Beat += beat => Console.WriteLine($"    fast {beat}");

CancellationTokenSource both = new CancellationTokenSource();
both.CancelAfter(TimeSpan.FromSeconds(3));
await Task.WhenAll(slow.StartAsync(both.Token), fast.StartAsync(both.Token));
```

<div class="grid grid-cols-2 gap-8">

<div v-click>

```text
slow 1
    fast 1
    fast 2
    fast 3
slow 2
...
```

</div>

<div>

One token stops both metronomes. Neither waits for the other, and no thread is blocked while they wait.

</div>

</div>

---

# `async` and `Main`

Top-level statements can use `await` directly. With a classic `Main` method, make it async:

```cs
class Program
{
    static async Task Main()
    {
        Metronome metronome = new Metronome(120);
        // subscribe
        await metronome.StartAsync(token);
    }
}
```

`static void Main` cannot use `await`, and the compiler says so: `CS4033: The 'await' operator can only be used within an async method.`

---

# Summary

| | Example |
|---|---|
| an async method | `async Task<int> ToastAsync(int slices)` |
| wait without blocking | `await Task.Delay(2000);` |
| start now, wait later | `Task water = BoilWaterAsync(); ... await water;` |
| wait for several | `await Task.WhenAll(a, b);` |
| cancel | `source.Cancel();` or `source.CancelAfter(...)` |
| check for cancelling | `token.IsCancellationRequested` |

An exception in a task is thrown at the `await`. Always await the tasks you start.

---

# The debugger

Put a breakpoint on `Beat?.Invoke(count);` in `Metronome.StartAsync` and start debugging.

The first time it stops, the call stack shows `Main` under `StartAsync`, because the method was called from there.

Press `F5` to continue to the second beat. Now `Main` is gone from the call stack. The rest of `StartAsync` was started by the timer behind `Task.Delay`, on a thread from the thread pool.

<!--
In Rider and Visual Studio the call stack can also show the async callers ("async call stack"). Mention it only if the editor shows it.
-->

---
layout: center
---

# Assignment

The statement is on CV UPT. Upload your solution before the deadline shown there.

It uses the same ideas as today: a class that raises an event in an async loop with `Task.Delay`, and other classes that react to it.
