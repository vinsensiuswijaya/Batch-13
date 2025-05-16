interface IDrawable
{
    void Draw();
}

interface IColorable
{
    ShapeColor Color { get; set; }
    void SetColor(ShapeColor color);
}

interface IShape : IDrawable, IColorable
{
    double Area();
    double Perimeter();
}

public class Circle : IShape
{
    public double Radius { get; set; }
    public ShapeColor Color { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
        Color = ShapeColor.White;
    }

    public void Draw()
    {
        Console.WriteLine($"{Color} circle with area of {Area():F2} is drawn");
    }

    public void SetColor(ShapeColor color)
    {
        Color = color;
    }

    public double Area()
    {
        return Math.PI * Radius * Radius;
    }

    public double Perimeter()
    {
        return 2 * Math.PI * Radius;
    }
}

public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public ShapeColor Color { set; get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
        Color = ShapeColor.White;
    }
    public Rectangle(double width, double height, ShapeColor color)
    {
        Width = width;
        Height = height;
        Color = color;
    }
    public void SetColor(ShapeColor color)
    {
        Color = color;
    }
    public void Draw()
    {
        Console.WriteLine($"{Color} rectangle with area of {Area():F2} is drawn");
    }
    public double Area()
    {
        return Width * Height;
    }
    public double Perimeter()
    {
        return 2 * (Width + Height);
    }
}

public class Program
{
    public static void Main()
    {
        Circle circle1 = new Circle(5);
        circle1.SetColor(ShapeColor.Red);
        circle1.Draw();
        Rectangle rect1 = new Rectangle(4, 5);
        Rectangle rect2 = new Rectangle(5, 7, ShapeColor.Green);
        rect1.Draw();
        rect2.Draw();
    }
}