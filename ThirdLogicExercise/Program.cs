Console.Write("Input Number: ");
int number = int.Parse(Console.ReadLine());
bool isChecked;

for (int i = 1; i <= number; i++)
{
    isChecked = false;
    if (i % 3 == 0) Printer("foo");
    if (i % 4 == 0) Printer("baz");
    if (i % 5 == 0) Printer("bar");
    if (i % 7 == 0) Printer("jazz");
    if (i % 9 == 0) Printer("huzz");
    if (!isChecked) Console.Write(i);
    if (i != number) Console.Write(", ");
}

void Printer(string output)
{
    isChecked = true;
    Console.Write(output);
}