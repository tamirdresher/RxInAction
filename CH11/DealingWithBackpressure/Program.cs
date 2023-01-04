using Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace DealingWithBackpressure
{
    class Program
    {
        static void Main(string[] args)
        {
            BackpressureExample();
            LossyBackpressureHandlingUsingCombineLatest();
            Console.ReadLine();
        }

        private static void LossyBackpressureHandlingUsingCombineLatest()
        {
            Demo.DisplayHeader("Backpressure example - using CombineLatest to drop old notifications (lossy approach)");

            var heartRatesValues = new[] { 70, 75, 80, 90, 80 };
            var speedValues = new[] { 50, 51, 53, 52, 55 };

            IObservable<int> heartRates = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => heartRatesValues[x % heartRatesValues.Length]);
            IObservable<int> speeds = Observable.Interval(TimeSpan.FromSeconds(3)).Select(x => speedValues[x % speedValues.Length]);

            heartRates.CombineLatest(speeds, (h, s) => String.Format("Heart:{0} Speed:{1}", h, s))
                .Take(5)
                .SubscribeConsole();
        }

        private static void BackpressureExample()
        {
            Demo.DisplayHeader("Backpressure example - Zipping a fast observable with a slow observable");
            Console.WriteLine("Press <enter> to stop the example");

            //Zipping a fast observable with a slow observable will have to store
            //the elements from the fast observable in memory
            IObservable<long> fast = Observable.Interval(TimeSpan.FromSeconds(1));
            IObservable<long> slow = Observable.Interval(TimeSpan.FromSeconds(2));

            IObservable<long> zipped = slow.Zip(fast, (x, y) => x);
            IDisposable subscription =
                zipped
                    .Select(x => String.Format("{0} elements are in memory", x))
                    .SubscribeConsole("Backpressure");

            Console.ReadLine();
            subscription.Dispose();
        }
    }
}
