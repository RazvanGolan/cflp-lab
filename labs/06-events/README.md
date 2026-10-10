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

Go over the checklists in the last part of the slides. The full examples are in lab 3 (`out` and `TryParse`), lab 4 (abstract classes) and lab 5 (methods that take a `Func`).

Then do the practice exercises from the slides in a new project (`dotnet new console -n Parcels`), without looking at earlier labs:

1. `bool TryGetCost(string kind, double kg, out double cost)`. A `"letter"` costs 2 lei and a `"box"` costs 5 lei plus 3 lei per kg. It fails for any other kind, or for a weight of zero or less.
2. `string? FirstWhere(List<string> words, Func<string, bool> test)` returns the first word that passes the test, or `null`. Call it with two different lambdas.
3. An abstract class `Parcel` with a `Weight` and an abstract `Cost()`, and two classes `Letter` and `Box` with the prices above. Both override `ToString`. Put letters and boxes in a `List<Parcel>`, then print each one and the total cost, without casts.

## Further reading

- [Events](https://learn.microsoft.com/dotnet/csharp/programming-guide/events/)
- [Delegates and events compared](https://learn.microsoft.com/dotnet/csharp/distinguish-delegates-events)
- [Combining delegates](https://learn.microsoft.com/dotnet/csharp/programming-guide/delegates/how-to-combine-delegates-multicast-delegates)
