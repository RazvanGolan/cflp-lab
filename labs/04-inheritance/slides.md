---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#be123c'
title: "Lab 4: Inheritance"
info: |
  CFLP laboratory, week 4.
layout: cover
---

# Lab 4: Inheritance

Base classes, abstract members, overriding, and one list that holds different types

---

# Where we left off

In lab 2, `Circle` and `Square` both implemented `IShape`. The interface listed the members, and each type wrote its own code for them.

Today the types also share code. A base class holds what they have in common, and each derived class adds or changes only what is different.

The example for today is a streaming catalog with movies and series.

---
layout: two-cols
---

# The same code, twice

```cs
public class Movie
{
    public string Title { get; }
    public int Year { get; }
    public int Length { get; }

    // a constructor that sets all three
}
```

`Title`, `Year` and the code that sets them are written twice. A fix in one class is easy to forget in the other.

::right::

<div class="pl-6 pt-22">

```cs
public class Series
{
    public string Title { get; }
    public int Year { get; }
    public int Episodes { get; }
    public int EpisodeLength { get; }

    // a constructor that sets all four
}
```

</div>

---
layout: two-cols
---

# A base class

```cs
public class Media
{
    public string Title { get; }
    public int Year { get; }

    protected Media(string title, int year)
    {
        Title = title;
        Year = year;
    }
}
```

`Media` holds what every item in the catalog has. `: Media` means that `Movie` inherits from it, so a movie has `Title` and `Year` without writing them again.

::right::

<div class="pl-6 pt-22">

```cs {1|5-9|all}
public class Movie : Media
{
    public int Length { get; }

    public Movie(string title, int year,
                 int length)
        : base(title, year)
    {
        Length = length;
    }
}
```

`: base(title, year)` runs the `Media` constructor first, then the body of the `Movie` constructor.

</div>

<!--
LIVE: start the Streaming project with Media.cs and Movie.cs. Series comes after, the same way.
-->

---

# `protected` and the base constructor

| Access | Who can use the member |
|---|---|
| `public` | any code |
| `protected` | the class and the classes derived from it |
| `private` | only the class itself |

The `Media` constructor is `protected`, so only a derived class can call it.

A derived class has to call a base constructor. Without `: base(title, year)`:

```text
error CS7036: There is no argument given that corresponds to the required
parameter 'title' of 'Media.Media(string, int)'
```

---

# A movie is a media item

A variable of the base type can hold an object of any derived type:

```cs
Media item = new Movie("Inception", 2010, 148);
Console.WriteLine(item.Title);
Console.WriteLine(item.Length);
```

<v-click>

```text
error CS1061: 'Media' does not contain a definition for 'Length'
```

The compiler only knows that `item` is a `Media`, so it lets you use only what `Media` has. A `Media` has no length.

</v-click>

<v-click>

How long is a media item, then? Every movie and every series has a duration, but each one calculates it differently.

</v-click>

---
layout: two-cols
---

# An abstract member

```cs {1|13|all}
public abstract class Media
{
    public string Title { get; }
    public int Year { get; }

    protected Media(string title, int year)
    {
        Title = title;
        Year = year;
    }

    public abstract int Minutes { get; }
}
```

An abstract member has no code. Each derived class must write it, with `override`.

::right::

<div class="pl-6 pt-22">

```cs
public class Movie : Media
{
    // ... as before
    public override int Minutes => Length;
}
```

```cs
public class Series : Media
{
    // ... as before
    public override int Minutes
        => Episodes * EpisodeLength;
}
```

Now `item.Minutes` works for any `Media`, and each object runs its own version.

</div>

---

# The rules of `abstract`

A class with an abstract member must be `abstract` itself. You cannot create an object of an abstract class:

```cs
Media item = new Media("Inception", 2010);
```

```text
error CS0144: Cannot create an instance of the abstract type or interface 'Media'
```

A derived class that does not write the abstract member does not compile:

```text
error CS0534: 'Podcast' does not implement inherited abstract member 'Media.Minutes.get'
```

That is what we want here. A media item is always a movie or a series, and every kind of media has a duration.

---

# `ToString` is virtual

Every class inherits from `object`, even when you do not write it. That is where `ToString` comes from, and why we wrote `override` in lab 1.

`ToString` is `virtual` in `object`: it has code, and a derived class may replace it.

```cs
public abstract class Media
{
    // ... as before

    public override string ToString()
    {
        return $"{Title} ({Year}), {Minutes} min";
    }
}
```

`Movie` and `Series` inherit this `ToString`. It calls `Minutes`, which runs the version of whichever object it is.

---

# Calling the base version

A series also shows its number of episodes. Its override builds on the one from `Media`:

```cs
public class Series : Media
{
    // ... as before

    public override string ToString()
    {
        return $"{base.ToString()}, {Episodes} episodes";
    }
}
```

`base.ToString()` runs the `Media` version.

<div v-click>

```text
Chernobyl (2019), 325 min, 5 episodes
```

</div>

---

# Forgetting `override`

```cs
public class Series : Media
{
    public string ToString()
    {
        return "a series";
    }
}
```

```text
warning CS0114: 'Series.ToString()' hides inherited member 'Media.ToString()'.
To make the current member override that implementation, add the override keyword.
Otherwise add the new keyword.
```

