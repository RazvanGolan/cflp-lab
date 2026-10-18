---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#4d7c0f'
title: "Lab 10: Extension methods and LINQ"
info: |
  CFLP laboratory, week 10.
layout: cover
---

# Lab 10: Extension methods and LINQ

Queries over lists and files, and sequences that are made only when you read them

---

# Where we left off

In lab 5 we wrote `CountWhere` and `TotalOf`: loops that take a `Func` and run it on every item of a list.

.NET has dozens of methods like these, for any collection, in a part of the library called LINQ. They are extension methods, so we start by writing one of our own.

Then we use LINQ on songs read from a CSV file. At the end we see that a LINQ query runs only when you read its results. Test 2 has two LINQ one-liners.

---
layout: two-cols
---

# An extension method

```cs {1|3|5-7|all}
public static class StringExtensions
{
    public static int WordCount(this string text)
    {
        string[] words = text.Split(' ',
            StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
}
```

::right::

<div class="pl-6 pt-22">

`WordCount` is a static method in a static class. The `this` before its first parameter makes it an extension method for `string`.

We cannot change the `string` class, but now every string seems to have a `WordCount` method.

`RemoveEmptyEntries` skips the empty pieces that two spaces in a row would leave.

</div>

<!--
LIVE: the Extensions project.
A static class can only have static members, and nobody can create an object of it (lab 2 had static members).
If asked: C# 14 also has a newer syntax, extension blocks (`extension(string text) { ... }`). The `this` form still works everywhere and is what the documentation and most code use.
-->

---

# Calling it

```cs
string line = "to be or not to be";
Console.WriteLine(line.WordCount());
Console.WriteLine(StringExtensions.WordCount(line));

string title = "Smells Like Teen Spirit";
Console.WriteLine(title.Shorten(10));
```

<div v-click>

```text
6
6
Smells Lik...
```

</div>

The compiler turns `line.WordCount()` into the second call, so both lines do the same thing. The method sees only the public members of `string`.

`Shorten` takes one more parameter, which goes between the parentheses as usual. The file needs `using Extensions;`, the namespace of the static class, or the call does not compile.

<!--
Shorten is in the same StringExtensions class: it cuts the text to `length` characters and adds "...".
-->

---
layout: two-cols
---

# On any sequence

```cs
public static string Joined(
    this IEnumerable<string> items)
{
    return string.Join(", ", items);
}
```

```cs
List<string> bands = ["Queen", "Eagles", "Abba"];
string[] genres = ["rock", "pop", "jazz"];
Console.WriteLine(bands.Joined());
Console.WriteLine(genres.Joined());
```

<div v-click>

```text
Queen, Eagles, Abba
rock, pop, jazz
```

</div>

::right::

<div class="pl-6 pt-22">

`IEnumerable<T>` is an interface for anything a `foreach` can walk through. Lists, arrays, sets and dictionaries all implement it.

An extension method for `IEnumerable<string>` works on all of them, and on any class we write that implements the interface.

</div>

---

# LINQ is extension methods

This is how .NET declares `Where`, without its body:

```cs
public static IEnumerable<TSource> Where<TSource>(
    this IEnumerable<TSource> source, Func<TSource, bool> predicate)
```

It is an extension method for any `IEnumerable<T>`, and it takes a `Func` like our `CountWhere`.

| Lab 5, by hand | LINQ |
|---|---|
| `CountWhere(songs, IsLong)` | `songs.Count(IsLong)` |
| `TotalOf(songs, s => s.Seconds)` | `songs.Sum(s => s.Seconds)` |

The methods are in the `System.Linq` namespace, which a console project imports for you.

<!--
LINQ stands for Language Integrated Query.
`ImplicitUsings` in the .csproj adds `using System.Linq;` and a few others to every file.
-->

---

# Songs from a file

```text
title,artist,year,seconds,genre
Bohemian Rhapsody,Queen,1975,354,rock
Imagine,John Lennon,1971,183,pop
```

```cs {1|2|3|4|5|all}
string path = Path.Combine(AppContext.BaseDirectory, "songs.csv");
List<Song> songs = File.ReadAllLines(path)
    .Skip(1)
    .Select(Song.Parse)
    .ToList();
```

`File.ReadAllLines` returns every line as a `string[]`. `Skip(1)` leaves out the header. `Select` turns each line into a `Song`, using `Song.Parse`, which we pass without parentheses as in lab 5. `ToList` puts the songs in a `List<Song>`.

