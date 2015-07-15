using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CreatingObservables.Chat;
using Helpers;

namespace EnumerablesToObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            EnumerableToObservable();
            SubscribingToEnumerable();
            ThrowingEnumerable();
            MergingObservableConnectionWithLoadedMessages();
            ObservableToEnumerable();
            Console.ReadLine();
        }

        private static void ObservableToEnumerable()
        {
            Console.WriteLine();
            Console.WriteLine("----- Observable To Enumerable ----");

var observable =
    Observable.Create<string>(o =>
    {
        o.OnNext("Observable");
        o.OnNext("To");
        o.OnNext("Enumerable");
        //  comment the call to OnCompleted() to see the thread wait
        o.OnCompleted();
        return Disposable.Empty;
    });

var enumerable = observable.ToEnumerable();
foreach (var item in enumerable)
{
    Console.WriteLine(item);
}
        }

        private static void EnumerableToObservable()
        {
            Console.WriteLine("----- Enumerable to Observable ----");

            IEnumerable<string> names = new[] { "Shira", "Yonatan", "Gabi", "Tamir" };
            IObservable<string> observable = names.ToObservable();

            observable.SubscribeConsole("names");
        }

        private static void SubscribingToEnumerable()
        {
            Console.WriteLine();
            Console.WriteLine("----- Subscribing to enumerable ----");

            IEnumerable<string> names = new[] { "Shira", "Yonatan", "Gabi", "Tamir" };
            names.Subscribe(new ConsoleObserver<string>("subscribe"));
        }

        private static void ThrowingEnumerable()
        {
            //this shows that exception that happen while iterating are sent to the OnError
            NumbersAndThrow()
                .ToObservable()
                .SubscribeConsole("enumerable with exception");
        }

        static IEnumerable<int> NumbersAndThrow()
        {
            Console.WriteLine();
            Console.WriteLine("----- Numbers and Throw ----");
            yield return 1;
            yield return 2;
            yield return 3;
            throw new ApplicationException("Something Bad Happened");
            yield return 4;
        }

        static void MergingObservableConnectionWithLoadedMessages()
        {
            Console.WriteLine();
            Console.WriteLine("----- Merging ObservableConnection with loaded messages ----");

            ChatClient client = new ChatClient();
            IObservable<string> liveMessages = client.ObserveMessages("user", "pass");
            IEnumerable<string> loadedMessages = LoadMessagesFromDB();

            loadedMessages.ToObservable()
                .Concat(liveMessages)
                .SubscribeConsole("merged");

            //this another way to do the same
            liveMessages.
                StartWith(loadedMessages)
                .SubscribeConsole("loaded first");

            client.NotifyRecieved("live message1");
            client.NotifyRecieved("live message2");
        }

        private static IEnumerable<string> LoadMessagesFromDB()
        {
            yield return "loaded1";
            yield return "loaded2";
        }
    }
}
