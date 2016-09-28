using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace RxLibrary
{
    public class Teststst { }
    public static class ObservableExtensions
    {
        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, int burstSize)
        {
            return src.Window(burstSize).SelectMany(window => window.Take(1));
        }

        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, TimeSpan burstDuration)
        {
            return src.FilterBursts(burstDuration, DefaultScheduler.Instance);
        }

        public static IObservable<T> FilterBursts<T>(this IObservable<T> src, TimeSpan burstDuration,
            IScheduler scheduler)
        {
            return src.Publish(xs =>
            {
                var windowBoundaries = xs.Throttle(burstDuration, scheduler);

                 return xs.Window(windowBoundaries).SelectMany(window => window.Take(1));
             });
        }
    }
}
