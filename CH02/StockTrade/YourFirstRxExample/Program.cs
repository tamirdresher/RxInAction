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
            var stockTicker = new StockTicker();


            /////////////////////////////////////////////////////////////
            //                                                         //
            // 1. Uncomment the StockMonitor version you wish to test  //
            //                                                         //
            /////////////////////////////////////////////////////////////

            // Regular events StockMonitor
            var stockMonitor = new StockMonitor(stockTicker);
            
            // Rx StockMonitor
            //var stockMonitor = new RxStockMonitor(stockTicker);

            //////////////////////////////////////////////////////////////////
            //                                                              //
            // 2. (optional) Uncomment to test the case of Concurrent Ticks //
            //                                                              //
            //////////////////////////////////////////////////////////////////

            //TestConcurrentTicks(stockTicker);

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
                    stockTicker.Notify(new StockTick() {Price = price, QuoteSymbol = symbol});
                }
                else
                {
                    Console.WriteLine("price should be decimal");
                }
            }

            GC.KeepAlive(stockMonitor);
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
