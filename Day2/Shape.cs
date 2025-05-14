// Demo Open-Closed Principle
public abstract class Shape{
    public abstract double Area();
}

public class Rectangle : Shape{
    public double Width {get; set;}
    public double Height {get; set;}
    public Rectangle(double width, double height){
        Width = width;
        Height = height;
    }
    public override double Area()
    {
        return Width * Height;
    }
}

public class Circle : Shape{
    public double Radius {get; set;}
    public Circle(int radius){
        Radius = radius;
    }
    public override double Area()
    {
        return 3.14 * Radius *Radius;
    }
}