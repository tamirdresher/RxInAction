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
            UsingInterval();
            UsingTimeInterval();
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

        private static void UsingTimeInterval()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using TimeInterval");


            IObservable<TimeInterval<long>> observable =
                Observable
                .Interval(TimeSpan.FromSeconds(1))
                .TimeInterval()
                .Do(_ => Thread.Sleep(TimeSpan.FromSeconds(3)));

            var subscription =
                observable.SubscribeConsole("time interval");

            Console.WriteLine("press enter to unsubsribe");
            Console.ReadLine();

            subscription.Dispose();

            Console.WriteLine("subscription disposed, press enter to continue");
            Console.ReadLine();
        }
    }
}
