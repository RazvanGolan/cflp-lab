# Lab 5: Generics and delegates

Slides: [lab](https://razvangolan.github.io/cflp-lab/05-generics-delegates/slides/) · [optional part](https://razvangolan.github.io/cflp-lab/05-generics-delegates/deep-dive/) · [application code](https://github.com/RazvanGolan/study-rooms/tree/week-05)

Generic collections, a generic class and a generic method of our own, then delegates, `Func` and `Action`, and lambdas.

## Code

| Project | What it shows |
|---|---|
| [`Collections`](code/Collections) | `List<T>` with `Add`, `Remove` and `RemoveAt`, a `Dictionary<string, int>` that counts words with `TryGetValue`, and a `HashSet<string>` |
| [`Generics`](code/Generics) | `History<T>`, a generic class that keeps the last few items, and a generic `Max<T>` with a `where` constraint |
| [`Playlist`](code/Playlist) | A delegate type, methods that take a `Func`, lambdas that use local variables, a method that returns a `Func`, and `Sort` and `RemoveAll` with lambdas |

Run any project from the repository root:

```bash
dotnet run --project labs/05-generics-delegates/code/Playlist
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- A generic class or method has a type parameter, usually `T`. Whoever uses it picks the type, and the compiler checks it.
- Removing items from a list inside a `foreach` over it throws an `InvalidOperationException`.
- `Dictionary<TKey, TValue>` finds a value by its key. `TryGetValue` does not throw when the key is missing, and the indexer does.
- A `HashSet<T>` keeps unique items. It uses `GetHashCode` to pick a bucket, so `Contains` is fast however many items there are. `Dictionary` hashes its keys the same way. Equal items must give the same hash code, so a class used as a key or in a set overrides `Equals` and `GetHashCode` together.
- Inside generic code the compiler knows nothing about `T`. A `where` constraint, such as `where T : IComparable<T>`, says what `T` must be able to do.
- A delegate type describes the shape of a method: what it takes and what it returns. `Func<..., TResult>` returns the last type, and `Action<...>` returns nothing.
- A method name without parentheses passes the method itself, so `IsLong` and `IsLong()` are different things.
- A lambda such as `s => s.Seconds > 300` is a method without a name. It can use the local variables around it, and it reads their current value each time it runs.

## Further reading

- [Generics](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/generics)
- [Collections](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/collections)
- [Delegates](https://learn.microsoft.com/dotnet/csharp/programming-guide/delegates/)
- [Lambda expressions](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions)
