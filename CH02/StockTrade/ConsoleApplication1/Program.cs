using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstRxExample
{
    class StockInfo
    {

        public StockInfo(string symbol, decimal price)
        {
            Symbol = symbol;
            PrevPrice = price;
        }
        public string Symbol { get; set; }
        public decimal PrevPrice { get; set; }
    }

    public class StockTick
    {
        public string QuoteSymbol { get; set; }
        public decimal Price { get; set; }

        //other properties
    }
    public class StockTicker
    {
        public event EventHandler<StockTick> StockTick;

        public void Notify(StockTick tick)
        {
            StockTick(this, tick);
        }
    }
    class Program
    {
       

class StockMonitor : IDisposable
{
    object _stockTickLocker=new object();//#A
    private readonly StockTicker _ticker;
    Dictionary<string, StockInfo> _stockInfos = new Dictionary<string, StockInfo>();
    public StockMonitor(StockTicker ticker)
    {
        _ticker = ticker;
        //var stockTicker = new StockTicker();
        ticker.StockTick += OnStockTick;
    }

    //rest of the code

void OnStockTick(object sender, StockTick stockTick)
{
    const decimal maxChangeRatio = 0.1m;
    StockInfo stockInfo;
    var quoteSymbol = stockTick.QuoteSymbol;
    lock (_stockTickLocker)
    {
        var stockInfoExists = _stockInfos.TryGetValue(quoteSymbol, out stockInfo);
        if (stockInfoExists)
        {
            var priceDiff = stockTick.Price - stockInfo.PrevPrice;
            var changeRatio = Math.Abs(priceDiff/stockInfo.PrevPrice); //#A the percentage of change
            if (changeRatio > maxChangeRatio)
            {
                Console.WriteLine("Stock:{0} has changed with {1} ratio, Old Price:{2} New Price:{3}", quoteSymbol,
                    changeRatio,
                    stockInfo.PrevPrice,
                    stockTick.Price);
            }
            _stockInfos[quoteSymbol].PrevPrice = stockTick.Price;
        }
        else
        {
            _stockInfos[quoteSymbol] = new StockInfo(quoteSymbol, stockTick.Price);
        }
    }
}

    public void Dispose()
    {
        _ticker.StockTick -= OnStockTick;
        _stockInfos.Clear();
    }
}

        private static void Main(string[] args)
        {
            var shoppy = new Shoppy();
            shoppy.TestShoppy();
            return;
            ;

            var stockTicker = new StockTicker();
            var stockListner = new RxStockMonitor(stockTicker);
            ThreadPool.QueueUserWorkItem((_)=>stockTicker.Notify(new StockTick() { Price = 100, QuoteSymbol = "MSFT" }));
            ThreadPool.QueueUserWorkItem((_)=>stockTicker.Notify(new StockTick() { Price = 150, QuoteSymbol = "INTC" }));
            ThreadPool.QueueUserWorkItem((_)=>stockTicker.Notify(new StockTick() { Price = 170, QuoteSymbol = "MSFT" }));
            ThreadPool.QueueUserWorkItem((_)=>stockTicker.Notify(new StockTick() { Price = 195.5M, QuoteSymbol = "MSFT" }));


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


    }

   
}
