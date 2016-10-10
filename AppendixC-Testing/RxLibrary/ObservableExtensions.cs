using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace RxLibrary
{
    public static class ObservableExtensions
    {
        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, int burstSize)
        {
            return src.Window(burstSize).SelectMany(window => window.Take(1));
        }

        /// <summary>
        /// Emits the first value from every burst of items. 
        /// a burst is a sequence of elements from an observable sequence which are followed by another element within a specified relative time duration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">Source sequence to throttle.</param>
        /// <param name="maximalDistance">The due-time after an emission that defines the end of a burst.</param>
        /// <returns>an observable sequence with the first value from every burst of items.</returns>
        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, TimeSpan maximalDistance)
        {
            return src.FilterBursts(maximalDistance, DefaultScheduler.Instance);
        }
        /// <summary>
        /// Emits the first value from every burst of items. 
        /// a burst is a sequence of elements from an observable sequence which are followed by another element within a specified relative time duration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">Source sequence to throttle.</param>
        /// <param name="maximalDistance">The due-time after an emission that defines the end of a burst.</param>
        /// <param name="scheduler">Scheduler to run the throttle timers on.</param>
        /// <returns></returns>
        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, TimeSpan maximalDistance,
            IScheduler scheduler)
        {
            return src.Publish(xs =>
            {
                var windowBoundaries = xs.Throttle(maximalDistance, scheduler);

                 return xs.Window(windowBoundaries).SelectMany(window => window.Take(1));
             });
        }

        
        public static IObservable<T> FilterBursts<T>(this IObservable<T> src,
            TimeSpan maximalDistance,
            TimeSpan maximalBurstDuration,
            IScheduler scheduler)
        {
            return src.Publish(xs =>
            {
                var maximDurationPassed = xs.Delay(maximalBurstDuration, scheduler).Take(1);
                var windowBoundary = xs.Throttle(maximalDistance, scheduler).Merge(maximDurationPassed);

                return xs.Window(() => windowBoundary).SelectMany(window => window.Take(1));
            });
        }
    }
}
