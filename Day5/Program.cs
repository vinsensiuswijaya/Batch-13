﻿public class Program
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
        void PrintAll<T>(List<T> items)
        {
            foreach (T item in items)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }

        // Demo Func
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        Func<int, bool> isDivisibleByThree = num => num % 3 == 0;
        Func<int, int> multipleByFive = num => num * 5;
        Func<int, bool> isDivisibleByFifteen = num => num % 15 == 0;

        List<int> num2 = numbers.Where(isDivisibleByThree).ToList();
        PrintAll<int>(num2);

        List<int> num3 = numbers.Select(multipleByFive).Where(isDivisibleByFifteen).ToList();
        PrintAll<int>(num3);

        // Demo multicast delegate
        Transformer t = Square;
        t += PlusThree;
        int result = t(4);
        Console.WriteLine($"result: {result}");

        // Demo try catch
        double Divide(int x, int y) => x / y;

        double dividedNum;
        try
        {
            dividedNum = Divide(4, 0);
            Console.WriteLine(dividedNum);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Cannot divide by zero");
        }
        catch (System.Exception)
        {
            Console.WriteLine("Error");
        }

        // Demo Nullable Value Type
        int? x = null;
        int y = x ?? 0;
        Console.WriteLine(y);

        bool? n = null;
        bool? f = false;
        bool? tru = true;

        Console.WriteLine(n | tru);
    }
}