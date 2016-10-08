using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxStockMonitor
{
    public class StockMonitor
    {

        public StockMonitor(IStockTicker ticker)
        {
            const decimal maxChangeRatio = 0.1m;

            //creating an observable from the StockTick event, each notification will carry only the eventargs and will be synchronized
            IObservable<StockTick> ticks =
                    Observable.FromEventPattern<EventHandler<StockTick>, StockTick>(
                        h => ticker.StockTick += h,
                        h => ticker.StockTick -= h)
                        .Select(tickEvent => tickEvent.EventArgs)
                        .Synchronize();

            DrasticChanges =
                from tick in ticks
                group tick by tick.QuoteSymbol
                into company
                from tickPair in company.Buffer(2, 1)
                let changeRatio = Math.Abs((tickPair[1].Price - tickPair[0].Price) / tickPair[0].Price)
                where changeRatio > maxChangeRatio
                select new DrasticChange()
                {
                    Symbol = company.Key,
                    ChangeRatio = changeRatio,
                    OldPrice = tickPair[0].Price,
                    NewPrice = tickPair[1].Price
                };

        }

        public IObservable<DrasticChange> DrasticChanges { get; }

    }

    public class DrasticChange
    {
        public decimal NewPrice { get; set; }
        public string Symbol { get; set; }
        public decimal ChangeRatio { get; set; }
        public decimal OldPrice { get; set; }
    }
}
