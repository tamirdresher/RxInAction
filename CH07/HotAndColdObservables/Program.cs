using Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace HotAndColdObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            ColdObservableMultipleSubscriptionsExample().Wait();
            HeatingAnObservable();
            PublishWithInitialValue();
            PublishWithSelector();
            PublishLast();
            Reconnecting();
            RefCount();
            ReplayTwo();

            Console.ReadLine();
        }

        private static void ReplayTwo()
        {
            Demo.DisplayHeader("Replay(2) - will replay the last two items for the future subscribed observer");

            IConnectableObservable<long> observable = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Replay(2);
            observable.Connect();
            observable.SubscribeConsole("First");
            Thread.Sleep(3000); //3 seconds before subsribing the next observable
            Console.WriteLine("subscribing the second observable");
            observable.SubscribeConsole("Second");

            //waiting for the observable to complete
            observable.Wait();
        }

        private static void RefCount()
        {
            Demo.DisplayHeader("Automatic disconnection with RefCount");

            //unbounded observable
            IObservable<long> publishedObservable = Observable.Interval(TimeSpan.FromSeconds(1))
                .Do(x => Console.WriteLine("Generating {0}", x))
                .Publish()
                .RefCount();

            IDisposable subscription1 = publishedObservable.SubscribeConsole("First");
            IDisposable subscription2 = publishedObservable.SubscribeConsole("Second");

            //waiting 3 seconds before disposing one subscription
            Thread.Sleep(3000);
            Console.WriteLine("Disposing one subscription");
            subscription1.Dispose();

            //waiting 3 seconds before disposing the second subscription
            Thread.Sleep(3000);
            Console.WriteLine("Disposing the second subscription");
            subscription2.Dispose();
        }

        private static void Reconnecting()
        {
            Demo.DisplayHeader("Reconnecting a connectable observable");

            IConnectableObservable<string> connectableObservable =
                Observable.Defer(() => ChatServer.Current.ObserveMessages())
                    .Publish();

            connectableObservable.SubscribeConsole("Messages Screen");
            connectableObservable.SubscribeConsole("Messages Statistics");

            IDisposable subscription = connectableObservable.Connect();

            //After After the application was notified on server outage
            Console.WriteLine("--Disposing the current connection and reconnecting--");
            subscription.Dispose();
            subscription = connectableObservable.Connect();

            //waiting for the observable to complete
            Thread.Sleep(5000);
        }

        private static void Multicast()
        {
            IObservable<long> coldObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            IObservable<long> observable =
                coldObservable.Multicast(
                    subjectSelector: () => new Subject<long>(),
                    selector: src => src.Zip(src, (a, b) => a + b));

            observable.SubscribeConsole();
        }

        private static void PublishLast()
        {
            Demo.DisplayHeader("PublishLast - will emit the last value, even for observers that subscribe after completed");

            //an observable that simulate an asynchronous operation that take a long time to complete
            IObservable<string> coldObservable = Observable.Timer(TimeSpan.FromSeconds(5)).Select(_ => "Rx");

            Console.WriteLine("Creating hot disconncted observable");
            IConnectableObservable<string> connectableObservable = coldObservable.PublishLast();

            Console.WriteLine("Subscribing two observers now, and the third after the source observable completed");
            connectableObservable.SubscribeConsole("First");
            connectableObservable.SubscribeConsole("Second");

            Console.WriteLine("Connecting the observable");
            connectableObservable.Connect();

            //waiting 6 seconds before subscribing a third observer
            Thread.Sleep(6000);
            Console.WriteLine("Subscribing the third observer - it will receive the last value");
            connectableObservable.SubscribeConsole("Third");
        }

        private static void PublishWithInitialValue()
        {
            Demo.DisplayHeader("Publish(initial-value) - published the observable but using BehaviorSubject");

            IObservable<long> coldObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            Console.WriteLine("Creating hot disconncted observable");
            IConnectableObservable<long> connectableObservable = coldObservable.Publish(-10);

            Console.WriteLine("Subscribing two observers now, and the third in two more seconds");
            connectableObservable.SubscribeConsole("First");
            connectableObservable.SubscribeConsole("Second");

            Console.WriteLine("Connecting the observable");
            connectableObservable.Connect();

            //waiting two seconds before subscribing a third observer
            Thread.Sleep(2000);
            Console.WriteLine("Subscribing the third observer - it will receive the current value");
            connectableObservable.SubscribeConsole("Third");

            //waiting for the observable sequence to complete
            Thread.Sleep(3000);
        }

        private static void PublishWithSelector()
        {
            Demo.DisplayHeader("Publish(selector) - is good for reusing a published observable to create a new observable");

            var i = 0;
            IObservable<int> numbers = Observable.Range(1, 5).Select(_ => i++);//this observable causes a side effect

            //since the 'numbers' observable is cold, this will result in the sequence of values in the form i+(i+1) and not i+i
            IObservable<int> zipped = numbers.Zip(numbers, (a, b) => a + b);
            zipped.SubscribeConsole("zipped");

            Console.WriteLine("Zipping an observable to itself after publihsing it");
            i = 0;
            IObservable<int> publishedZip =
                numbers.Publish(published => published.Zip(published, (a, b) => a + b));
            publishedZip.SubscribeConsole("publishedZipped");
        }

        private static void HeatingAnObservable()
        {
            Demo.DisplayHeader("Heating an observable using Publish and Connect");

            IObservable<long> coldObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            Console.WriteLine("Creating hot disconncted observable");
            IConnectableObservable<long> connectableObservable = coldObservable.Publish();

            Console.WriteLine("Subscribing two observers now, and the third in two more seconds");
            connectableObservable.SubscribeConsole("First");
            connectableObservable.SubscribeConsole("Second");

            Console.WriteLine("Connecting the observable");
            connectableObservable.Connect();

            //waiting two seconds before subscribing a third observer
            Thread.Sleep(2000);
            Console.WriteLine("Subscribing the third observer - it will receive the next notification");
            connectableObservable.SubscribeConsole("Third");

            //waiting for the observable sequence to complete
            Thread.Sleep(3000);
        }

        public static async Task ColdObservableMultipleSubscriptionsExample()
        {
            Demo.DisplayHeader("Cold observable will regenerate the entire seqeunce of notfications for each observer");

            IObservable<string> coldObservable =
                Observable.Create<string>(async o => {
                    o.OnNext("Hello");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    o.OnNext("Rx");
                });

            coldObservable.SubscribeConsole("o1");
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            coldObservable.SubscribeConsole("o2");

            //waiting for the observable sequence to complete
            Thread.Sleep(3000);
        }
    }
}
