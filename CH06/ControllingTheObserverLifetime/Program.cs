using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace ControllingTheObserverLifetime
{
    class Program
    {
        static void Main(string[] args)
        {
            DelayingSubscription();
            DelayingSubscriptionOnlyStartsWhenSubscribing();

            UnsubscribingAtASpcificTime();
            TakeUntil();
            TakeUntilAStopMessage();

            SkipUntil();
            Skip();

            TakeWhileAndSkipWhile();
            RepeatByAmount();

            Do();
            ReusableLogWithDo();
            Console.ReadLine();
        }

        private static void ReusableLogWithDo()
        {
            Demo.DisplayHeader("A reusable log created with Do(...)");

            Observable.Range(1, 5)
                .Log("Range")
                .Where(x => x % 2 == 0)
                .Log("Where")
                .Select(x => x * 3)
                .SubscribeConsole("final");

        }

        private static void Do()
        {
            Demo.DisplayHeader("Adding side effects with Do(...)");

            Observable.Range(1, 5)
                .Do(x => { Console.WriteLine("{0} was emitted", x); })
                .Where(x => x % 2 == 0)
                .Do(x => { Console.WriteLine("{0} survived the Where()", x); })
                .Select(x => x * 3)
                .SubscribeConsole("final");

        }

        private static void RepeatByAmount()
        {
            Demo.DisplayHeader("Repeating an observable sequence");

            Observable.Range(1, 3)
                .Repeat(2)
                .SubscribeConsole("Repeat(2)");
        }

        private static void TakeWhileAndSkipWhile()
        {
            Demo.DisplayHeader("Skip while the number<2 and Take while number<7");
            Observable.Range(1, 10)
                .SkipWhile(x => x < 2)
                .TakeWhile(x => x < 7)
                .SubscribeConsole();
        }

        private static void Skip()
        {
            Demo.DisplayHeader("Skip(2) - skip two notifications");

            Observable.Range(1, 5)
                .Skip(2)
                .SubscribeConsole("Skip(2)");
        }

        private static void SkipUntil()
        {
            Demo.DisplayHeader("SkipUntil(observable) - start receiving notifications when the observervable emit a specific message");

            IObservable<string> messages =
                new[] { "First", "START", "Message1", "Message2", "LastMessage" }.ToObservable();

            IObservable<string> controlChannel = messages;

            messages.SkipUntil(controlChannel.Where(m => m == "START"))
                .SubscribeConsole();
        }

        private static void TakeUntilAStopMessage()
        {
            Demo.DisplayHeader("TakeUntil(observable) - stopping when the observervable emit a specific message");


            IObservable<string> messages = Observable.Range(1, 5)
                .Select(i => "Message" + i)
                .Concat(Observable.Return("STOP"))
                .Concat(Observable.Return("After Stop")); //the final message will not be oberved by the observer 

            IObservable<string> controlChannel = messages;//to keep it simple, the control channel is the same observable as the messages

            messages
                .TakeUntil(controlChannel.Where(m => m == "STOP"))
                .RunExample("TakeUnti(STOP)");
        }

        private static void TakeUntil()
        {
            Demo.DisplayHeader("TakeUntil(observable) - stopping when the observervable emit, after 5 seconds");


            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
                .Select(t => DateTimeOffset.Now)
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(5)))
                .RunExample("TakeUntil(observable)");
        }

        private static void UnsubscribingAtASpcificTime()
        {
            var unsbscriptionTime = DateTimeOffset.Now.AddSeconds(6);
            Demo.DisplayHeader("TakeUntil(time) - stopping at a spcific time: " + unsbscriptionTime);


            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
                .Select(t => DateTimeOffset.Now)
                .TakeUntil(unsbscriptionTime)
                .RunExample("TakeUntil(time)");

        }

        private static void DelayingSubscription()
        {
            Demo.DisplayHeader("Delaying the subscription for 5 seconds");

            Console.WriteLine("Creating subscription at {0}", DateTime.Now);
            Observable.Range(1, 5)
                .Timestamp()
                .DelaySubscription(TimeSpan.FromSeconds(5))
                .SubscribeConsole();

            Thread.Sleep(6000);
            Console.WriteLine("Done");
        }

        private static void DelayingSubscriptionOnlyStartsWhenSubscribing()
        {
            Demo.DisplayHeader("DelaySubscription - the delay starts only when the observer subscribes");

            Console.WriteLine("Creating the observable pipeline at {0}", DateTime.Now);
            var observable =
    Observable.Range(1, 5)
        .Timestamp()
        .DelaySubscription(TimeSpan.FromSeconds(5));

            Console.WriteLine("Sleeping for 2 seconds");
            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.WriteLine("Subscribing the Console, but the real subscription will happen only 5 seconds from now");
            Console.WriteLine("Creating subscription at {0}", DateTime.Now);

            observable.SubscribeConsole();

            Thread.Sleep(6000);
            Console.WriteLine("Done");
        }
    }
}
