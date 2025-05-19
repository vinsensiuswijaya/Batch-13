public class Program
{
    delegate int Transformer(int x);

    public static void Main()
    {
        int Square(int x)
        {
            int result = x * x;
            Console.WriteLine(result);
            return result;
        }
        int PlusThree(int x)
        {
            int result = x + 3;
            Console.WriteLine(result);
            return result;
        }

        // Demo Func
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        Func<int, bool> isDivisibleByThree = num => num % 3 == 0;
        Func<int, int> multipleByFifteen = num => num * 15;

        List<int> num2 = numbers.Where(isDivisibleByThree).ToList();
        foreach (int num in num2)
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine();

        List<int> num3 = numbers.Select(multipleByFifteen).ToList();
        foreach (int num in num3)
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine();

        // Demo multicast delegate
        Transformer t = Square;
        t += PlusThree;
        int result = t(4);
        Console.WriteLine($"result: {result}");


    }
}