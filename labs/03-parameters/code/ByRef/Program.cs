Point point = new Point { X = 0, Y = 0 };
MovePoint(ref point);
Console.WriteLine($"point.X = {point.X}");

Marker marker = new Marker { X = 1, Y = 1 };
ResetX(marker);
Replace(marker);
Console.WriteLine($"marker.X = {marker.X}");

Marker other = new Marker { X = 1, Y = 1 };
ReplaceByRef(ref other);
Console.WriteLine($"other.X = {other.X}");

int count = 0;
Increment(ref count);
Console.WriteLine($"count = {count}");

// Each of these is a compiler error. Uncomment one at a time to see it.
// int unset;
// Increment(ref unset);      // CS0165: the variable has no value
// Increment(ref 5);          // CS1510: 5 is not a variable
// Increment(ref marker.X);   // CS0206: X is a property

void MovePoint(ref Point p)
{
    p.X += 5;
}

void ResetX(Marker m)
{
    m.X = 0;
}

void Replace(Marker m)
{
    m = new Marker { X = 99, Y = 99 };
}

void ReplaceByRef(ref Marker m)
{
    m = new Marker { X = 99, Y = 99 };
}

void Increment(ref int n)
{
    n++;
}
