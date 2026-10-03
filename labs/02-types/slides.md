---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#0f766e'
title: "Lab 2: Classes, structs, enums and interfaces"
info: |
  CFLP laboratory, week 2.
layout: cover
---

# Lab 2: Classes, structs, enums and interfaces

The kinds of types you can define, and how to control who changes an object

---

# Where we left off

Last week we wrote a `Book` class. Its properties had only `get`, so nothing could change a book after the constructor ran.

Today we look at the other kinds of types C# lets you define:

- a `class`, which you already know
- a `struct`, which looks like a class but is copied differently
- an `enum`, a fixed list of named values
- an `interface`, a list of members that several types promise to have

We also look at properties that the class itself can change, and at constructors that refuse bad data.

---
layout: two-cols
---

# Struct or class?

```cs
struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

```cs
class Marker
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

The two declarations differ only in the first word.

::right::

<div class="pl-6 pt-22">

```cs {1-3|5-7|all}
Point p1 = new Point { X = 1, Y = 2 };
Point p2 = p1;
p2.X = 10;

Marker m1 = new Marker { X = 1, Y = 2 };
Marker m2 = m1;
m2.X = 10;

Console.WriteLine($"p1.X = {p1.X}");
Console.WriteLine($"m1.X = {m1.X}");
```

<div v-click>

```text
p1.X = 1
m1.X = 10
```

</div>

</div>

<!--
LIVE: write both types and the program from an empty project. Ask the class to guess both lines before revealing.
The braces after `new Point` are an object initializer: they set properties right after the object is created.
-->

---

# What `=` copies

A struct variable holds the values themselves. `p2 = p1` copies them, so `p2` is a separate point.

A class variable holds a reference to an object stored somewhere else in memory. `m2 = m1` copies the reference, so both variables point to the same object.

```text
p1   [ X = 1  | Y = 2 ]
p2   [ X = 10 | Y = 2 ]

m1 ──┐
     ├──> [ X = 10 | Y = 2 ]
m2 ──┘
```

Structs are called value types. Classes are called reference types.

<!--
Stay at this level. Stack and heap come up in the optional part if someone asks.
`int`, `double`, `bool` and `DateTime` are structs. `string` is a class, but it never changes after it is created, so it feels like a value.
-->

---
layout: two-cols
---

# Passing to a method

The same rule applies to parameters: a method gets a copy of what the variable holds.

```cs
void MovePoint(Point p)
{
    p.X += 5;
}

void MoveMarker(Marker m)
{
    m.X += 5;
}
```

::right::

<div class="pl-6 pt-22">

```cs {1-2|3-4|5-6|all}
Point point = new Point { X = 0, Y = 0 };
Marker marker = new Marker { X = 0, Y = 0 };
MovePoint(point);
MoveMarker(marker);
Console.WriteLine($"point.X = {point.X}");
Console.WriteLine($"marker.X = {marker.X}");
```

<div v-click>

```text
point.X = 0
marker.X = 5
```

`MovePoint` changed its own copy. In lab 3 we use `ref` to let a method change the caller's struct.

</div>

</div>

<!--
Local functions in a top-level program cannot be overloaded, which is why the two methods have different names.
-->

---
layout: two-cols
---

# When to choose a struct

Use a struct for a small value that is defined by its contents, such as a point or a colour. Two red colours are the same colour.

Use a class for things that change over time and have their own identity, such as a student or an order. When in doubt, use a class.

`readonly` makes the compiler reject anything that could change the struct after the constructor. A value that cannot change can be copied safely.

::right::

<div class="pl-6 pt-22">

```cs
public readonly struct Color
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
}
```

</div>

---

# Enums

An enum is a type with a fixed list of named values.

```cs
enum Weather
{
    Sunny,
    Cloudy,
    Rainy,
    Snowy
}
```

```cs {1|2|3|all}
Weather today = Weather.Rainy;
Console.WriteLine(today);
Console.WriteLine((int)today);
```

<div v-click>

```text
Rainy
2
```

Each name is stored as an `int`, starting from `0`. Printing an enum shows its name.

</div>

<!--
Ask: why not use a string, "rainy"? A typo like "rainny" compiles and fails at run time. `Weather.Rainny` does not compile.
-->

