---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#1d4ed8'
title: "Lab 3: Parameter passing"
info: |
  CFLP laboratory, week 3.
layout: cover
---

# Lab 3: Parameter passing

How arguments reach a method, and how a method can send values back

---

# Where we left off

Last week `MovePoint` could not move the point:

```cs
Point point = new Point { X = 0, Y = 0 };
MovePoint(point);
Console.WriteLine(point.X);

void MovePoint(Point p)
{
    p.X += 5;
}
```

```text
0
```

A method gets a copy of each argument. `p` is a copy of `point`, so the change stays inside the method.

---
layout: two-cols
---

# A copy of a reference

```cs {1-4|6-9|11-14|all}
Marker marker = new Marker { X = 1, Y = 1 };
ResetX(marker);
Replace(marker);
Console.WriteLine(marker.X);

void ResetX(Marker m)
{
    m.X = 0;
}

void Replace(Marker m)
{
    m = new Marker { X = 99, Y = 99 };
}
```

<div v-click>

```text
0
```

</div>

::right::

<div class="pl-6 pt-22">

A class variable holds a reference to the object, and the method gets a copy of that reference. `m` and `marker` point to the same object, so `ResetX` changes it.

`Replace` points its own `m` to a new object:

```text
marker ───> [ X = 0  ]
m      ───> [ X = 99 ]
```

Without `ref`, C# always copies the argument. For a class, the reference is copied and the object is shared.

</div>

<!--
Ask the class to predict the output before revealing it.
Analogy: a photocopy of a paper with a house address. Whoever has the copy can repaint the house, and you see it. If they write a new address on their copy, your paper still has the old one. `ref` hands over the original paper.
This is the "call by sharing" case from the lecture: the method can change the object, but not which object the caller's variable points to.
-->

---

# `ref`

```cs {2|5|all}
Point point = new Point { X = 0, Y = 0 };
MovePoint(ref point);
Console.WriteLine(point.X);

void MovePoint(ref Point p)
{
    p.X += 5;
}
```

<div v-click>

```text
5
```

</div>

With `ref` the method gets the caller's variable instead of a copy. `p` is another name for `point`.

`ref` is written in both places. When you read the call, you can see that `point` may change.

---
layout: two-cols
---

# `ref` with a class

Without `ref`:

```cs
Marker marker = new Marker { X = 1, Y = 1 };
Replace(marker);
Console.WriteLine(marker.X);

void Replace(Marker m)
{
    m = new Marker { X = 99, Y = 99 };
}
```

```text
1
```

`m` is a copy of `marker`. Pointing the copy to a new object leaves `marker` where it was.

::right::

<div class="pl-6 pt-22">

With `ref`:

```cs
Marker marker = new Marker { X = 1, Y = 1 };
Replace(ref marker);
Console.WriteLine(marker.X);

void Replace(ref Marker m)
{
    m = new Marker { X = 99, Y = 99 };
}
```

```text
99
```

Now `m` is the caller's variable, so pointing it to a new object changes `marker` too.

</div>

<!--
In the ByRef project the ref version is called ReplaceByRef, because two local functions in one file cannot have the same name.
-->

---

# What `ref` needs

The argument must be a variable that already has a value.

```cs
int count;
Increment(ref count);
```

```text
error CS0165: Use of unassigned local variable 'count'
```

```cs
Increment(ref 5);
Increment(ref task.Estimate);
```

```text
error CS1510: A ref or out value must be an assignable variable
error CS0206: A non ref-returning property or indexer may not be used as an out or ref value
```

A literal is not a variable. A property is not one either: it is a pair of methods, as we saw last week.

<!--
`Increment` is `void Increment(ref int n) { n++; }`.
For a property, copy it into a local variable, pass that with ref, then assign it back.
-->

---

# `out`

```cs {1|2|4-8|all}
Divide(17, 5, out int quotient, out int remainder);
Console.WriteLine($"17 = 5 * {quotient} + {remainder}");

void Divide(int a, int b, out int quotient, out int remainder)
{
    quotient = a / b;
    remainder = a % b;
}
```

<div v-click>

```text
17 = 5 * 3 + 2
```

</div>

`out` is a `ref` that only goes out of the method. The caller does not have to give it a value, and the method must assign it before it returns. `out int quotient` declares the variable right in the call.

<!--
LIVE: delete `remainder = a % b;` and show error CS0177: The out parameter 'remainder' must be assigned to before control leaves the current method.
-->

