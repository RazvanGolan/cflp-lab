using Shapes;

List<IShape> shapes = [new Circle(1), new Square(2), new Circle(0.5)];

double total = 0;
foreach (IShape shape in shapes)
{
    Console.WriteLine($"{shape.Name}: {shape.Area():F2}");
    total += shape.Area();
}
Console.WriteLine($"Total area: {total:F2}");
