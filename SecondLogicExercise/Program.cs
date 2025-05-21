Console.Write("Input Number: ");
int number = int.Parse(Console.ReadLine());

for (int i = 1; i <= number; i++)
{
    if (i % 3 == 0) Console.Write("foo");
    if (i % 5 == 0) Console.Write("bar");
    if (i % 7 == 0) Console.Write("jazz");
    if (i % 3 != 0 && i % 5 != 0 && i % 7 != 0) Console.Write(i);
    Console.Write(", ");
}