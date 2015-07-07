using System;
using System.Collections.Generic;

namespace FirstRxExample
{
    class StockMonitor : IDisposable
    {
        object _stockTickLocker = new object();
        private readonly StockTicker _ticker;
        Dictionary<string, StockInfo> _stockInfos = new Dictionary<string, StockInfo>();
        public StockMonitor(StockTicker ticker)
        {
            _ticker = ticker;
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
                    var changeRatio = Math.Abs(priceDiff / stockInfo.PrevPrice); //#A the percentage of change
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
}