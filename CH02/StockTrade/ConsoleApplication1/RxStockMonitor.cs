using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstRxExample
{
class RxStockMonitor : IDisposable
{
    private IDisposable _subscription;

    public RxStockMonitor(StockTicker ticker)
    {
        const decimal maxChangeRatio = 0.1m;

        var ticks = Observable.FromEventPattern<EventHandler<StockTick>, StockTick>(
            h => ticker.StockTick += h, //#A
            h => ticker.StockTick -= h) //#B
            .Select(tickEvent => tickEvent.EventArgs)
            .Synchronize();

        IObservable<IGroupedObservable<string, StockTick>> groupedTicks =
            ticks.GroupBy(tick => tick.QuoteSymbol); 
        var drasticChanges =
            from tick in ticks
            group tick by tick.QuoteSymbol
            into company
            from tickPair in company.Buffer(2, 1)
            let changeRatio = Math.Abs((tickPair[1].Price - tickPair[0].Price)/tickPair[0].Price)
            where changeRatio > maxChangeRatio
            select new
            {
                Symbol = company.Key,
                ChangeRatio = changeRatio,
                OldPrice = tickPair[0].Price,
                NewPrice = tickPair[1].Price
            };

        _subscription =
            drasticChanges.Subscribe(change =>
                {
                    Console.WriteLine("Stock:{0} has changed with {1} ratio, Old Price:{2} New Price:{3}", change.Symbol,
                        change.ChangeRatio,
                        change.OldPrice,
                        change.NewPrice);
                },
                ex => { /* code that handles erros */}, //#C
                () =>{/* code that handles the observable completenss */}); //#C
    }
    
    public void Dispose()
    {
        _subscription.Dispose();
    }
}
}
