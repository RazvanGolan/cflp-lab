# Lab 6: Events

Slides: [lab](https://razvangolan.github.io/cflp-lab/06-events/slides/)

Delegates that hold several methods, events, subscribing and unsubscribing, then a recap for Test 1.

## Code

| Project | What it shows |
|---|---|
| [`Multicast`](code/Multicast) | One `Action<string>` that holds two methods, `+=` and `-=`, and `?.Invoke` on a delegate that may be `null` |
| [`Music`](code/Music) | A `Player` that raises `SongStarted` and `QueueFinished`, a lambda and a `PlayCounter` that subscribe to them, and unsubscribing with `-=` |

Run any project from the repository root:

```bash
dotnet run --project labs/06-events/code/Music
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- A delegate can hold several methods. `+=` adds one, `-=` removes one, and a call runs all of them in the order they were added.
- A delegate with no methods is `null`. Raise an event with `?.Invoke(...)` so that it works with no subscribers.
- `public event Action<T>? Name;` declares an event. Code outside the class can only use `+=` and `-=` on it, so only the class itself can raise it.
- A subscriber can be a lambda or a method of another object, as long as it has the right shape.
- `-=` with a lambda written again removes nothing, because it is a new method. Keep the lambda in a variable if you need to unsubscribe it.
- Subscribers run one after another, and the code that raised the event continues only after all of them have finished.

## Before Test 1

Go over the checklists in the last part of the slides, then redo the practice exercise without looking at earlier labs. The full examples are in lab 3 (`out` and `TryParse`), lab 4 (abstract classes) and lab 5 (methods that take a `Func`).

## Further reading

- [Events](https://learn.microsoft.com/dotnet/csharp/programming-guide/events/)
- [Delegates and events compared](https://learn.microsoft.com/dotnet/csharp/distinguish-delegates-events)
- [Combining delegates](https://learn.microsoft.com/dotnet/csharp/programming-guide/delegates/how-to-combine-delegates-multicast-delegates)
