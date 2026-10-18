# Lab 9: Async and await

Slides: [lab](https://razvangolan.github.io/cflp-lab/09-async/slides/)

Async methods and `await`, running several tasks at once, exceptions in tasks, cancellation, and a class that raises an event at a fixed interval from an async loop.

## Code

| Project | What it shows |
|---|---|
| [`Kitchen`](code/Kitchen) | Two async methods awaited one after another, then started together, `Task.WhenAll`, and an exception that comes out at the `await` |
| [`Download`](code/Download) | A `CancellationTokenSource` cancelled when you press Enter, and a time limit with `CancelAfter` |
| [`Rhythm`](code/Rhythm) | A `Metronome` that raises `Beat` in a loop in `StartAsync`, a `BarCounter` that subscribes to it, and two metronomes running at once |

Run any project from the repository root:

```bash
dotnet run --project labs/09-async/code/Rhythm
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- An `async` method can use `await`. It returns `Task` when it has no result and `Task<T>` when it returns a `T`.
- `await` waits for a task without blocking the thread. `Thread.Sleep`, `Wait()` and `.Result` block it.
- Calling an async method starts it. Keep the `Task` in a variable to await it later, so that several tasks run at the same time. `Task.WhenAll` waits for all of them.
- An exception inside an async method is thrown at the `await`, so put the `try` around the `await`.
- Always await the tasks you start. The compiler warns (CS4014) when you forget, and a program that ends does not wait for tasks still running.
- Avoid `async void`. Nobody can await it, and an exception inside it crashes the program.
- A `CancellationTokenSource` cancels, and the work gets its `Token`. `Task.Delay(ms, token)` throws an `OperationCanceledException` when the token is cancelled. A loop can also check `token.IsCancellationRequested` and stop on its own.
- A class that does something at a fixed interval raises an event in a loop inside an async `StartAsync` method, with `await Task.Delay` between rounds. Subscribe to the event before you start it.
- Top-level statements can use `await`. A classic `Main` has to be `static async Task Main()`.

## Further reading

- [Asynchronous programming with async and await](https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/)
- [The task asynchronous programming model](https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/task-asynchronous-programming-model)
- [Cancellation in managed threads](https://learn.microsoft.com/dotnet/standard/threading/cancellation-in-managed-threads)
