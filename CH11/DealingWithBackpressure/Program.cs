using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

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
            var speedValues = new[] { 50,51,53,52,55 };

            var heartRates = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => heartRatesValues[x % heartRatesValues.Length]);
            var speeds = Observable.Interval(TimeSpan.FromSeconds(3)).Select(x => speedValues[x % speedValues.Length]);

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
            var fast = Observable.Interval(TimeSpan.FromSeconds(1));
            var slow = Observable.Interval(TimeSpan.FromSeconds(2));

            var zipped = slow.Zip(fast, (x, y) => x);
            var subscription =
                zipped
                    .Select(x => string.Format("{0} elements are in memory", x))
                    .SubscribeConsole("Backpressure");

            Console.ReadLine();
            subscription.Dispose();
        }
    }
}
