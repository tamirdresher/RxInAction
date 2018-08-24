using FakeItEasy;
using Microsoft.Reactive.Testing;
using System;
using System.Reactive.Linq;
using Xunit;

namespace YourFirstRxExample.Tests
{
    public class StockMonitorTests
    {
        [Fact]
        public void Bla()
        {
            System.Reactive.IEventSource<StockTick> stockTicks = new[]
            {
                new StockTick() {QuoteSymbol = "MSFT", Price = 53.49M},
                new StockTick() {QuoteSymbol = "INTC", Price = 32.68M},
                new StockTick() {QuoteSymbol = "ORCL", Price = 41.48M},
                new StockTick() {QuoteSymbol = "CSCO", Price = 28.33M},
            }.ToObservable().ToEvent();
            var testScheduler = new TestScheduler();
            ITestableObserver<DrasticChange> testableObserver = testScheduler.CreateObserver<DrasticChange>();
            var stockTicker = A.Fake<IStockTicker>();
            A.CallTo(() => stockTicker.StockTick).
            var rxStockMonitor = new RxStockMonitor(stockTicker);

            rxStockMonitor.DrasticChanges.Subscribe(testableObserver);
        }
    }
}
