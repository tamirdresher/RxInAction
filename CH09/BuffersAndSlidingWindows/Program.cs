using Helpers;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

namespace BuffersAndSlidingWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            //UsingBufferWithAmount();
            //BufferingHiRateChatMessages();
            //Window();
            AggreagateResultInAWindow();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        private static void UsingBufferWithAmount()
        {
            Demo.DisplayHeader("The Buffer operator - gather items from an Observable into bundles.");

            IObservable<double> speedReadings = new[] { 50.0, 51.0, 51.5, 53.0, 52.0, 52.5, 53.0 } //in MPH
                .ToObservable();

            var timeDelta = 0.0002777777777777778; //1 second in hours unit

            IObservable<double> accelrations =
                from buffer in speedReadings.Buffer(count: 2, skip: 1)
                where buffer.Count == 2
                let speedDelta = buffer[1] - buffer[0]
                select speedDelta / timeDelta;

            accelrations.RunExample("Acceleration");
        }

        private static void BufferingHiRateChatMessages()
        {
            Demo.DisplayHeader("The Buffer operator - can be used to slow high-rate stream by taking it by chunks");

            IObservable<string> coldMessages = Observable.Interval(TimeSpan.FromMilliseconds(50))
                .Take(4)
                .Select(x => "Message " + x);

            IObservable<string> messages =
                coldMessages.Concat(
                     coldMessages.DelaySubscription(TimeSpan.FromMilliseconds(200)))
                    .Publish()
                    .RefCount();

            messages.Buffer(messages.Throttle(TimeSpan.FromMilliseconds(100)))
                .SelectMany((b, i) => b.Select(m => String.Format("Buffer {0} - {1}", i, m)))
                .RunExample("Hi-Rate Messages");
        }

        private static void Window()
        {
            Demo.DisplayHeader("The Window operator - split the observable sequence into sub-observables along temporal boundaries");

            IObservable<long> numbers = Observable.Interval(TimeSpan.FromMilliseconds(50));
            IObservable<IObservable<long>> windows = numbers.Window(TimeSpan.FromMilliseconds(200));

            windows.Do(_ => Console.WriteLine("New Window:"))
                .Take(3)
                .SelectMany(x => x)
                .SubscribeConsole();
        }

        private static void AggreagateResultInAWindow()
        {
            Demo.DisplayHeader("The Window operator - each window is an observable that can be used with an aggregation function");

            IObservable<decimal> donationsWindow1 = new decimal[] { 50M, 55, 60 }.ToObservable(); // ObservableExtensionsHelpers.FromValues();
            IObservable<decimal> donationsWindow2 = new decimal[] { 49M, 48, 45 }.ToObservable();// ObservableExtensionsHelpers.FromValues(49M, 48, 45);

            IObservable<decimal> donations =
                donationsWindow1.Concat(donationsWindow2.DelaySubscription(TimeSpan.FromSeconds(1.5)));

            IObservable<IObservable<decimal>> windows = donations.Window(TimeSpan.FromSeconds(1));

            IObservable<decimal> donationsSums =
                from window in windows.Do(_ => Console.WriteLine("New Window"))
                from sum in window.Scan((prevSum, donation) => prevSum + donation)
                select sum;

            donationsSums.RunExample("donations in shift");
        }

        private static void ControllingTheWindowClosure()
        {
            var numbers = new Subject<int>();
            var mouseClicks = new Subject<Unit>();
            IObservable<IObservable<int>> windows = numbers.Window(() => mouseClicks);
        }
    }
}
