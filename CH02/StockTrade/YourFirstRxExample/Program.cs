using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstRxExample
{
    class Program
    {
        private static void Main(string[] args)
        {
        
            //
            // Uncomment the StockTicker version you wish to test
            //
            var stockTicker = new StockTicker();
            //var stockTicker = new RxStockMonitor(stockTicker);

            //
            // Uncomment to test the case of Concurrent Ticks
            //
            //TestConcurrentTicks(stockTicker);

            // A small program to let you enter the Ticks info.
            // Symbol X will exit the program 
            while (true)
            {
                Console.Write("enter symbol: ");
                var symbol = Console.ReadLine();
                if (symbol.ToLower() == "x")
                {
                    break;
                }
                Console.WriteLine("enter value: ");
                var val = decimal.Parse(Console.ReadLine());
                for (int i = 0; i < 30; i++)
                {
                    int i1 = i;
                    Task.Run(() => stockTicker.Notify(new StockTick() { Price = val, QuoteSymbol = symbol + i1 }));
                }
            }

            Console.WriteLine("Bye Bye");
        }

        private static void TestConcurrentTicks(StockTicker stockTicker)
        {
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() {Price = 100, QuoteSymbol = "MSFT"}));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() {Price = 150, QuoteSymbol = "INTC"}));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() {Price = 170, QuoteSymbol = "MSFT"}));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() {Price = 195.5M, QuoteSymbol = "MSFT"}));
        }
    }
}
