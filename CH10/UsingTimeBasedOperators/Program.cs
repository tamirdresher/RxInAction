using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace UsingTimeBasedOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            AddingATimestampToNotifications();
            AddingTimeIntervalBetweenNotifications();
            //UsingTimeInterval();
            UsingTimeout();
            //UsingDelay();
            //Throttling();
            SamplingTheObservable();
        }

        private static void SamplingTheObservable()
        {
            Demo.DisplayHeader("The Sample operator - samples the observable sequence every time-interval, emitting the last item in that interval");

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Sample(TimeSpan.FromSeconds(3.5))
                .Take(3)
                .RunExample("Sample");

            //Same as
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Sample(Observable.Timer(TimeSpan.FromSeconds(3.5), TimeSpan.FromSeconds(3.5)))
                .Take(3)
                .RunExample("Sample with sampler-observable");
        }

        private static void Throttling()
        {
            Demo.DisplayHeader("Throttle operator - only emit an item from an Observable if a particular timespan has passed without it emitting another item");

            var observable = Observable
              .Return(1)
              .Concat(Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => 2))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 3))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 4))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => 5));

            observable.Throttle(TimeSpan.FromSeconds(2))
                .RunExample("Throttle");
        }

        private static void UsingDelay()
        {
            var observable = Observable
                .Timer(TimeSpan.FromSeconds(1))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)));

            observable
                .Timestamp()
                .Delay(TimeSpan.FromSeconds(2))
                .Timestamp()
                .Take(5)
                .RunExample("Delay");
        }

        public static void UsingTimeout()
        {
var observable = Observable
     .Timer(TimeSpan.FromSeconds(1))
     .Concat(Observable.Timer(TimeSpan.FromSeconds(1)))
     .Concat(Observable.Timer(TimeSpan.FromSeconds(4)))
     .Concat(Observable.Timer(TimeSpan.FromSeconds(4)));

observable
    .Timeout(TimeSpan.FromSeconds(3))
    .RunExample("Timeout");
        }

        private static void AddingATimestampToNotifications()
        {
            Demo.DisplayHeader("Timestamp operator - adds a timestamp to every notification");
            Observable
                 .Interval(TimeSpan.FromSeconds(1))
                 .Take(3)
                 .Timestamp()
                 .RunExample("Timestamp");
        }


        private static void UsingInterval()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using Interval");


            IObservable<long> observable = Observable
                .Interval(TimeSpan.FromSeconds(1));

            var subscription =
                observable.SubscribeConsole("interval");

            Console.WriteLine("press enter to unsubsribe");
            Console.ReadLine();

            subscription.Dispose();

            Console.WriteLine("subscription disposed, press enter to continue");
            Console.ReadLine();
        }

        private static void AddingTimeIntervalBetweenNotifications()
        {
            Console.WriteLine();
            Demo.DisplayHeader("TimeInterval operator - records the time interval between two consecutive notifications");

            var observable = Observable
                .Timer(TimeSpan.FromSeconds(1))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(2)))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)));
            
            observable
                .TimeInterval()
                .RunExample("time interval");


        }
    }
}
