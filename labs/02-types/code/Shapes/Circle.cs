namespace Shapes;

public class Circle(double r) : IShape
{
    public string Name => "circle";
    public double Area() => Math.PI * r * r;
}