---

# A `switch` expression on an enum

```cs {3|5-8|8|all}
string Advice(Weather weather)
{
    return weather switch
    {
        Weather.Sunny => "Take sunglasses",
        Weather.Rainy => "Take an umbrella",
        Weather.Snowy => "Wear boots",
        _ => "Nothing special today"
    };
}
```

Each line compares the value with a case and gives a result. The first case that matches wins. `_` matches anything that is left.

<!--
LIVE: remove the `_` line and show the compiler warning that `Weather.Cloudy` is not handled.
-->

---
layout: two-cols
---

# Reading an enum

```cs {1-4|6-7|9-17|all}
foreach (Weather w in Enum.GetValues<Weather>())
{
    Console.WriteLine($"{(int)w}. {w}");
}

Console.Write("What is the weather like? ");
string text = Console.ReadLine() ?? "";

if (Enum.TryParse(text, ignoreCase: true,
                  out Weather weather))
{
    Console.WriteLine(Advice(weather));
}
else
{
    Console.WriteLine("Unknown weather");
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
0. Sunny
1. Cloudy
2. Rainy
3. Snowy
What is the weather like? rainy
Take an umbrella
```

</div>

`Enum.TryParse` works like `int.TryParse` from last week. It accepts the name or the number.

</div>

<!--
`ignoreCase: true` is a named argument. It makes the code easier to read than a bare `true`.
-->

---

# A number that is not on the list

What happens if the user types `42`?

<div v-click>

```text
What is the weather like? 42
Nothing special today
```

</div>

<div v-click>

`TryParse` accepted `42`, because any `int` fits in an enum, even one without a name. Check that the value is on the list:

```cs
if (Enum.TryParse(text, ignoreCase: true, out Weather weather)
    && Enum.IsDefined(weather))
```

</div>

---
layout: two-cols
---

# What is a property

A field is a variable stored inside each object. In Java you keep it private and add one method to read it and one to change it:

```cs
private string title = "";

public string GetTitle()
{
    return title;
}

public void SetTitle(string value)
{
    title = value;
}
```

```cs
book.SetTitle("Dune");
Console.WriteLine(book.GetTitle());
```

::right::

<div class="pl-6 pt-22">

C# puts the two methods together in a property. From outside it is used like a field.

```cs
private string title = "";

public string Title
{
    get { return title; }
    set { title = value; }
}
```

```cs
book.Title = "Dune";
Console.WriteLine(book.Title);
```

</div>

<!--
By convention, fields are camelCase and properties are PascalCase.
-->

---

# A property is a pair of methods

The `get` and `set` blocks are called accessors. The compiler turns each one into a method, `get_Title()` and `set_Title(string value)`. Inside `set`, `value` is the parameter: whatever is on the right of the `=`.

```cs
book.Title = "Dune";             // calls set_Title("Dune")
Console.WriteLine(book.Title);   // calls get_Title()
```

Because the accessors are methods, they can do more than copy a field. A setter can check the new value, and a getter can calculate its result, as `Century` did last week.

When the accessors only read and write a field, C# writes them for you:

```cs
public string Title { get; set; }
```

This is an auto-property. The compiler creates the hidden field and both methods.

<!--
If there is time, show get_Title and set_Title in the compiled .dll with the tools from the lab 1 deep dive.
-->

---

# Field or property?

| | Field | Property |
|---|---|---|
| What it is | a variable | a `get` and a `set` method |
| Runs code when used | no | yes |
| Public to read, private to write | no | yes |
| Usually | private | public |

Keep fields private and let other code use properties. You can then add a check to a setter later without changing the code that uses it.

<!--
Naming: fields are camelCase, properties are PascalCase.
Interfaces (end of this lab) can declare properties but not fields, so this is another reason to expose properties.
-->

---

# Who can change a property

| Auto-property | Who can set it | `GetX()` | `SetX()` |
|---|---|---|---|
| `{ get; set; }` | any code | public | public |
| `{ get; private set; }` | code in the class | public | private |
| `{ get; }` | the constructor | public | none |

`{ get; init; }` is like `{ get; }`, but an object initializer can also set it once: `new Book { Title = "Dune" }`. Java has nothing similar.

