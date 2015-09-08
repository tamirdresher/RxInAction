using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Helpers;

namespace PeriodicAndTimeBasedObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreatingPeriodicObservableWithInterval();
            //PeriodicallyGetUpdates();
            //UsingTimerToScheduleTheBeginning();
            //UsingTimerToScheduleWithRelativeTime();
            //UsingTimerToScheduleWithAbsoluteTime();
            ChangingTheUnderlyingObservableByTime();
            Console.ReadLine();
        }

        private static void ChangingTheUnderlyingObservableByTime()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Switching observables after 5 seconds (with timer)");

            IObservable<string> firstObservable =
                Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .Select(x => "first" + x);
            IObservable<string> secondObservable =
                Observable
                    .Interval(TimeSpan.FromSeconds(2))
                    .Select(x => "second" + x)
                    .Take(5);// we dont want to run the example forever, so we'll do only 5 emission;

            IObservable<IObservable<string>> immediateObservable = Observable.Return(firstObservable);

            //Scheduling the second observable emission
            IObservable<IObservable<string>> scheduledObservable =
                Observable
                    .Timer(TimeSpan.FromSeconds(5))
                    .Select(x => secondObservable);

            var switchingObservable=immediateObservable
                .Merge(scheduledObservable)
                .Switch()
                .Timestamp();


            Console.WriteLine("subscribing");
            switchingObservable.RunExample("timer switch");
        }

        private static void UsingTimerToScheduleTheBeginning()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating observable with Timer - emission will start 5 sec after the subscription, with a period of 1 sec between notification----");

            var observable = Observable
                .Timer(dueTime: TimeSpan.FromSeconds(5), //first emission
                        period: TimeSpan.FromSeconds(1))
                .Take(5);// we dont want to run the example forever, so we'll do only 3 emissions

            Console.WriteLine("subscribing");
            observable.RunExample("Timer(5s,1s)");
        }


        private static void CreatingPeriodicObservableWithInterval()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating observable with Interval");

            var observable = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Take(5);// we dont want to run the example forever, so we'll do only 3 emissions

            observable.RunExample("Interval");
        }

        /// <summary>
        /// Shows how to use the Interval operator to periodically poll a webservice for updates
        /// NOTE: a similar example for WPF GUI application can be found in the project "CreatingPeriodicUpdatableView" 
        /// </summary>
        private static void PeriodicallyGetUpdates()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using Interval to periodically poll a webservice");

            var updatesWebService = new UpdatesWebService();
            var observable = Observable
                .Interval(TimeSpan.FromSeconds(3))
                .Take(3) // we dont want to run the example forever, so we'll do only 3 updates
                .SelectMany(_ => updatesWebService.GetUpdatesAsync())
                .SelectMany(updates => updates);

            observable.RunExample("updates");

        }


        private static void UsingTimerToScheduleWithRelativeTime()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Schedule single emission with Timer (relative)----");

            Console.WriteLine("Scheduling to 5 sec from subscription");
            var observable = Observable
                .Timer(dueTime: TimeSpan.FromSeconds(5))
                .Timestamp();

            Console.WriteLine("subscribing at {0}", new Timestamped<string>("", DateTimeOffset.UtcNow));
            observable.RunExample("Timer (relative)");
        }



        private static void UsingTimerToScheduleWithAbsoluteTime()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Schedule single emission with Timer (absolute)----");

            var dateTimeOffset = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(5);

            Console.WriteLine("Scheduling to {0}", dateTimeOffset);
            var observable = Observable.Timer(dateTimeOffset)
                .Timestamp();

            Console.WriteLine("subscribing at {0}", new Timestamped<string>("", DateTimeOffset.UtcNow));
            observable.RunExample("Timer (relative)");
        }

    }
}
