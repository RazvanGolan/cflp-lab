---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#b45309'
title: "Lab 5: Generics and delegates"
info: |
  CFLP laboratory, week 5.
layout: cover
---

# Lab 5: Generics and delegates

Code that works with any type, and methods passed to other methods

---

# Where we left off

Since lab 2 we have used `List<string>`, `List<TaskItem>` and `List<Media>`. It is the same `List` class each time, with a different type between the angle brackets. A class like that is generic.

Today we look at more generic collections and write a generic class and a generic method of our own.

In the second half we pass a method to another method, as an argument. Both tests ask for that.

---
layout: two-cols
---

# `List<T>`

```cs {1|2|3|4|all}
List<int> scores = [7, 3, 9, 3];
scores.Add(5);
scores.Remove(3);
scores.RemoveAt(0);
Console.WriteLine(string.Join(", ", scores));
Console.WriteLine(scores.Contains(9));
Console.WriteLine(scores.IndexOf(5));
```

<div v-click>

```text
9, 3, 5
True
2
```

</div>

::right::

<div class="pl-6 pt-22">

`Remove(3)` removes the first `3` it finds. `RemoveAt(0)` removes the element at index `0`, whatever it is.

Removing from a list inside a `foreach` over the same list throws:

```text
InvalidOperationException: Collection was
modified; enumeration operation may not execute.
```

Use a `for` loop that goes backwards, or remember what to remove and do it after the loop.

</div>

<!--
`string.Join(", ", scores)` glues the elements into one string with ", " between them.
After Add: 7, 3, 9, 3, 5. After Remove(3): 7, 9, 3, 5. After RemoveAt(0): 9, 3, 5.
-->

---
layout: two-cols
---

# A dictionary

```cs {1-2|4-14|16-19|all}
string text = "to be or not to be";
Dictionary<string, int> counts = [];

foreach (string word in text.Split(' '))
{
    if (counts.TryGetValue(word, out int count))
    {
        counts[word] = count + 1;
    }
    else
    {
        counts[word] = 1;
    }
}

foreach (KeyValuePair<string, int> pair in counts)
{
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
to: 2
be: 2
or: 1
not: 1
```

</div>

A `Dictionary<TKey, TValue>` finds a value by its key. Here the key is a word and the value is how many times it appears.

`TryGetValue` follows the `TryParse` pattern from lab 3. `counts["bird"]` would throw a `KeyNotFoundException`, because that key is not there.

`counts[word] = 1` adds the key, or replaces its value if it is already there.

</div>

---

# Other collections

| Type | Use it for |
|---|---|
| `List<T>` | items in order, read by index |
| `Dictionary<TKey, TValue>` | finding a value by its key |
| `HashSet<T>` | items without duplicates, with a fast `Contains` |
| `Queue<T>` | first in, first out: `Enqueue` and `Dequeue` |
| `Stack<T>` | last in, first out: `Push`, `Pop` and `Peek` |

Taking an item from an empty queue or stack throws an `InvalidOperationException`.

<!--
They are all in System.Collections.Generic, which a console project imports for you (ImplicitUsings).
-->

---
layout: two-cols
---

# How a hash set works

```cs
HashSet<string> fruits = ["apple", "plum", "kiwi"];
Console.WriteLine(fruits.Add("pear"));
Console.WriteLine(fruits.Add("plum"));
Console.WriteLine(fruits.Contains("kiwi"));
Console.WriteLine(fruits.Count);
```

<div v-click>

```text
True
False
True
4
```

</div>

`Add` returns `false` when the item is already there. A set has no index, and you cannot count on its order.

::right::

<div class="pl-6 pt-22">

```text
"pear".GetHashCode() % 4  →  bucket 2

bucket 0
bucket 1   "apple"
bucket 2   "plum"  "pear"
bucket 3   "kiwi"
```

`GetHashCode` turns an item into a number, and the number picks a bucket. `Contains` computes the same number and looks in that bucket only, so it stays fast with a million items. `List<T>.Contains` checks every element.

