Point p1 = new Point { X = 1, Y = 2 };
Point p2 = p1;
p2.X = 10;

Marker m1 = new Marker { X = 1, Y = 2 };
Marker m2 = m1;
m2.X = 10;

Console.WriteLine($"p1.X = {p1.X}");
Console.WriteLine($"m1.X = {m1.X}");

Point point = new Point { X = 0, Y = 0 };
Marker marker = new Marker { X = 0, Y = 0 };
MovePoint(point);
MoveMarker(marker);
Console.WriteLine($"point.X = {point.X}");
Console.WriteLine($"marker.X = {marker.X}");

void MovePoint(Point p)
{
    p.X += 5;
}

void MoveMarker(Marker m)
{
    m.X += 5;
}
