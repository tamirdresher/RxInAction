using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
            //AddingATimestampToNotifications();
            //AddingTimeIntervalBetweenNotifications();
            //UsingTimeInterval();
            //UsingTimeout();
            //DelayingNotifications();
            //DelayingEachNotificationIndependently();
            //Throttling();
            //VariableThrottling();
            SamplingTheObservable();
        }



        public static void SamplingTheObservable()
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
              .Return("Update A")
              .Concat(Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "Update B"))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "Update C"))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "Update D"))
              .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "Update E"));

            observable.Throttle(TimeSpan.FromSeconds(2))
                .RunExample("Throttle");
        }

        private static void VariableThrottling()
        {
            Demo.DisplayHeader("Throttle operator - each element can define the throttling duration independantly by providing a function that creates an observable");

var observable = Observable
    .Return("Update A")
    .Concat(Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "Update B"))
    .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "Immediate Update"))
    .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "Update D"))
    .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "Update E"));

observable
    .Throttle(x => x == "Immediate Update"
        ? Observable.Empty<long>()
        : Observable.Timer(TimeSpan.FromSeconds(2)))
    .RunExample("Variable Throttling");


        }

        private static void DelayingNotifications()
        {
            Demo.DisplayHeader("Delay operator - shifts the observable sequence by a timespan");

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
        private static void DelayingEachNotificationIndependently()
        {
            Demo.DisplayHeader("Throttle operator - each notification can be shifted independently by providing observable that signals the delay is over");

            var observable = ObservableEx.FromValues(4, 1, 2, 3);

            observable
                .Timestamp()
                .Delay(x => Observable.Timer(TimeSpan.FromSeconds(x.Value)))
                .Timestamp()
                .RunExample("Independent Delay");
        }
        public static void UsingTimeout()
        {
            Demo.DisplayHeader("Timeout operator - enforcing a timeout if no notification was made in a period of time");

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
            IObservable<long> deviceHeartbeat = 
                Observable.Interval(TimeSpan.FromSeconds(1));

            deviceHeartbeat
                 .Take(3)
                 .Timestamp()
                 .RunExample("Heartbeat");
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
