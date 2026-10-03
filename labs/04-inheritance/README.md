# Lab 4: Inheritance

Slides: [lab](https://razvangolan.github.io/cflp-lab/04-inheritance/slides/)

Base classes, abstract members, overriding, casts and pattern matching, and a list of the base type used without casting.

## Code

| Project | What it shows |
|---|---|
| [`Streaming`](code/Streaming) | An abstract `Media` class with `Movie` and `Series`, an abstract `Minutes` property and `ToString` overrides, a `Catalog` that keeps a `List<Media>`, an `IDownloadable` interface, and `is`, `as` and a type `switch` |

Run it from the repository root:

```bash
dotnet run --project labs/04-inheritance/code/Streaming
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- `class Movie : Media` makes `Movie` inherit everything `Media` has. A class has at most one base class, and the base class comes before any interfaces.
- A derived constructor calls a base constructor with `: base(...)`, and the base constructor runs first.
- `protected` members can be used by the class and by the classes derived from it.
- A variable of the base type can hold any derived object, but you can only use the members of the base type through it.
- An `abstract` member has no code, and every derived class must `override` it. An abstract class cannot be created with `new`.
- A `virtual` member has code that a derived class may `override`. `ToString` is virtual in `object`, the base of every class.
- Without `override`, a method with the same name only hides the base one. The compiler warns about it (CS0114), and code that uses the base type still runs the base version.
- A cast to the wrong type throws `InvalidCastException`. `is`, `as` and a `switch` on the type do not throw.
- When you are tempted to check the type of each object in a list, add an abstract or virtual member to the base class instead.

## Further reading

- [Inheritance](https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [Abstract and sealed classes and class members](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
- [Polymorphism](https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching)