Items that land in the same bucket are compared with `Equals`.

</div>

<!--
The numbers are made up. String hash codes change every time the program starts, so never save them.
Two different items in one bucket are a collision. A good hash code spreads items evenly, so each bucket holds one or two.
When the set fills up, it creates more buckets and moves every item to its new bucket. That is why Add is fast on average, with an occasional slower one.
-->

---

# Other hash types

| Type | Keeps |
|---|---|
| `HashSet<T>` | unique items |
| `Dictionary<TKey, TValue>` | hashed keys, each with a value |
| `ConcurrentDictionary<K, V>` | the same, safe for several threads |
| `FrozenSet<T>`, `FrozenDictionary<K, V>` | built once, never changed, faster |

`SortedSet<T>` and `SortedDictionary<TKey, TValue>` use no hashing. They keep the items sorted in a tree.

Equal items must give the same hash code. `string` and `int` already do. Your own class compares references unless you override `Equals` and `GetHashCode` together.

<!--
`Hashtable` is the old version of Dictionary from before generics: keys and values are `object`. You may see it in old code; don't use it.
Example: two `new Song("Imagine", "John Lennon", 183)` objects are two items in a HashSet<Song>, because Song does not override Equals and GetHashCode.
Concurrent types are in System.Collections.Concurrent, the frozen ones in System.Collections.Frozen.
If someone asks about SHA-256 or password hashing: those are hash functions too, built to be hard to reverse. GetHashCode is built to be fast, and is not safe for passwords.
-->

---
layout: two-cols
---

# A generic class

```cs {1|3|6-13|all}
public class History<T>
{
    private readonly List<T> items = [];
    private readonly int capacity;
    // constructor, Count and Print
    public void Add(T item)
    {
        if (items.Count == capacity)
        {
            items.RemoveAt(0);
        }
        items.Add(item);
    }

    public T Last()
    {
        return items[items.Count - 1];
    }
}
```

::right::

<div class="pl-6 pt-22">

A history keeps the last few items you added, like the recent searches in an app. When it is full, the oldest one goes.

`T` is a type parameter. Inside the class it stands for a type that whoever uses the class picks later.

The class never needs to know what `T` is. It only stores items of that type and gives them back.

</div>

<!--
LIVE: write History<T> in the Generics project. The constructor is `public History(int capacity) { this.capacity = capacity; }`.
Ask: what does Last() do on an empty history? (ArgumentOutOfRangeException from the list.) How would you give a clearer error? A throw, as in lab 3.
-->

---
layout: two-cols
---

# One class, any type

```cs
History<string> searches = new History<string>(3);
searches.Add("list");
searches.Add("dictionary");
searches.Add("lambda");
searches.Add("generics");
searches.Print();

History<int> pages = new History<int>(2);
pages.Add(10);
pages.Add(42);
Console.WriteLine(pages.Last());
```

<div v-click>

```text
dictionary
lambda
generics
42
```

</div>

::right::

<div class="pl-6 pt-22">

In `History<int>`, every `T` becomes `int`. The compiler checks it:

```cs
pages.Add("ten");
```

```text
error CS1503: Argument 1: cannot convert
from 'string' to 'int'
```

Without generics, a history would keep `object`. Every read would need a cast, and a wrong type would only show up while the program runs.

</div>

<!--
In the Generics project the last line prints "Last page: 42".
"list" is gone because the capacity is 3.
-->

---
layout: two-cols
---

# A generic method

```cs {1|1-2|all}
T Max<T>(params T[] values)
    where T : IComparable<T>
{
    T max = values[0];
    foreach (T v in values)
    {
        if (v.CompareTo(max) > 0)
        {
            max = v;
        }
    }
    return max;
}
```

::right::

<div class="pl-6 pt-22">

```cs
Console.WriteLine(Max(3, 9, 4));
Console.WriteLine(Max("pear", "apple", "plum"));
```

```text
9
plum
```

Without the `where` line, `v > max` does not compile:

```text
error CS0019: Operator '>' cannot be applied
to operands of type 'T' and 'T'
```

