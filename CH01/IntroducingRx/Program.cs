using System;
using System.Linq;

namespace IntroducingRx
{
    class Program
    {
        static void Main(string[] args)
        {
            IObservable<string> strings =
                new[] { "Hello", "Rx", "A", "AB" }.ToObservable();

            IDisposable subscription =
                strings.Where(str => str.StartsWith("A"))
                    .Select(str => str.ToUpper())
                    .Subscribe(Console.WriteLine);

            subscription.Dispose();
        }
    }
}
