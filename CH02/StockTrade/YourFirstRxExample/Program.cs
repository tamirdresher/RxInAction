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
        private static StockTicker _stockTicker;

        private static void Main(string[] args)
        {
            _stockTicker = new StockTicker();


            /////////////////////////////////////////////////////////////
            //                                                         //
            // 1. Uncomment the StockMonitor version you wish to test  //
            //                                                         //
            /////////////////////////////////////////////////////////////

            // Regular events StockMonitor
            //var stockMonitor = new StockMonitor(_stockTicker);

            // Rx StockMonitor - uses Rx to consume and process the stock ticks
            var stockMonitor = new RxStockMonitor(_stockTicker);

            ShowMenu();
          
            GC.KeepAlive(stockMonitor);
            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
            Console.WriteLine("Bye Bye");
        }

        private static void ShowMenu()
        {

            Console.WriteLine("Choose a simulation type (or x to exit):");
            Console.WriteLine("1) Manual     - you enter the symbol and price");
            Console.WriteLine("2) Automatic  - the system emits and updates a predefined collection of ticks");
            Console.WriteLine("3) Concurrent - tests what happens when ticks are emitted concurrently");

            var selection = Console.ReadLine();
            switch (selection)
            {
                case "1":
                    ManualSimulator(_stockTicker);
                    break;
                case "2":
                    AutomaticSimulator(_stockTicker);
                    break;
                case "3":
                    TestConcurrentTicks(_stockTicker);
                    break;
                case "x":
                    return;
                default:
                    Console.WriteLine("Unknow selection");
                    return;
            }

        }

        private static void AutomaticSimulator(StockTicker stockTicker)
        {
            var simulator = new StockSimulator(stockTicker);
            simulator.Run();
        }

        private static void ManualSimulator(StockTicker stockTicker)
        {
            //////////////////////////////////////////////////////
            // A small program to let you enter the Ticks info. //
            // Symbol X will exit the program                   // 
            //////////////////////////////////////////////////////
            while (true)
            {
                Console.Write("enter symbol (or x to exit): ");
                var symbol = Console.ReadLine();
                if (symbol.ToLower() == "x")
                {
                    break;
                }
                Console.WriteLine("enter price: ");
                decimal price;
                if (decimal.TryParse(Console.ReadLine(), out price))
                {
                    stockTicker.Notify(new StockTick() { Price = price, QuoteSymbol = symbol });
                }
                else
                {
                    Console.WriteLine("price should be decimal");
                }
            }
        }

        private static void TestConcurrentTicks(StockTicker stockTicker)
        {
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() { Price = 100, QuoteSymbol = "MSFT" }));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() { Price = 150, QuoteSymbol = "INTC" }));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() { Price = 170, QuoteSymbol = "MSFT" }));
            ThreadPool.QueueUserWorkItem((_) => stockTicker.Notify(new StockTick() { Price = 195.5M, QuoteSymbol = "MSFT" }));
        }
    }
}