<!--
LIVE: the Charts project. songs.csv has 16 songs.
-->

---
layout: two-cols
---

# Parsing a line

```cs
public static Song Parse(string line)
{
    string[] parts = line.Split(',');
    return new Song(parts[0], parts[1],
        int.Parse(parts[2]),
        int.Parse(parts[3]), parts[4]);
}
```

```xml
<ItemGroup>
  <None Update="songs.csv"
        CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

::right::

<div class="pl-6 pt-22">

`Parse` is a static method of `Song`, so it needs no song to be called. It splits the line at each comma and converts the numbers.

The lines in the `.csproj` copy `songs.csv` next to the compiled program. `AppContext.BaseDirectory` is that folder, so the program finds the file wherever you start it from.

</div>

<!--
Without the copy, `File.ReadAllLines("songs.csv")` looks in the current folder, which depends on how the program was started (`dotnet run` from the repository root, or the IDE). FileNotFoundException is the usual symptom.
This split breaks for titles with a comma in them. Real CSV files quote such fields; a library like CsvHelper handles that.
-->

---
layout: two-cols
---

# `Where` and `Select`

```cs
IEnumerable<Song> beatles =
    songs.Where(s => s.Artist == "The Beatles");
foreach (Song song in beatles)
{
    Console.WriteLine(song);
}

IEnumerable<string> titles =
    songs.Select(s => s.Title);
Console.WriteLine(
    string.Join(", ", titles.Take(4)));
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
Hey Jude, The Beatles (1968, 7:11)
Yesterday, The Beatles (1965, 2:05)
Let It Be, The Beatles (1970, 4:03)

Bohemian Rhapsody, Imagine,
Hey Jude, Yesterday
```

</div>

`Where` keeps the items for which the lambda returns `true`.

`Select` turns each item into something else, here its title. The result has as many items as the list.

`Take(4)` keeps the first four.

</div>

---

# Sorting

```cs {2|3|4|5|all}
List<string> longest = songs
    .OrderByDescending(s => s.Seconds)
    .Take(3)
    .Select(s => $"{s.Title} ({s.Seconds} s)")
    .ToList();
```

<div v-click>

```text
So What (562 s)
Hey Jude (431 s)
Hotel California (391 s)
```

</div>

Each method returns a new sequence, and the next one works on it. The list `songs` does not change.

`OrderBy` sorts from small to large, `OrderByDescending` the other way. To break ties, add `ThenBy` after it. `Skip(n)` leaves out the first `n` items.

---
layout: two-cols
---

# One value

```cs
songs.Count(s => s.Genre == "rock")
songs.Sum(s => s.Seconds)
songs.Average(s => s.Seconds)
songs.Min(s => s.Year)
songs.MaxBy(s => s.Seconds)
songs.First(s => s.Year > 2000)
songs.FirstOrDefault(s => s.Year > 2020)
songs.Any(s => s.Genre == "jazz")
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
7
4775
298.4375
1959
So What, Miles Davis (1959, 9:22)
Clocks, Coldplay (2002, 5:07)
null
True
```

</div>

`Min` gives the smallest year. `MaxBy` gives the song with the most seconds.

`First` throws an `InvalidOperationException` when nothing matches. `FirstOrDefault` returns `null` instead.

</div>

<!--
In the code, the FirstOrDefault line prints `?.Title ?? "none"`, because Console.WriteLine(null) prints an empty line.
All is the other half of Any: true when every item passes.
-->

---

# Groups

```cs
foreach (IGrouping<string, Song> genre in songs.GroupBy(s => s.Genre))
{
    double average = genre.Average(s => s.Seconds);
    Console.WriteLine($"{genre.Key}: {genre.Count()}, average {average:F0} s");
}
```

<div v-click>

```text
rock: 7, average 326 s
pop: 5, average 206 s
hip hop: 1, average 326 s
dance: 1, average 248 s
jazz: 2, average 443 s
```

</div>

`GroupBy` puts the songs with the same genre together. Each group has a `Key`, the genre, and is itself a sequence of songs, so `Count` and `Average` work on it.

The groups come in the order in which their first song appears in the file.

---

# A longer query

```cs {2|3|4|5|all}
IEnumerable<string> oldestFirst = songs
    .Where(s => s.Genre == "rock")
    .OrderBy(s => s.Year)
    .ThenBy(s => s.Title)
    .Select(s => $"{s.Year} {s.Title}");
