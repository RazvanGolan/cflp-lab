---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#15803d'
title: "Lab 6: Events"
info: |
  CFLP laboratory, week 6.
layout: cover
---

# Lab 6: Events

Letting other code know that something happened, then a recap for Test 1

---

# Where we left off

Last week a delegate held one method, and we passed it to another method:

```cs
CountWhere(songs, s => s.Artist == "The Beatles");
```

Today a delegate holds several methods at once. An event uses that to tell any number of listeners that something happened.

The last half hour is a recap for next week's test.

---
layout: two-cols
---

# One delegate, several methods

```cs {1-3|5-6|8-10|all}
Action<string>? log = ToScreen;
log += Shout;
log("hello");

log -= ToScreen;
log?.Invoke("again");

log -= Shout;
log?.Invoke("nobody hears this");
Console.WriteLine(log == null);
```

```cs
void ToScreen(string text)
    => Console.WriteLine($"screen: {text}");
void Shout(string text)
    => Console.WriteLine(
        $"shout: {text.ToUpper()}");
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
screen: hello
shout: HELLO
shout: AGAIN
True
```

</div>

`+=` adds a method to the delegate and `-=` removes it. One call runs every method in it, in the order they were added.

When the last method is removed, the delegate is `null`. Calling it directly would throw a `NullReferenceException`. `?.Invoke` calls it only if it is not `null`.

</div>

<!--
LIVE: the Multicast project.
Ask before revealing: what does the last line print?
The compiler warns (CS8602) if you call `log("again")` right after a `-=`, because the delegate may be null by then.
-->

---

# A music player

A `Player` plays the songs in its queue. Other parts of a program want to know when a song starts:

- the screen shows the title
- a counter adds up how many songs were played
- later, maybe a history or a notification

The `Player` should not know about any of them. If it called the screen and the counter directly, every new feature would mean changing `Player`.

The player announces that a song started, and whoever is interested listens.

---
layout: two-cols
---

# An event

```cs {5|13|all}
public class Player
{
    private readonly List<Song> queue = [];

    public event Action<Song>? SongStarted;
    public event Action? QueueFinished;

    // Add puts a song in the queue

    public void PlayAll()
    {
        foreach (Song song in queue)
        {
            SongStarted?.Invoke(song);
        }
        queue.Clear();
        QueueFinished?.Invoke();
    }
}
```

::right::

<div class="pl-6 pt-22">

`event Action<Song>` is a delegate that other classes can subscribe to. Each subscriber is a method that takes a `Song`.

`SongStarted?.Invoke(song)` raises the event: it calls every subscriber with the song that started.

With no subscribers the event is `null`, which is why the call uses `?.`.

</div>

<!--
LIVE: write Player in the Music project. Song is the same class as in lab 5.
-->

---
layout: two-cols
---

# A listener

```cs
public class PlayCounter
{
    public int Played { get; private set; }
    public int Seconds { get; private set; }

    public void OnSongStarted(Song song)
    {
        Played++;
        Seconds += song.Seconds;
    }
}
```

::right::

<div class="pl-6 pt-22">

`PlayCounter` adds up the songs that were played and their length.

It knows nothing about `Player`. It only has a method with the right shape for `SongStarted`: it takes a `Song` and returns nothing.

</div>

---

# Subscribing

The player has "Imagine" and "Yesterday" in its queue.

```cs {1|3-4|6-7|9|all}
player.SongStarted += song => Console.WriteLine($"Now playing: {song}");

PlayCounter counter = new PlayCounter();
player.SongStarted += counter.OnSongStarted;

player.QueueFinished += () =>
    Console.WriteLine($"Done: {counter.Played} songs, {counter.Seconds} s");

player.PlayAll();
```

<div v-click>

```text
Now playing: Imagine, John Lennon (3:03)
Now playing: Yesterday, The Beatles (2:05)
Done: 2 songs, 308 s
```

</div>

A subscriber can be a lambda or a method of another object.

---

# Unsubscribing

```cs
player.SongStarted -= counter.OnSongStarted;
player.Add(new Song("Hey Jude", "The Beatles", 431));
player.PlayAll();
```

<div v-click>

```text
Now playing: Hey Jude, The Beatles (7:11)
Done: 2 songs, 308 s
```

</div>

The counter no longer hears about new songs. The screen and the "Done" message are still subscribed.

`-=` with a lambda removes nothing, because every lambda you write is a new method:

```cs
player.SongStarted -= song => Console.WriteLine($"Now playing: {song}");
```

To be able to unsubscribe a lambda later, keep it in a variable and use the variable with both `+=` and `-=`.

<!--
An object stays alive as long as an event it subscribed to is alive, because the event keeps a reference to its method. That is why long-lived programs unsubscribe when a listener is no longer needed.
-->

---

# Why the `event` keyword