Start with the most restrictive one that works.

<!--
For `{ get; }` the hand-written version also needs a `readonly` field, set in the constructor.
Every extra writer is another place to look when a value is wrong.
-->

---
layout: two-cols
---

# A task that controls its own state

```cs
public enum TaskState { Todo, InProgress, Done }
```

```cs
public class TaskItem
{
    public int Id { get; }
    public string Title { get; }
    public TaskState State { get; private set; }
}
```

Outside the class:

```cs
Console.WriteLine(task.State);  // works
task.State = TaskState.Done;    // error
```

::right::

<div class="pl-6 pt-22">

`State` written by hand would look like this:

```cs
private TaskState state;

public TaskState GetState()
{
    return state;
}

private void SetState(TaskState value)
{
    state = value;
}
```

```cs
Console.WriteLine(task.GetState());  // works
task.SetState(TaskState.Done);       // error
```

</div>

<!--
LIVE: start the TaskBoard project here. Create TaskState.cs and TaskItem.cs.
The class is called TaskItem because `Task` already exists in .NET.
The compiler error for the assignment is CS0272: the set accessor is inaccessible. Show it live.
-->

---
layout: two-cols-header
---

# Methods that change the state

The only way to change `State` is through these methods, so a task can never jump from `Todo` straight to `Done`. They return `false` when the change is not allowed, and the caller decides what to do.

::left::

```cs
public bool Start()
{
    if (State != TaskState.Todo)
    {
        return false;
    }
    State = TaskState.InProgress;
    return true;
}
```

::right::

<div class="pl-6">

```cs
public bool Complete()
{
    if (State != TaskState.InProgress)
    {
        return false;
    }
    State = TaskState.Done;
    return true;
}
```

</div>

---

# A setter that checks the value

```cs {1|3-15|7-10|all}
private int estimate;

public int Estimate
{
    get { return estimate; }
    set
    {
        if (value < 0)
        {
            throw new ArgumentException("The estimate cannot be negative.");
        }
        estimate = value;
    }
}
```

This is the long form of a property, with a check in the setter. `throw` stops the program with an error, unless someone catches it. Exceptions come in lab 5.

<!--
Since C# 14 you can write `field` inside the accessors instead of declaring `estimate` yourself. Mention it only if someone asks.
-->

---

# Constructors

```cs {1-6|8-10|all}
public TaskItem(string title, int estimate)
{
    Title = title;
    Estimate = estimate;
    State = TaskState.Todo;
}

public TaskItem(string title) : this(title, 1)
{
}
```

A class can have several constructors with different parameters. `: this(title, 1)` calls the other constructor first, so the code that sets the properties exists only once.

<v-click>

A default value for the parameter gives the same result with one constructor:

```cs
public TaskItem(string title, int estimate = 1)
```

</v-click>

---

# A constructor that refuses bad data

```cs {3-6|all}
public TaskItem(string title, int estimate)
{
    if (string.IsNullOrWhiteSpace(title))
    {
        throw new ArgumentException("A task needs a title.");
    }
    Title = title;
    Estimate = estimate;
    State = TaskState.Todo;
}
```

If the constructor throws, the object is never created. Every `TaskItem` that exists has a title.

<div v-click>

```text
new TaskItem("   ");
Unhandled exception. System.ArgumentException: A task needs a title.
```

</div>

<!--
`Estimate = estimate` goes through the setter from the previous slide, so a negative estimate is rejected here too.
-->

---

# `static` members

A `static` field belongs to the type, so there is one copy shared by every object. We use one to give each task its own id.

```cs {1|5|all}
private static int nextId = 1;

public TaskItem(string title, int estimate)
{
    Id = nextId++;
    // ... the rest as before
}
```

`nextId++` gives the current value to `Id`, then adds one to `nextId`. The first task gets `1`, the next one `2`.

`Console.WriteLine` and `int.Parse` are static methods too. You call them on the type name, `Console` or `int`, and never create a `Console` object first.

---

# `List<T>`: an array that grows

An array has a fixed size. A `List<T>` grows when you add to it. `T` is the type of the elements.

