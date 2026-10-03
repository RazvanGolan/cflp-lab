namespace Shapes;

public struct Square(double side) : IShape
{
    public string Name => "square";
    public double Area() => side * side;
}