Without `event`, `SongStarted` would be a public delegate field. Then any code could do this:

```cs
player.SongStarted = null;
player.SongStarted(new Song("Imagine", "John Lennon", 183));
```

The first line removes every other subscriber. The second pretends that a song started.

With `event`, code outside `Player` can only subscribe and unsubscribe:

```text
error CS0070: The event 'Player.SongStarted' can only appear on the left hand side
of += or -= (except when used from within the type 'Player')
```

Only the class that declares the event can raise it.

<!--
LIVE: uncomment the two lines at the end of the Music project's Program.cs. Then remove the `event` keyword from Player and show that they compile.
-->

---

# The usual .NET shape

Events in the .NET libraries, such as a button's `Click`, use `EventHandler<T>`:

```cs
public event EventHandler<Song>? SongStarted;

SongStarted?.Invoke(this, song);

player.SongStarted += (sender, song) => Console.WriteLine(song.Title);
```

The first argument is the object that raised the event, so one method can listen to several players and still know which one called.

The `Action<T>` form we used works the same way, and it is what the assignment uses.

---

# Summary

| | Example |
|---|---|
| declare | `public event Action<Song>? SongStarted;` |
| raise | `SongStarted?.Invoke(song);` |
| subscribe | `player.SongStarted += OnSongStarted;` |
| unsubscribe | `player.SongStarted -= OnSongStarted;` |

Raising an event calls every subscriber, in order. Only the class that declares the event can raise it; everyone else can only use `+=` and `-=`.

---

# The debugger

Put a breakpoint on `SongStarted?.Invoke(song);` in `Player.PlayAll` and start debugging.

Press `F11`. The debugger goes into the first subscriber, the lambda in `Program.cs`. Press `Shift+F11` to return, then `F11` again to step into `PlayCounter.OnSongStarted`.

Each subscriber finishes before the next one starts, and `PlayAll` continues only after all of them have run.

---
layout: cover
---

# Recap for Test 1

`out` and the `TryParse` pattern, methods that take a `Func`, and an abstract class with a list of the base type

---
layout: two-cols
---

# `out` and `TryParse`

```cs
bool TryParseTime(string text,
                  out int hour, out int minute)
{
    hour = 0;
    minute = 0;
    // split and parse the text,
    // return false when something
    // is wrong
    hour = h;
    minute = m;
    return true;
}
```

```cs
if (TryParseTime(text, out int h, out int m))
{
    // use h and m
}
```

::right::

<div class="pl-6 pt-22">

Check:

- it returns `bool`
- every `out` gets a value on every path, even when it fails
- it does not throw for bad input
- `out` is written in the call too

Lab 3 has the full example.

</div>

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
CountWhere(songs, s => s.Seconds > 300);
```

::right::

<div class="pl-6 pt-22">

Check:

- `Func<int, bool>` takes an `int` and returns a `bool`
- the loop calls it on each item: `test(item)`
- it returns as soon as it knows the answer
- the call passes a lambda, or a method name without `()`

Lab 5 has `CountWhere` and `TotalOf`.

</div>

---
layout: two-cols
---

# Abstract classes

```cs
public abstract class Media
{
    public string Title { get; }

    protected Media(string title)
    {
        Title = title;
    }

    public abstract int Minutes { get; }

    public override string ToString()
        => $"{Title}, {Minutes} min";
}
```

::right::

<div class="pl-6 pt-22">

Check:

- the base class and its member are `abstract`
- each derived constructor calls `base(...)`
- each abstract member and `ToString` has `override`
- the loop over `List<Media>` has no casts and no `is`

Lab 4 has the full example.

</div>

<!--
This Media is shorter than the one in lab 4 (no Year), to fit on the slide.
Without `override` on ToString you get warning CS0114 and the old version runs. A missing `out` assignment is error CS0177.
-->

---

# Practice

Write these in a new console project. Ask if you get stuck.

1. An abstract class `Parcel` with a `Weight` in kg and an abstract `Cost()`. A `Letter` costs 2 lei. A `Box` costs 5 lei plus 3 lei per kg. Both override `ToString`.
2. A method `bool TryParseWeight(string text, out double kg)` that accepts text such as `"2.5kg"` and fails on anything else, or on a weight of zero or less.
3. A method `double SumOf(List<Parcel> parcels, Func<Parcel, double> value)`, then use it to add up the cost and the weight of a list with letters and boxes.

<!--
About 15 minutes. Walk around and check the lists from the previous three slides.
Hint for 2: `text.EndsWith("kg")`, then `double.TryParse` on the rest. The lab machines may use a locale with a comma, so test "2.5" with CultureInfo.InvariantCulture if parsing fails.
-->

---
layout: center
---

# Assignment

The statement is on CV UPT. Upload your solution before the deadline shown there.

It uses the same ideas as today: a class that raises an event with `Action<T>`, and other classes that subscribe to it and react.
