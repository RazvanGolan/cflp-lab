using System.Reflection;
using System.Runtime.CompilerServices;

MethodInfo entry = Assembly.GetEntryAssembly()!.EntryPoint!;
Console.WriteLine($"Entry point: {entry.DeclaringType}.{entry.Name}");

MethodInfo add = typeof(Calculator).GetMethod(nameof(Calculator.Add))!;
byte[] il = add.GetMethodBody()!.GetILAsByteArray()!;
Console.WriteLine($"IL of Add: {Convert.ToHexString(il)}");

long total = 0;
for (int i = 0; i < 100_000_000; i++)
{
    total = Calculator.Add(total, i);
}
Console.WriteLine($"Total: {total}");

static class Calculator
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static long Add(long a, long b) => a + b;
}
