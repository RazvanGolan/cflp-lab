# Lab 3: Parameter passing

Slides: [lab](https://razvangolan.github.io/cflp-lab/03-parameters/slides/) · [optional part](https://razvangolan.github.io/cflp-lab/03-parameters/deep-dive/) · [application code](https://github.com/RazvanGolan/study-rooms/tree/week-03)

How arguments reach a method, `ref` and `out` parameters, the `TryParse` pattern, `params`, and a first look at exceptions.

## Code

| Project | What it shows |
|---|---|
| [`ByRef`](code/ByRef) | A method that changes a struct through `ref`, a copy of a reference compared with `ref` on a class variable, and the compiler errors for wrong `ref` arguments |
| [`OutParameters`](code/OutParameters) | A method with two `out` results, and `TryParseTime`, a method that follows the `TryParse` pattern |
| [`Maximum`](code/Maximum) | A `Max` method that takes any number of arguments with `params`, and throws an `ArgumentException` when it gets none |
| [`Exceptions`](code/Exceptions) | Reading a file inside `try`, with a `catch` for a missing file |

Run any project from the repository root:

```bash
dotnet run --project labs/03-parameters/code/OutParameters
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- Without `ref` or `out`, a method gets a copy of each argument. For a class, the copy is a copy of the reference: the method can change the object, but it cannot make the caller's variable point to a different one.
- `ref` and `out` are written both in the method and in the call.
- A `ref` argument must be a variable that already has a value. A literal or a property is not accepted.
- An `out` parameter does not need a value before the call, and the method must assign it before it returns.
- A method that can fail on normal input returns `bool` and gives its result through `out`, like `int.TryParse`. It sets the `out` values to a default when it fails.
- `out _` discards a result you do not need.
- `params` collects the arguments into an array, so the method has to decide what to do when the array is empty.
- An exception that nothing catches stops the program. The first line of the output gives the type and the message, and the lines below show where it happened.
- `throw` a clear exception when a method gets arguments it cannot work with. Use `try` and `catch` only for errors you can handle, such as a missing file.

## Further reading

- [Method parameters](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/method-parameters)
- [The `out` parameter modifier](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/out-parameter-modifier)
- [Discards](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/discards)
- [Exceptions and exception handling](https://learn.microsoft.com/dotnet/csharp/fundamentals/exceptions/)
