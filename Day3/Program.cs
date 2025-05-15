using System.Text;

public class Day3{
    [Flags]
    public enum Direction{
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        UpRight = Up | Right,
        UpLeft = Up | Left,
        All = Up | Down | Left | Right
    }

    struct Color{
        public int red;
        public int green;
        public int blue;
    }

    // Demo Generic
    public class Stack<T>{
        int position;
        T[] data = new T[100];

        public void Push(T obj){
            data[position++] = obj;
        }
        public T Pop(){
            return data[--position];
        }
    }

    // Demo Indexer
    public class Sentence{
        private string[] words = "The Quick Brown Fox".Split();

        public string this[int wordNum]{
            get {return words[wordNum];}
            set { words[wordNum] = value;}
        }
    }

    public class Circle : IShape{
        public double Radius {get; set;}

        public Circle(double radius){
            Radius = radius;
        }

        public double Area() => Math.PI * Radius * Radius;

        public double Perimeter() => 2 * Math.PI * Radius;
    }

    public class Rectangle : IShape{
        public double Width {get; set;}
        public double Height {get; set;}

        public Rectangle(double width, double height){
            Width = width;
            Height = height;
        }
        public double Area() => Width * Height;

        public double Perimeter() => 2 * (Width + Height);
    }

    public static void Main()
    {
        // Demo Enum
        int todayDOW = 5;
        Console.WriteLine((DayOfWeek)todayDOW);

        Direction dir1 = Direction.Down | Direction.Right;
        Console.WriteLine(dir1.ToString());
        // Demo Bitwise Operator
        dir1 ^= Direction.Down;
        Console.WriteLine(dir1.ToString());
        dir1 |= Direction.Up;
        Console.WriteLine(dir1.ToString());

        Direction dir2 = Direction.All;
        Console.WriteLine(dir2.ToString());

        DayOfWeek invalidDay = (DayOfWeek)12345;
        Console.WriteLine(invalidDay);

        Color purple = new Color { red = 255, green = 0, blue = 255 };
        Console.WriteLine($"Accessing struct: {purple.red}");

        // Demo Null Operator
        string s1 = null;
        string s2 = s1 ?? "nothing";
        Console.WriteLine(s2);
        StringBuilder sb = new StringBuilder();
        sb = null;
        Console.WriteLine(sb?.ToString().ToUpper());

        // Demo Generic
        var stack1 = new Stack<int>();
        stack1.Push(5);
        stack1.Push(10);
        Console.WriteLine(stack1.Pop());

        // Generic Method
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        string str1 = "asdf";
        string str2 = "sdfg";
        Swap(ref str1, ref str2);
        Console.WriteLine($"Swapped string: {str1}, {str2}");
        int num1 = 2;
        int num2 = 4;
        Swap(ref num1, ref num2);
        Console.WriteLine($"Swapped integer: {num1}, {num2}");

        Sentence sentence1 = new Sentence();
        Console.WriteLine($"Indexer {sentence1[0]}");

        Rectangle r1 = new Rectangle(4, 5);
        Console.WriteLine($"Rectangle Area: {r1.Area()}");
    }
}