`T` could be any type. The `where` line allows only types that can be compared, and gives `T` a `CompareTo` method.

</div>

<!--
`Max(3, 9, 4)` is `Max<int>(3, 9, 4)`. The compiler picks T from the arguments.
This is the Max from lab 3, made generic. The check for an empty array is left out to keep the slide short.
`a.CompareTo(b)` is negative when a comes first, zero when they are equal, positive when b comes first.
int, double and string all implement IComparable<T>. Strings compare alphabetically, so "plum" is the largest.
-->

---
layout: two-cols
---

# The same loop, twice

The example for the rest of the lab is a playlist of `Song` objects, with `Title`, `Artist` and `Seconds`.

```cs
int CountLong(List<Song> songs)
{
    int count = 0;
    foreach (Song song in songs)
    {
        if (song.Seconds > 300)
        {
            count++;
        }
    }
    return count;
}
```

::right::

<div class="pl-6 pt-31">

```cs
int CountBy(List<Song> songs, string artist)
{
    int count = 0;
    foreach (Song song in songs)
    {
        if (song.Artist == artist)
        {
            count++;
        }
    }
    return count;
}
```

Only the condition in the `if` changes. If we could pass the condition as an argument, one method would be enough.

</div>

---
layout: two-cols
---

# A delegate type

```cs
delegate bool SongTest(Song song);
```

```cs {1|3-14|all}
bool IsLong(Song song) => song.Seconds > 300;

int CountSongs(List<Song> playlist, SongTest test)
{
    int count = 0;
    foreach (Song song in playlist)
    {
        if (test(song))
        {
            count++;
        }
    }
    return count;
}
```

::right::

<div class="pl-6 pt-22">

`SongTest` is a type. Its values are methods that take a `Song` and return a `bool`.

```cs
Console.WriteLine(CountSongs(songs, IsLong));
```

<div v-click>

```text
3
```

</div>

`IsLong` without parentheses passes the method itself. `test(song)` calls whatever method was passed.

A method with a different shape does not fit:

```text
error CS0407: 'void Shout(string)' has the
wrong return type
```

</div>

<!--
LIVE: in the Playlist project. The delegate declaration goes at the very end of Program.cs, after the local functions, because a type has to come after the top-level code.
CS0407 comes from passing `void Shout(string s)` where a SongTest is expected.
-->

---

# `Func` and `Action`

.NET already has generic delegate types, so you rarely declare your own:

| Type | Takes | Returns |
|---|---|---|
| `Func<Song, bool>` | a `Song` | `bool` |
| `Func<int, int, int>` | two `int` values | `int` |
| `Action<string>` | a `string` | nothing |

In a `Func`, the last type is what the method returns. `Action` is for methods that return `void`.

`Func<Song, bool>` describes the same methods as our `SongTest`.

<!--
`Func<int>` takes nothing and returns an int. `Action` takes nothing and returns nothing.
`Predicate<T>` is an older type with the same shape as `Func<T, bool>`. Some List methods take it. A lambda fits both, but a `Func` variable cannot be passed where a `Predicate` is expected.
-->

---

# Lambdas

A lambda is a method without a name, written where you need it:

```cs
Func<Song, bool> byBeatles = s => s.Artist == "The Beatles";
```

The parameter is on the left of `=>`, the result on the right. The compiler knows that `s` is a `Song` from the `Func<Song, bool>`.

```cs
Func<Song, bool> isLong = (Song s) => s.Seconds > 300;
Func<Song, bool> isShort = s => { return s.Seconds < 180; };
Func<int, int, int> add = (a, b) => a + b;
Action<string> title = text => Console.WriteLine($"== {text} ==");
```

With more than one parameter, the parentheses are required. A body in braces needs a `return`, unless the lambda returns nothing.

<!--
Lab 2's expression-bodied members (`Area() => side * side`) use the same arrow, but they are named members of a class. A lambda is a value that you can store or pass.
-->

---
layout: two-cols
---

# Taking a `Func`

