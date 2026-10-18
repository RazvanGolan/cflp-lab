# Lab 10: Extension methods and LINQ

Slides: [lab](https://razvangolan.github.io/cflp-lab/10-linq/slides/)

Writing extension methods, LINQ in method syntax on a list and on a CSV file, iterators with `yield return`, and why a LINQ query runs only when you read its results.

## Code

| Project | What it shows |
|---|---|
| [`Extensions`](code/Extensions) | `WordCount` and `Shorten`, two extension methods for `string`, and `Joined`, one for any `IEnumerable<string>` |
| [`Charts`](code/Charts) | Songs read from `songs.csv` with `File.ReadAllLines`, then `Where`, `Select`, `OrderBy`, `Take`, `Count`, `Sum`, `Average`, `MaxBy`, `First`, `Any` and `GroupBy`, and the same query in query syntax |
| [`Iterators`](code/Iterators) | Two iterators written with `yield return`, one of them endless, a `Where` that runs only during the `foreach`, and `ToList` |

Run any project from the repository root:

```bash
dotnet run --project labs/10-linq/code/Charts
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- An extension method is a static method in a static class, with `this` before its first parameter. You call it as if it belonged to that type, and the file needs a `using` for the namespace of its class.
- `IEnumerable<T>` is anything a `foreach` can walk through: lists, arrays, sets, dictionaries. LINQ methods are extension methods for it, in `System.Linq`.
- `Where` keeps some items, `Select` turns each item into something else, `OrderBy` and `ThenBy` sort, and `Take` and `Skip` cut the sequence. None of them changes the original list.
- `Count`, `Sum`, `Average`, `Min`, `Max`, `MaxBy`, `First` and `Any` return one value. `First` throws when nothing matches, and `FirstOrDefault` returns `null` (or `0` for numbers).
- `GroupBy` returns groups, each with a `Key` and the items that share it.
- To read a file that sits next to the project, copy it to the output folder in the `.csproj` and build the path from `AppContext.BaseDirectory`.
- `yield return` gives one value to the `foreach` and pauses the method until the next one is needed.
- A LINQ query runs when you read it (`foreach`, `Count()`, `ToList()`), and again each time you read it. `ToList()` runs it once and keeps the results.

## Further reading

- [Extension methods](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)
- [LINQ overview](https://learn.microsoft.com/dotnet/csharp/linq/)
- [Standard query operators](https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/)
- [Iterators](https://learn.microsoft.com/dotnet/csharp/iterators)