```cs {1|2-3|5|6-9|all}
List<string> names = [];
names.Add("Walter White");
names.Add("Jesse Pinkman");

Console.WriteLine(names.Count);
foreach (string name in names)
{
    Console.WriteLine(name);
}
```

`names[0]` reads the first element, the same as with an array. We look at lists and other collections in more detail in lab 5.

---
layout: two-cols
---

# A class that keeps other objects

```cs
public class Board
{
    private readonly List<TaskItem> tasks = [];

    public void Add(TaskItem task)
    {
        tasks.Add(task);
    }

    // Find, CountIn and Print
}
```

The list is private, so other code can only use the board through its methods.

::right::

<div class="pl-6 pt-22">

```cs
public TaskItem? Find(int id)
{
    foreach (TaskItem task in tasks)
    {
        if (task.Id == id)
        {
            return task;
        }
    }
    return null;
}
```

`Find` returns `null` when there is no task with that id, which is why its type is `TaskItem?`.

</div>

<!--
LIVE. Then add `CountIn(TaskState state)` and `Print()` the same way; they are used on the next slide.
-->

---

# Using the board

```cs {1-4|6-10|12-13|all}
Board board = new Board();
board.Add(new TaskItem("Write the README"));
board.Add(new TaskItem("Fix the login bug", 3));
board.Add(new TaskItem("Prepare the demo", 2));

TaskItem? bug = board.Find(2);
if (bug != null)
{
    bug.Start();
}

board.Print();
Console.WriteLine($"In progress: {board.CountIn(TaskState.InProgress)}");
```

<div v-click>

```text
#1 Write the README, Todo, 1h
#2 Fix the login bug, InProgress, 3h
#3 Prepare the demo, Todo, 2h
In progress: 1
```

</div>

---
layout: two-cols
---

# Interfaces

An interface lists members without writing their code. A type that implements it promises to have them. Both classes and structs can implement an interface.

```cs
public interface IShape
{
    string Name { get; }
    double Area();
}
```

`=>` is a short way to write a member whose body is a single expression. `Area() => side * side` means the same as `Area() { return side * side; }`.

::right::

<div class="pl-6 pt-22">

```cs
public class Circle(double r) : IShape
{
    public string Name => "circle";
    public double Area() => Math.PI * r * r;
}
```

```cs
public struct Square(double side) : IShape
{
    public string Name => "square";
    public double Area() => side * side;
}
```

</div>

<!--
`Circle(double r)` is a primary constructor: the parameter is available in the whole class. It is short for a constructor that stores `r` in a field.
Interface names start with `I` by convention.
These `=>` members are expression-bodied members, not lambdas. Lambdas also use `=>` and come in lab 6. `Century` in lab 1 was written the same way.
-->

---

# Using objects through an interface

```cs {1|3-8|9|all}
List<IShape> shapes = [new Circle(1), new Square(2), new Circle(0.5)];

double total = 0;
foreach (IShape shape in shapes)
{
    Console.WriteLine($"{shape.Name}: {shape.Area():F2}");
    total += shape.Area();
}
Console.WriteLine($"Total area: {total:F2}");
```

<div v-click>

```text
circle: 3.14
square: 4.00
circle: 0.79
Total area: 7.93
```

</div>

The loop does not know which shape it holds, and it never needs to check. Each object runs its own `Area`. In lab 4 we do the same with a base class.

<!--
Pre-written project, run it rather than typing.
If someone asks: a struct stored as an IShape is copied into an object on the heap (boxing).
-->

---

# The debugger

Put a breakpoint on the `Console.WriteLine` line in the struct and class example and start debugging.

In the Variables panel, open `p1` and `p2`, then `m1` and `m2`. The two points have different values. The two markers show the same object.

Then put a breakpoint inside the `TaskItem` constructor and press `F5` to stop there once for each task. `nextId` keeps growing, because it belongs to the type. The properties of `this` start empty every time, because each call builds a new object.

<!--
Use the pre-written ValueVsReference and TaskBoard projects.
-->

---
layout: center
---

# Assignment

The statement is on CV UPT. Upload your solution before the deadline shown there.

It uses the same ideas as today: an enum, properties that only the class can change, constructors, and a class that keeps a list of other objects.
