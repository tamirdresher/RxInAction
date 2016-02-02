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

            Console.ReadLine();
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
