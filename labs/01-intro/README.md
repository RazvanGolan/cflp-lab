# Lab 1: Introduction to C#

Slides: [lab](https://razvangolan.github.io/cflp-lab/01-intro/slides/) · [deep dive (optional)](https://razvangolan.github.io/cflp-lab/01-intro/deep-dive/)

Your first programs, console input and output, and your first class.

## Code

| Project | What it shows |
|---|---|
| [`Hello`](code/Hello) | The program `dotnet new console` creates, with top-level statements |
| [`ClassicMain`](code/ClassicMain) | The same program with an explicit class and `Main` method |
| [`TopLevel`](code/TopLevel) | How a program with top-level statements runs: order of statements, a function, `args` |
| [`ConsoleIO`](code/ConsoleIO) | Printing with string interpolation, reading input, `int.Parse` and `int.TryParse` |
| [`Temperatures`](code/Temperatures) | Reading `n` numbers into an array and computing the minimum, maximum and average |
| [`Library`](code/Library) | A `Book` class with read-only properties, a computed property and `ToString` |
| [`UnderTheHood`](code/UnderTheHood) | Deep dive: the generated `Main` method, raw IL bytes, and a method the JIT recompiles |

Run any project from the repository root:

```bash
dotnet run --project labs/01-intro/code/Library
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- `Console.ReadLine()` returns `null` when there is no more input, so its type is `string?`.
- `int.Parse` throws an exception on text that is not a number. `int.TryParse` returns `false` instead.
- `double.Parse` follows the system language settings. On a machine set to Romanian, `21.5` is read as `215`.
- Dividing two `int` values gives an `int`: `7 / 2` is `3`.
- Without `override ToString()`, printing an object shows only its type name.

## Further reading

- [A tour of C#](https://learn.microsoft.com/dotnet/csharp/tour-of-csharp/)
- [Top-level statements](https://learn.microsoft.com/dotnet/csharp/fundamentals/program-structure/top-level-statements)
- [Properties](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/properties)
- [Tiered compilation](https://learn.microsoft.com/dotnet/core/runtime-config/compilation#tiered-compilation)
