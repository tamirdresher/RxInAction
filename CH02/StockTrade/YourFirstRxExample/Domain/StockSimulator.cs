using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstRxExample
{
    /// <summary>
    /// This simulator will emit a batch of StockTicks every two seconds.
    /// each time, a single item will be selected and updated with a "drastic change" (more than 10%)
    /// </summary>
    public class StockSimulator
    {
        private readonly StockTicker _ticker;
        private IEnumerable<StockTick> _ticks;
        private int _itemToDrasticUpdate = 0;
        public StockSimulator(StockTicker ticker)
        {
            _ticker = ticker;
            _ticks = new[]
            {
                new StockTick() {QuoteSymbol = "MSFT", Price = 53.49M},
                new StockTick() {QuoteSymbol = "INTC", Price = 32.68M},
                new StockTick() {QuoteSymbol = "ORCL", Price = 41.48M},
                new StockTick() {QuoteSymbol = "CSCO", Price = 28.33M},
            };
        }

        public void Run()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    UpdatePrices();
                    PrintPrices();
                    Emit();
                    Thread.Sleep(2000);
                }
            });
        }

        private void Emit()
        {
            Console.WriteLine("Emitting...");
            foreach (var stockTick in _ticks)
            {
                _ticker.Notify(stockTick);
            }
        }

        private void PrintPrices()
        {
            Console.WriteLine("Next series to emit:");
            Console.WriteLine("\t");
            foreach (var stockTick in _ticks)
            {
                Console.WriteLine("{{{0} - {1}}}, ", stockTick.QuoteSymbol, stockTick.Price);
            }
            Console.WriteLine();

        }

        private void UpdatePrices()
        {
            _ticks = _ticks.Select((tick, i) =>
              {
                  var changePercentage = _itemToDrasticUpdate == i ? 1.2M : 1.1M;
                  return new StockTick() { Price = tick.Price * changePercentage, QuoteSymbol = tick.QuoteSymbol };
              }).ToList();

            _itemToDrasticUpdate++;
            _itemToDrasticUpdate %= _ticks.Count();
        }
    }
}