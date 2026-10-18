# Lab 11: Test 2 recap

Slides: [lab](https://razvangolan.github.io/cflp-lab/11-test-2-recap/slides/)

A recap for Test 2: methods that take a `Func`, LINQ one-liners, and a program where a class hierarchy, an event and an async loop work together. There is no new code this week.

## What Test 2 asks for

1. A method that takes a `Func`, written by hand with a loop. See lab 5 (`CountWhere`, `TotalOf`).
2. A few LINQ one-liners. See lab 10 and its summary slide.
3. A longer program: an abstract class with derived classes (lab 4), a class that keeps a list of them (`Catalog` in lab 4), and a timer class that raises an event (lab 6) from an async `StartAsync` loop (lab 9), all wired together in `Main`.

## Things to check

- A method that takes a `Func` calls it on each item inside the loop and returns its result after the loop, or as soon as it knows the answer.
- A LINQ one-liner starts from the collection, adds one step for each part of the question, and ends with what the question asks for: a number, one item, or a list with `ToList()`.
- The manager class keeps a private `List<Base>` and only uses members of the base class in its loops, with no casts and no `is`.
- The timer's `StartAsync` is `async Task`, waits with `await Task.Delay`, and raises its event with `?.Invoke(...)`.
- In `Main`, subscribe with `+=` before you start the timer, and `await` the call to `StartAsync`.
- Do not name your timer class `Timer`. .NET already has `System.Threading.Timer`, and `Program.cs` sees both (error CS0104).

## Practice

Do these in a new project (`dotnet new console -n Parking`), without looking at earlier labs:

1. `Dictionary<string, int> CountBy<T>(List<T> items, Func<T, string> key)` counts how many items have each key. Call it on a list of words with their first letter as the key, then on a list of numbers with `"even"` or `"odd"`.
2. For `List<int> minutes = [35, 120, 15, 240, 60, 70];`, the minutes some cars stayed in a car park, write one line for each: the stays longer than an hour, longest first; the total time in hours; whether any stay was shorter than 20 minutes; each stay as a text such as `"35 min"`.
3. An abstract class `Vehicle` with a `Plate`, an abstract `PricePerHour` and a `Bill` that starts at 0. A method `Charge()` adds one hour's price to the bill. `Car` costs 5 lei an hour and `Motorbike` 2 lei. Both override `ToString`. A class `Garage` keeps a `List<Vehicle>`: `Park` adds a vehicle, and `OnHour(int hour)` charges every vehicle and prints it. A class `Clock` has an event `Action<int>? Hour` and a method `async Task StartAsync(int hours)` that raises `Hour` once every half second, `hours` times. In `Main`, park two cars and a motorbike, subscribe the garage to the clock, run it for 3 hours, then print the total of all bills.

## Further reading

- [Asynchronous programming with async and await](https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/)
- [Standard query operators](https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/)
- [Events](https://learn.microsoft.com/dotnet/csharp/programming-guide/events/)