It is only a warning, so the program runs. `Console.WriteLine(series)` still prints the `Media` version, because the new method does not replace the virtual one. Read the warnings.

<!--
LIVE: remove `override` from Series.ToString and run the program to show that nothing changed in the output.
-->

---

# `virtual`, `abstract` and `override`

| Keyword | Written in | Meaning |
|---|---|---|
| `virtual` | the base class | the member has code, and derived classes may replace it |
| `abstract` | the base class | the member has no code, and derived classes must write it |
| `override` | the derived class | this replaces the base version |

A method that is neither `virtual` nor `abstract` cannot be overridden.

<!--
`sealed override` stops further overriding, and a `sealed` class cannot be inherited from. Mention only if someone asks.
-->

---
layout: two-cols
---

# A list of the base type

```cs {1-6|8-13|all}
List<Media> catalog =
[
    new Movie("Inception", 2010, 148),
    new Series("Chernobyl", 2019, 5, 65),
    new Movie("Spirited Away", 2001, 125),
];

int total = 0;
foreach (Media item in catalog)
{
    Console.WriteLine(item);
    total += item.Minutes;
}
Console.WriteLine($"Total: {total} min");
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
Inception (2010), 148 min
Chernobyl (2019), 325 min, 5 episodes
Spirited Away (2001), 125 min
Total: 598 min
```

</div>

The list holds movies and series together. Each `item` prints with its own `ToString` and counts with its own `Minutes`.

</div>

---
layout: two-cols
---

# Casts and `is`

A cast asks for the derived type back. It is checked while the program runs:

```cs
Media item = new Series("Chernobyl", 2019, 5, 65);
Movie movie = (Movie)item;
```

```text
Unhandled exception. System.InvalidCastException:
Unable to cast object of type 'Series' to type 'Movie'.
```

`as` gives `null` instead of throwing:

```cs
Movie? movie = item as Movie;
```

::right::

<div class="pl-6 pt-22">

`is` checks the type, and gives you a variable of that type when it matches:

```cs
if (item is Movie movie)
{
    Console.WriteLine(movie.Length);
}
```

A `switch` expression can match on the type too:

```cs
string kind = item switch
{
    Movie m => $"{m.Length} min",
    Series s => $"{s.Episodes} episodes",
    _ => "something else"
};
```

</div>

<!--
For the Chernobyl series, `kind` is "5 episodes".
`is` and `switch` never throw: when the type does not match, the condition is false or the next case is tried.
Prefer them to a plain cast whenever the type might not match.
In the Streaming project the exception says 'Streaming.Series' and 'Streaming.Movie', because the classes are in a namespace.
-->

---
layout: two-cols
---

# Without casting

Pattern matching works here, but it does not scale:

```cs
foreach (Media item in catalog)
{
    if (item is Movie movie)
    {
        total += movie.Length;
    }
    else if (item is Series series)
    {
        total += series.Episodes
               * series.EpisodeLength;
    }
}
```

::right::

<div class="pl-6 pt-22">

Let each object answer for itself:

```cs
foreach (Media item in catalog)
{
    total += item.Minutes;
}
```

Add a `Podcast` class later and this loop does not change. The version on the left needs another `else if`, in every loop that does something similar.

When you are tempted to check the type, add an abstract or virtual member to the base class instead.

</div>

<!--
The tests ask for exactly this: a list of the base type, used without casting.
-->

---
layout: two-cols
---

# A catalog class

```cs
public class Catalog
{
    private readonly List<Media> items = [];

    public void Add(Media item)
    {
        items.Add(item);
    }

    public int TotalMinutes()
    {
        int total = 0;
        foreach (Media item in items)
        {
            total += item.Minutes;
        }
        return total;
    }
}
```

::right::

<div class="pl-6 pt-22">

```cs
public Media? Longest()
{
    Media? longest = null;
    foreach (Media item in items)
    {
        if (longest == null
            || item.Minutes > longest.Minutes)
        {
            longest = item;
        }
    }
    return longest;
}
```

`Catalog` works with `Media` only. It never needs to know that movies and series exist.

</div>

---

# A base class and an interface

Some items can be downloaded. That is not true for every `Media`, so it is an interface:

```cs
public interface IDownloadable
{
    int SizeInMb { get; }
}

public class Movie : Media, IDownloadable
{
    // ... as before
    public int SizeInMb => Length * 30;
}
```

A class has at most one base class, and any number of interfaces. The base class comes first:

```text
error CS1722: Base class 'Media' must come before any interfaces
```

<!--
Compare with lab 2: an interface has no constructor and no fields, and usually no code. An abstract class can have all of these, but a class can inherit from only one.
-->

---

# The debugger

Put a breakpoint on `total += item.Minutes;` in the loop over the catalog and start debugging.

In the Variables panel, `item` shows the type of the object it holds: `Movie` the first time, `Series` the second.

Press `F11` on that line. The debugger steps into `Movie.Minutes` or `Series.Minutes`, depending on the object. That choice happens while the program runs, not when it is compiled.

---
layout: center
---

# Assignment

The statement is on CV UPT. Upload your solution before the deadline shown there.

It uses the same ideas as today: an abstract base class, derived classes that override its members and `ToString`, an interface, and a list of the base type used without casting.
