using System;
using System.Text;

// Demo Overflow
int maxInt = int.MaxValue;
maxInt++;
Console.WriteLine(maxInt);

// Demo StringBuilder
StringBuilder sb = new StringBuilder("Vinsensius");
Console.WriteLine(sb);
StringBuilder sb2 = sb;
sb2.Append(" Wijaya");
Console.WriteLine(sb);

// Demo Null
string nullString = null;
if (nullString != null && nullString.Length > 0) Console.WriteLine("check");

// Demo Method
static int Factorial(int num){
    if (num == 0 ) return 1;
    return num * Factorial(num - 1);
}
Console.WriteLine("Factor num: " + Factorial(8));

Circle coin = new Circle(3);
Console.WriteLine(coin.Radius);
Console.WriteLine(coin.Area());
Rectangle book = new Rectangle(5, 10);
Console.WriteLine(book.Area());

// Demo Upcasting
Shape wheel = new Circle(12);
// Console.WriteLine(wheel.Radius()); //Error as the base class Shape doesn't have Radius field
Console.WriteLine(wheel.Area());