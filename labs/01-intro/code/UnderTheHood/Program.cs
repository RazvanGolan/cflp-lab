using System.Reflection;
using System.Runtime.CompilerServices;

#region entry
MethodInfo entry = Assembly.GetEntryAssembly()!.EntryPoint!;
Console.WriteLine($"Entry point: {entry.DeclaringType}.{entry.Name}");
#endregion

#region il
MethodInfo add = typeof(Calculator).GetMethod(nameof(Calculator.Add))!;
byte[] il = add.GetMethodBody()!.GetILAsByteArray()!;
Console.WriteLine($"IL of Add: {Convert.ToHexString(il)}");
#endregion

#region hot
long total = 0;
for (int i = 0; i < 100_000_000; i++)
{
    total = Calculator.Add(total, i);
}
Console.WriteLine($"Total: {total}");
#endregion

#region calculator
static class Calculator
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static long Add(long a, long b) => a + b;
}
#endregion
