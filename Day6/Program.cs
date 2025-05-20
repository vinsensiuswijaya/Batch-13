public class PriceChangedEventArgs : EventArgs
{
    public decimal LastPrice { get; }
    public decimal NewPrice { get; }

    public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
    {
        LastPrice = lastPrice;
        NewPrice = newPrice;
    }
}

public class Stock
{
    private decimal price;
    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public decimal Price
    {
        get => price;
        set
        {
            if (price != value)
            {
                var oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }

    protected virtual void OnPriceChanged(PriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }
}

public class Subscriber
{
    public void OnPriceChanged(object sender, PriceChangedEventArgs e)
    {
        Console.WriteLine($"Subscriber received that the price is changed from {e.LastPrice} to {e.NewPrice}");
    }
}

class Program
{
    static void Main()
    {
        Stock s1 = new Stock();
        Subscriber subscriber = new Subscriber();
        s1.PriceChanged += subscriber.OnPriceChanged;
        s1.Price = 100m;
        s1.Price = 200m;
        s1.PriceChanged -= subscriber.OnPriceChanged;
        s1.Price = 300m; // No output as the subscriber has unsubscribed

        // Demo Enumerator & Iterator
        IEnumerable<int> Fibs(int fibCount)
        {
            int prevFib = 0, curFib = 1;
            for (int i = 0; i < fibCount; i++)
            {
                yield return prevFib;
                int newFib = prevFib + curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }

        foreach (int fib in Fibs(6))
        {
            Console.Write($"{fib} ");
        }
        Console.WriteLine();

        IEnumerable<int> Triangular(int count)
        {
            int prev = 0, cur = 1;
            for (int i = 0; i < count; i++)
            {
                yield return prev;
                prev += cur;
                cur++;
            }
        }

        foreach (int sequence in Triangular(6))
        {
            Console.Write($"{sequence} ");
        }
    }
}