```cs
int CountWhere<T>(List<T> items,
                  Func<T, bool> test)
{
    int count = 0;
    foreach (T item in items)
    {
        if (test(item))
        {
            count++;
        }
    }
    return count;
}
```

```cs
CountWhere(songs, byBeatles);
CountWhere(songs, IsLong);
```

<div v-click>

```text
2
3
```

</div>

::right::

<div class="pl-6 pt-22">

```cs
int TotalOf<T>(List<T> items,
               Func<T, int> value)
{
    int total = 0;
    foreach (T item in items)
    {
        total += value(item);
    }
    return total;
}
```

```cs
TotalOf(songs, s => s.Seconds);
```

<div v-click>

```text
1484
```

</div>

These work for a list of anything, with any condition or any value. A named method fits a `Func` too, if it has the right shape.

</div>

<!--
LIVE: write CountWhere first, then TotalOf.
.NET already has both as LINQ methods (Count and Sum), which we use in week 10. We write them by hand to see how they work, and the assignment and the tests ask for more methods like these.
-->

---
layout: two-cols
---

# A lambda can use local variables

```cs
int limit = 200;
Console.WriteLine(
    CountWhere(songs, s => s.Seconds < limit));
```

<div v-click>

```text
2
```

</div>

The lambda uses `limit`, a variable from the code around it. It reads the variable each time it runs.

::right::

<div class="pl-6 pt-22">

A method can also build a lambda and return it:

```cs
Func<Song, bool> LongerThan(int seconds)
{
    return song => song.Seconds > seconds;
}
```

```cs
Func<Song, bool> overSix = LongerThan(360);
Console.WriteLine(CountWhere(songs, overSix));
```

<div v-click>

```text
2
```

</div>

The lambda still knows `seconds` after `LongerThan` has returned.

</div>

<!--
The lambda captures the variable, not its value. Change `limit` after creating the lambda and before calling it, and the lambda sees the new value.
-->

---
layout: two-cols
---

# `Sort` and `RemoveAll`

```cs
songs.Sort((a, b) =>
    a.Seconds.CompareTo(b.Seconds));
foreach (Song song in songs)
{
    Console.WriteLine(song);
}

int removed =
    songs.RemoveAll(s => s.Seconds > 400);
Console.WriteLine($"Removed {removed}");
```

<div v-click>

```text
Yesterday, The Beatles (2:05)
Imagine, John Lennon (3:03)
Bohemian Rhapsody, Queen (5:54)
Hotel California, Eagles (6:31)
Hey Jude, The Beatles (7:11)
Removed 1
```

</div>

::right::

<div class="pl-6 pt-22">

Both are methods of `List<T>` that take a delegate.

`Sort` takes a method that compares two songs. `CompareTo` returns what it needs: negative when `a` comes first, zero when they are equal, positive when `b` comes first.

`RemoveAll` removes every song for which the lambda returns `true`, and tells you how many it removed.

To sort from longest to shortest, swap `a` and `b` in the lambda.

</div>

---

# Summary

| | Example | What it means |
|---|---|---|
| generic class | `History<T>` | the type is picked where it is used |
| generic method | `Max<T>` | the type comes from the arguments |
| constraint | `where T : ...` | what `T` must be able to do |
| delegate | `Func<Song, bool>` | a type whose values are methods |
| lambda | `s => s.Seconds > 300` | a method without a name |

Pass a method by its name without parentheses: `IsLong`, not `IsLong()`.

<!--
Next week, events are built on delegates.
-->

---

# The debugger

Put a breakpoint on `if (test(item))` inside `CountWhere` and start debugging.

Press `F11` on that line. The debugger jumps to the lambda, at the top of `Program.cs`, where it was written. Press `F11` again to come back to `CountWhere`.

While you are inside the lambda, the Call Stack panel shows the lambda on top, `CountWhere` below it, and the main program at the bottom.

---
layout: center
---

# Assignments

There are two statements on CV UPT this week. Upload each one before the deadline shown there.

The first uses lists and a generic class of your own. The second asks for methods that take a `Func`, called with lambdas.
