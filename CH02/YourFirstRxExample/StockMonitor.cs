using System;
using System.Collections.Generic;

namespace FirstRxExample {

    class StockMonitor : IDisposable {
        readonly object _stockTickLocker = new object();
        private readonly StockTicker _ticker;
        readonly Dictionary<string, StockInfo> _stockInfos = new Dictionary<string, StockInfo>();

        public StockMonitor(StockTicker ticker) {
            this._ticker = ticker;
            ticker.StockTick += this.OnStockTick;
        }

        //rest of the code

        void OnStockTick(object sender, StockTick stockTick) {
            const decimal maxChangeRatio = 0.1m;
            var quoteSymbol = stockTick.QuoteSymbol;
            lock (this._stockTickLocker) {
                var stockInfoExists = this._stockInfos.TryGetValue(quoteSymbol, out var stockInfo);
                if (stockInfoExists) {
                    var priceDiff = stockTick.Price - stockInfo.PrevPrice;
                    var changeRatio = Math.Abs(priceDiff / stockInfo.PrevPrice); //#A the percentage of change
                    if (changeRatio > maxChangeRatio) {
                        Console.WriteLine("Stock:{0} has changed with {1} ratio, Old Price:{2} New Price:{3}", quoteSymbol,
                            changeRatio,
                            stockInfo.PrevPrice,
                            stockTick.Price);
                    }
                    this._stockInfos[quoteSymbol].PrevPrice = stockTick.Price;
                } else {
                    this._stockInfos[quoteSymbol] = new StockInfo(quoteSymbol, stockTick.Price);
                }
            }
        }

        public void Dispose() {
            this._ticker.StockTick -= this.OnStockTick;
            this._stockInfos.Clear();
        }
    }
}
