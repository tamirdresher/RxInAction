using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxLibrary
{
    public class MachineMonitor
    {
        private readonly IConcurrencyProvider _concurrencyProvider;
        private readonly ITemperatureSensor _temperatureSensor;
        private readonly IProximitySensor _proximitySensor;
        public const int MAXIMAL_ALERT_BURST_TIME_IN_SECONDS = 5;
        public const int MINIMAL_ALERT_PAUSE_IN_SECONDS = 5;
        public const int MAXIMAL_TIME_WITH_NO_MOVEMENT_IN_SECONDS = 1;
        public const double RISKY_TEMPERATURE = 70;
        public MachineMonitor(
            IConcurrencyProvider concurrencyProvider,
            ITemperatureSensor temperatureSensor, 
            IProximitySensor proximitySensor)
        {
            _concurrencyProvider = concurrencyProvider;
            _temperatureSensor = temperatureSensor;
            _proximitySensor = proximitySensor;
        }

        /// <summary>
        /// The maximal amount of time we consider a sequence of notifications as a burst
        /// even if the notifications are emitted close to each other, after this amount 
        /// of time a burst is "closed"
        /// </summary>
        public TimeSpan MaxAlertBurstTime { get; set; } = TimeSpan.FromSeconds(MAXIMAL_ALERT_BURST_TIME_IN_SECONDS);
        /// <summary>
        /// the amount of time we allow between two consecutive alerts. 
        /// if two alerts are notified with a short time between them, we consider them as one 
        /// </summary>
        public TimeSpan MinAlertPause { get; set; } = TimeSpan.FromSeconds(MINIMAL_ALERT_PAUSE_IN_SECONDS);

        /// <summary>
        /// if no proximity notification is emitted during this time, we conclude that 
        /// there is no one near the sensor
        /// </summary>
        public TimeSpan MaximalTimeWithNoMovementInSeconds { get; set; } = TimeSpan.FromSeconds(MAXIMAL_TIME_WITH_NO_MOVEMENT_IN_SECONDS);

        public IObservable<Alert> ObserveAlerts()
        {
            return Observable.Defer(() =>
            {
                var scheduler = _concurrencyProvider.TimeBasedOperations;
                var proximities = _proximitySensor.Readings.Publish().RefCount();
                var temperatures = _temperatureSensor.Readings.Replay(1).RefCount();

                var riskyTempaeatures = temperatures.Where(t => t >= RISKY_TEMPERATURE);
                var proximityWindowBoundaries = proximities.Throttle(MaximalTimeWithNoMovementInSeconds);

                
                return (from proximityWindows in proximities.Window(proximityWindowBoundaries)
                        from t in proximityWindows.CombineLatest(riskyTempaeatures, (p, t) => t)
                        select t)
                    .FilterBursts(MinAlertPause, MaxAlertBurstTime, scheduler)
                    .Select(_ => new Alert("Temperature is too hot! Please move away", scheduler.Now));
            });
        }
    }
}