---
layout: two-cols
---

# The `TryParse` pattern

A method that can fail on normal input:

- returns `bool`: did it work?
- gives the result through `out`
- sets the `out` values to a default when it fails
- does not throw for bad input

`int.TryParse` and `Enum.TryParse` follow it. So can your own methods.

`Split(':')` cuts `"14:30"` into `"14"` and `"30"`.

::right::

<div class="pl-6 pt-22">

```cs {4-5|6|7-10|12-16|all}
bool TryParseTime(string text,
                  out int hour, out int minute)
{
    hour = 0;
    minute = 0;
    string[] parts = text.Split(':');
    if (parts.Length != 2
        || !int.TryParse(parts[0], out int h)
        || !int.TryParse(parts[1], out int m)
        || h < 0 || h > 23 || m < 0 || m > 59)
    {
        return false;
    }
    hour = h;
    minute = m;
    return true;
}
```

</div>

<!--
LIVE: write this from an empty project.
`||` stops at the first true condition, so `parts[1]` is never read when there is only one part.
-->

---
layout: two-cols
---

# Using it

```cs {1-2|4|6-7|9-12|all}
Console.Write("Start time (hh:mm): ");
string text = Console.ReadLine() ?? "";

if (TryParseTime(text, out int h, out int m))
{
    int total = h * 60 + m;
    Console.WriteLine($"{total} minutes");
}
else
{
    Console.WriteLine("Use hh:mm, like 14:30");
}
```

::right::

<div class="pl-6 pt-22">

<div v-click>

```text
Start time (hh:mm): 14:30
870 minutes

Start time (hh:mm): 25:00
Use hh:mm, like 14:30
```

</div>

When you only need to know whether the text is valid, `out _` throws the value away:

```cs
bool valid = TryParseTime(text, out _, out _);
```

</div>

---

# Why a `bool` and not an exception

`int.Parse` throws an exception on `"abc"`. `int.TryParse` returns `false`. Both are in .NET because they fit different situations.

A user typing a wrong time is normal, so the method returns `false` and the caller decides what to do. The `if` around the call makes it hard to forget that case.

An exception is for something the caller could not reasonably expect, such as a file that disappears while you read it. We look at exceptions in lab 5.

---
layout: two-cols
---

# `params`

```cs
Console.WriteLine(Max(3, 7, 2));
Console.WriteLine(Max(10));
int[] scores = [4, 9, 1];
Console.WriteLine(Max(scores));
```

```text
7
10
9
```

`params` collects any number of arguments into an array. It must be the last parameter.

<v-click>

What does `Max()` do?

</v-click>

::right::

<div class="pl-6 pt-22">

```cs
int Max(params int[] values)
{
    int max = values[0];
    foreach (int v in values)
    {
        if (v > max)
        {
            max = v;
        }
    }
    return max;
}
```

</div>

<!--
Answer: `values` is an empty array, so `values[0]` throws IndexOutOfRangeException. Decide with the class: throw an ArgumentException with a clear message, or make the first value a normal parameter: `Max(int first, params int[] rest)`.
`Console.WriteLine("{0} and {1}", a, b)` uses params too.
-->

---

# Summary

| | In the call | Needs a value first | Method can change the variable |
|---|---|---|---|
| nothing | `F(x)` | yes | no |
| `ref` | `F(ref x)` | yes | yes |
| `out` | `F(out x)` | no | yes, and it must |

Without `ref`, a method can still change the object that a class variable points to. It cannot make the variable point somewhere else.

`params` is about how many arguments a method takes. They arrive as an array.

<!--
If someone asks: `in` passes a variable by reference but read-only. It is mostly used to avoid copying large structs.
-->

---

# The debugger

Put a breakpoint on `p.X += 5;` inside `MovePoint(ref Point p)` and start debugging. Press `F10` to run that line.

In the Call Stack panel, click the line below `MovePoint` to look at the code that called it. `point.X` is already `5`, while the method is still running.

Remove `ref` from both places and try again. The caller still sees `0`, because the method changed its own copy.

`Shift+F11` runs to the end of the current method and returns to the caller.

---
layout: center
---

# Assignment

The statement is on CV UPT. Upload your solution before the deadline shown there.

It uses the same ideas as today: `ref` parameters, structs passed to methods, and a method that returns `bool` with its result in an `out` parameter.
