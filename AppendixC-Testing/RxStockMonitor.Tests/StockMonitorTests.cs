using System;
using Xunit;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using NSubstitute;

namespace RxStockMonitor.Tests
{
    public class StockMonitorTests:ReactiveTest
    {
        [Fact]
        public void TwoStockTicksWithSmallChangeAndOneWithDrasticChange_OnlyOneDrasticChangeEmitted()
        {
            
            var testScheduler = new TestScheduler();
            var testableObserver = testScheduler.CreateObserver<DrasticChange>();
            var stockTicker = Substitute.For<IStockTicker>();
            var rxStockMonitor = new StockMonitor(stockTicker);

            testScheduler.Schedule(TimeSpan.FromTicks(1), () => { stockTicker.StockTick += Raise.Event<EventHandler<StockTick>>(stockTicker,new StockTick{Price=100,QuoteSymbol="MSFT"}); });
            testScheduler.Schedule(TimeSpan.FromTicks(2), () => { stockTicker.StockTick += Raise.Event<EventHandler<StockTick>>(stockTicker, new StockTick{Price=101,QuoteSymbol="MSFT"}); });
            testScheduler.Schedule(TimeSpan.FromTicks(3), () => { stockTicker.StockTick += Raise.Event<EventHandler<StockTick>>(stockTicker, new StockTick{Price=200,QuoteSymbol="MSFT"}); });

            rxStockMonitor.DrasticChanges.Subscribe(testableObserver);

            testScheduler.Start();

            testableObserver.Messages.AssertEqual(
                OnNext(3,(DrasticChange drasticChange)=>drasticChange.Symbol=="MSFT")
                );

        }
    }
   
}
