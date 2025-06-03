public class Program
{
    public static void Main()
    {
        Generator g1 = new Generator(new Dictionary<int, string>
        {
            [3] = "foo",
            [4] = "baz",
            [5] = "bar",
            [7] = "jazz",
            [9] = "huzz"
        });
        Console.Write("Input number: ");
        int number = int.Parse(Console.ReadLine());
        g1.Print(number);
    }
}

public class Generator
{
    public Dictionary<int, string> Rules { get; set; }
    public Generator()
    {
        Rules = new Dictionary<int, string>();
    }
    public Generator(Dictionary<int, string> rules)
    {
        Rules = rules;
    }
    public void AddRule(int input, string output)
    {
        Rules[input] = output;
    }
    public void Print(int number)
    {
        for (int num = 1; num <= number; num++)
        {
            bool isChecked = false;
            foreach (var rule in Rules)
            {
                if (num % rule.Key == 0)
                {
                    Console.Write($"{rule.Value}");
                    isChecked = true;
                }
            }
            if (!isChecked) Console.Write($"{num}");
            if (num != number) Console.Write(", ");
        }
    }
}
