---
theme: ../../theme
routerMode: hash
title: "Lab 1: Introduction to C#"
info: |
  CFLP laboratory, week 1.
layout: cover
---

# Lab 1: Introduction to C#

Your first programs, console input and output, and your first class

---

# How the lab works

Each session has two parts.

- The lab itself, about an hour and a half. We write code together, and you get an assignment to upload on Moodle.
- An optional deep dive into how C# and .NET work underneath. It is not graded, and you can leave before it starts.

There are two tests during the semester, on the topics from the labs. The dates will be on Moodle.

Slides and code for every lab are published after the lab at

```text
https://razvangolan.github.io/cflp-lab/
```

---

# Getting the code

Every example from the slides is a small project in the repository.

```bash
git clone https://github.com/RazvanGolan/cflp-lab.git
cd cflp-lab
dotnet run --project labs/01-intro/code/Hello
```

<CodeLink path="labs/01-intro/code" />

<!--
Show the landing page quickly, then the code folder on GitHub.
-->

---

# Your first program

```bash
dotnet new console -n Hello
cd Hello
dotnet run
```

<div v-click>

```text
Hello, World!
```

</div>

<!--
LIVE in the terminal. Type the three commands, then `ls` to show the files.
Open the folder in the editor for the next slide.
-->

---

# What `dotnet new` created

<<< @/code/Hello/Hello.csproj xml

<v-clicks>

- `TargetFramework` says which version of .NET the program runs on.
- `ImplicitUsings` imports common namespaces such as `System`, so you can write `Console` instead of `System.Console`.
- `Nullable` makes the compiler warn you when a value might be `null`.

</v-clicks>

---
layout: two-cols
---

# Top-level statements

<<< @/code/Hello/Program.cs cs

Since C# 9 you can write statements directly in one file. The compiler generates the class and the `Main` method for you.

Only one file in a project can do this.

::right::

<div class="pl-6 pt-22">

<<< @/code/ClassicMain/Program.cs cs

You will still see this form in older code and in documentation.

</div>

---

# Writing to the console

<<< @/code/ConsoleIO/Program.cs#output cs {1-3|5|6|7|all}

<div v-click>

```text
Name: Ana
Ana is in year 2
Average: 9.46
```

</div>

<!--
`{average:F2}` rounds to two decimals when printing; the variable itself keeps 9.456.
`Console.Write` does not add a newline, `Console.WriteLine` does.
-->

---

# Reading input

<<< @/code/ConsoleIO/Program.cs#parse cs {2|3|4|all}

`ReadLine` returns `string?`: a string, or `null` when there is no more input. `?? ""` replaces `null` with an empty string.

<div v-click>

What happens if the user types `abc`?

```text
How old are you? abc
Unhandled exception. System.FormatException: The input string 'abc' was not in a correct format.
```

</div>

<!--
LIVE: run it, type abc, let it crash. Ask the class before revealing.
-->

---

# Parsing without crashing

<<< @/code/ConsoleIO/Program.cs#tryparse cs {3|3-6|all}

`int.TryParse` returns `false` instead of throwing. The number comes back through the `out` parameter, which we cover in lab 3.

```text
How many siblings do you have? two
That is not a whole number. Try again: 2
You have 2 sibling(s)
```

---

# Watch the decimal separator

`double.Parse` reads numbers the way your operating system's language settings write them.

```text
How many days? 1
Day 1: 21.5
Min: 215, max: 215, average: 215,0
```

On a machine set to Romanian, the decimal separator is a comma. `21.5` is read as `215` and nothing warns you.

<v-click>

To always use a dot, pass the invariant culture:

```cs
double t = double.Parse(text, CultureInfo.InvariantCulture);
```

</v-click>

<!--
Check the lab machines' locale before the lab (`locale` in a terminal).
`CultureInfo` needs `using System.Globalization;`.
-->

---

# Arrays and loops

<<< @/code/Temperatures/Program.cs#read cs {2|4|5-9|all}

`new double[n]` creates an array of `n` elements, all set to `0`. Indexes go from `0` to `n - 1`.

<!--
LIVE: write this part from an empty Program.cs.
-->

---

# Minimum, maximum, average

<<< @/code/Temperatures/Program.cs#stats cs {1-3|5-10|12|all}

<div v-click>

```text
How many days? 3
Day 1: 21.5
Day 2: 18
Day 3: 24
Min: 18, max: 24, average: 21.2
```

</div>

<div v-click>

What happens if the user enters `0` days?

</div>

<!--
LIVE, continuing the previous slide.
Answer to the question: temperatures[0] throws IndexOutOfRangeException, because the array is empty.
-->

---

# Your first class

<<< @/code/Library/Book.cs#properties cs

<<< @/code/Library/Book.cs#constructor cs

A property with only `get` can be set in the constructor and never changed afterwards. That makes it read-only.

<!--
LIVE: create Book.cs next to Program.cs. Write the properties, then the constructor.
Try `book.Year = 2000;` in Program.cs to show the compiler error.
-->

---

# A computed property and `ToString`

<<< @/code/Library/Book.cs#computed cs

`Century` has no stored value. It is calculated every time you read it. Dividing two `int` values gives an `int`: `(1869 - 1) / 100` is `18`, so the century is `19`.

<<< @/code/Library/Book.cs#tostring cs

<v-click>

Without the override, `Console.WriteLine(book)` prints only the type name:

```text
Library.Book
```

</v-click>

<!--
LIVE: first print a book without ToString to show `Library.Book`, then add the override.
-->

---

# Using the class

<<< @/code/Library/Program.cs#books cs {1-6|8-11|all}

<div v-click>

```text
War and Peace by Leo Tolstoy, 1869 (century 19)
The Hobbit by J. R. R. Tolkien, 1937 (century 20)
Dune by Frank Herbert, 1965 (century 20)
```

</div>

<!--
`Console.WriteLine(book)` calls `ToString()` for us.
The [ ... ] syntax is a collection expression (C# 12). `new Book[3]` plus assignments does the same.
-->

---

# Finding the oldest book

<<< @/code/Library/Program.cs#oldest cs {1|2-8|10|all}

<div v-click>

```text
Oldest: War and Peace
```

</div>

---

# The debugger

Put a breakpoint on `if (book.Year < oldest.Year)` and run the program in debug mode. In VS Code:

| Key | Action |
|---|---|
| `F9` | add or remove a breakpoint |
| `F5` | start debugging, or continue to the next breakpoint |
| `F10` | step over the current line |
| `F11` | step into the method called on this line |

Watch `book` and `oldest` in the Variables panel as the loop runs. Other editors have the same commands under different keys.

<!--
Use the pre-written Library project. Step into `Century` from the ToString call to show F11.
-->

---
layout: center
---

# Assignment

The statement is on Moodle. Upload your solution before the deadline shown there.

It uses the same ideas as today: reading numbers, loops, and a class with read-only properties, a computed property and `ToString`.