```

<div v-click>

```text
1968 Hey Jude
1970 Let It Be
1975 Bohemian Rhapsody
1976 Hotel California
1991 Smells Like Teen Spirit
1995 Wonderwall
2002 Clocks
```

</div>

Put each step on its own line and read the query from top to bottom.

---

# Query syntax

The same query can be written like this:

```cs
IEnumerable<string> sameQuery =
    from s in songs
    where s.Genre == "rock"
    orderby s.Year, s.Title
    select $"{s.Year} {s.Title}";
```

The compiler turns it into the method calls from the previous slide. You will see this form in older code and in the documentation.

We use the method syntax, because many methods, such as `Count`, `First` or `Take`, have no keyword in query syntax.

<!--
The Charts project checks that both give the same result with SequenceEqual, which compares two sequences item by item.
-->

---
layout: two-cols
---

# `yield return`

```cs
IEnumerable<int> Countdown(int from)
{
    for (int i = from; i > 0; i--)
    {
        Console.WriteLine($"  making {i}");
        yield return i;
    }
    Console.WriteLine("  done");
}
```

```cs
foreach (int number in Countdown(3))
{
    Console.WriteLine(number);
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
  making 3
3
  making 2
2
  making 1
1
  done
```

</div>

`yield return` hands one value to the `foreach` and pauses the method. When the `foreach` asks for the next value, the method continues from there.

A method like this is an iterator. It never builds a list.

</div>

<!--
LIVE: the Iterators project. Ask before revealing: does "making 3, making 2, making 1" print first, or does it alternate?
-->

---
layout: two-cols
---

# An endless sequence

```cs
IEnumerable<int> Squares()
{
    int n = 1;
    while (true)
    {
        yield return n * n;
        n++;
    }
}
```

::right::

<div class="pl-6 pt-22">

```cs
foreach (int square in Squares().Take(5))
{
    Console.Write($"{square} ");
}
```

<div v-click>

```text
1 4 9 16 25
```

</div>

The loop in `Squares` never ends, and the program still finishes. `Take(5)` asks for five values and then stops asking.

</div>

---
layout: two-cols
---

# A query runs later

```cs
List<int> scores = [4, 9, 6];
IEnumerable<int> passed = scores.Where(n =>
{
    Console.WriteLine($"  checking {n}");
    return n >= 5;
});
Console.WriteLine("Query written");
scores.Add(8);

foreach (int score in passed)
{
    Console.WriteLine(score);
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
Query written
  checking 4
  checking 9
9
  checking 6
6
  checking 8
8
```

</div>

`Where` is an iterator too. Writing the query checks nothing yet. The lambda runs during the `foreach`, one item at a time, so it also sees the `8` added later.

</div>

<!--
Ask before revealing: what prints first, and does 8 appear?
-->

---

# Running it once

```cs
List<int> marks = [4, 9, 6];
IEnumerable<int> query = marks.Where(n => n >= 5);
List<int> saved = query.ToList();
marks.Add(8);
Console.WriteLine($"query: {query.Count()}, saved: {saved.Count}");
```

<div v-click>

```text
query: 3, saved: 2
```

</div>

Every `foreach`, `Count()` or `ToList()` on a query runs it again, from the start. `ToList()` runs it once and keeps the results in a list that no longer changes.

Call `ToList()` when you need the results more than once, or when the data may change before you read them.

<!--
The cost matters when the source is slow: a query over File.ReadLines reads the file again each time.
-->

---

# Summary

| | Example |
|---|---|
| extension method | `public static int WordCount(this string text)` |
| filter, transform | `songs.Where(s => s.Year > 2000).Select(s => s.Title)` |
| sort | `songs.OrderBy(s => s.Year).ThenBy(s => s.Title)` |
| one value | `Count`, `Sum`, `Average`, `Min`, `MaxBy`, `First`, `Any` |
| groups | `songs.GroupBy(s => s.Genre)` |
| iterator | `yield return value;` |

A query runs each time you read it. `ToList()` keeps the results.

---

# The debugger

Put a breakpoint on `return n >= 5;` inside the lambda in the Iterators project and start debugging.

The debugger does not stop at the line where the query is written. It stops for the first time after "Query written" is printed, while the `foreach` runs.

The Call Stack panel shows the lambda on top, then .NET's own code for `Where` (often shown as external code), then the main program at the `foreach` line.

---
layout: center
---

# Assignments

There are two statements on CV UPT this week. Upload each one before the deadline shown there.

The first is a games store, and the second works with a CSV file of cars. Both use what we did today.
