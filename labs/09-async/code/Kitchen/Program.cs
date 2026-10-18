using System.Diagnostics;

Stopwatch watch = Stopwatch.StartNew();
await BoilWaterAsync();
int slices = await ToastAsync(2);
Console.WriteLine($"{slices} slices after {watch.Elapsed.TotalSeconds:F1} s");
Console.WriteLine();

watch.Restart();
Task water = BoilWaterAsync();
Task<int> toast = ToastAsync(2);
await water;
slices = await toast;
Console.WriteLine($"{slices} slices after {watch.Elapsed.TotalSeconds:F1} s");
Console.WriteLine();

watch.Restart();
await Task.WhenAll(BoilWaterAsync(), ToastAsync(4), BoilWaterAsync());
Console.WriteLine($"All three after {watch.Elapsed.TotalSeconds:F1} s");
Console.WriteLine();

// The exception is kept in the task and thrown at the await.
Task<int> nothing = ToastAsync(0);
Console.WriteLine("No exception yet");
try
{
    await nothing;
}
catch (ArgumentException e)
{
    Console.WriteLine($"Caught: {e.Message}");
}

// Without await, the program does not wait for the water. Warning CS4014.
// BoilWaterAsync();

async Task BoilWaterAsync()
{
    Console.WriteLine("Kettle on");
    await Task.Delay(3000);
    Console.WriteLine("Water boiled");
}

async Task<int> ToastAsync(int slices)
{
    if (slices <= 0)
    {
        throw new ArgumentException("No bread");
    }
    Console.WriteLine($"Toasting {slices} slices");
    await Task.Delay(2000);
    Console.WriteLine("Toast ready");
    return slices;
}
