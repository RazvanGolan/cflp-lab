# Lab 2: Classes, structs, enums and interfaces

Slides: [lab](https://razvangolan.github.io/cflp-lab/02-types/slides/) · [optional part](https://razvangolan.github.io/cflp-lab/02-types/deep-dive/)

Value and reference types, enums, properties and constructors, static members, and a first interface.

## Code

| Project | What it shows |
|---|---|
| [`ValueVsReference`](code/ValueVsReference) | A struct and a class with the same properties: what `=` copies, and what a method can change |
| [`Weather`](code/Weather) | An enum, a `switch` expression over it, and reading it from the console with `Enum.TryParse` |
| [`TaskBoard`](code/TaskBoard) | A `TaskItem` class with `private set`, a setter that checks its value, two constructors and a static id counter, and a `Board` class that keeps the tasks in a `List<T>` |
| [`Shapes`](code/Shapes) | An interface implemented by a class and by a struct, used through a `List<IShape>` |

Run any project from the repository root:

```bash
dotnet run --project labs/02-types/code/TaskBoard
```

The code here is the cleaned-up version of what we wrote live, so it may differ slightly from the lab.

## Things to remember

- Assigning a struct copies its values. Assigning an object of a class copies the reference, so both variables point to the same object.
- A method gets a copy of a struct argument, so changes made inside the method are lost when it returns.
- `Enum.TryParse` accepts any number, even one that has no name in the enum. Check the result with `Enum.IsDefined`.
- A property is a pair of methods, `get` and `set`. Keep fields private and let other code use properties.
- `{ get; private set; }` lets code inside the class change a property and stops code outside it.
- If a constructor throws an exception, the object is never created.
- A `static` field has one copy, shared by every object of the class.
- `=>` before the body of a member is short for a body that only returns that expression: `Area() => side * side` is `Area() { return side * side; }`.

## Further reading

- [Value types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types)
- [Choosing between class and struct](https://learn.microsoft.com/dotnet/standard/design-guidelines/choosing-between-class-and-struct)
- [Enumeration types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/enum)
- [Properties](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/properties)
- [Constructors](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/constructors)
- [Static classes and static members](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members)
- [Interfaces](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/interfaces)
