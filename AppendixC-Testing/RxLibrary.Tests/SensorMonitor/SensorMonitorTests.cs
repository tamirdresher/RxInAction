using Microsoft.Reactive.Testing;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using Xunit;

namespace RxLibrary.Tests
{
    public class SensorMonitorTests : ReactiveTest
    {
        [Fact]
        public void BurstOverFiveSeconds_TemperatureIsRisky_AlertAtBurstStartAndAfterFiveSeconds()
        {
            var testScheduler = new TestScheduler();
            var oneSecond = TimeSpan.TicksPerSecond;

            ITestableObservable<double> temperatures = testScheduler.CreateHotObservable<double>(
                OnNext(310, 500.0)
            );
            ITestableObservable<Unit> proximities = testScheduler.CreateHotObservable<Unit>(
                OnNext(100, Unit.Default),
                OnNext(1 * oneSecond - 1, Unit.Default),
                OnNext(2 * oneSecond - 1, Unit.Default),
                OnNext(3 * oneSecond - 1, Unit.Default),
                OnNext(4 * oneSecond - 1, Unit.Default),
                OnNext(5 * oneSecond - 1, Unit.Default),

                OnNext(6 * oneSecond - 1, Unit.Default)

            );
            IConcurrencyProvider concurrencyProvider = Substitute.For<IConcurrencyProvider>();
            concurrencyProvider.ReturnsForAll<IScheduler>(testScheduler);
            ITemperatureSensor tempSensor = Substitute.For<ITemperatureSensor>();
            tempSensor.Readings.Returns(temperatures);
            IProximitySensor proxSensor = Substitute.For<IProximitySensor>();
            proxSensor.Readings.Returns(proximities);

            var monitor = new MachineMonitor(concurrencyProvider, tempSensor, proxSensor);

            ITestableObserver<Alert> res = testScheduler.Start(() => monitor.ObserveAlerts(),
                0,
                0,
                Int64.MaxValue);

            res.Messages.AssertEqual(
                OnNext(310, (Alert a) => a.Time.Ticks == 310),
                OnNext(6 * oneSecond - 1, (Alert a) => true)
            );
        }
    }
}
