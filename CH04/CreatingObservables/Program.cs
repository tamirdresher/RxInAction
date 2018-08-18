﻿using CreatingObservables.Chat;
using Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CreatingObservables {
    class Program {
        static void Main(string[] args) {
            HandcraftedObservable();
            EnforcingUnsubscribingObservers();
            UsingObservableCreate();
            ChatExample.Run();
            Console.ReadLine();
        }

        private static void UsingObservableCreate() {
            Console.WriteLine();
            Demo.DisplayHeader("Using the Create operator");

            IObservable<int> numbers = ObserveNumbers(5);

            numbers.SubscribeConsole("Observable.Created(...)");
        }

        public static IObservable<int> ObserveNumbers(int amount) {
            return Observable.Create<int>(observer => {
                for (var i = 0; i < amount; i++) {
                    observer.OnNext(i);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        private static void EnforcingUnsubscribingObservers() {
            Console.WriteLine();
            Demo.DisplayHeader("Enforcing The Observers Unsubscription (OnCompleted/OnError)");

            IObservable<int> errorTestObservable =
                new ErrorNumbersObservable(5);

            var consoleObserver = new ConsoleObserver<int>("errorTest");

            Console.WriteLine("the contract is not enforced in the manual observable");
            IDisposable subscription = errorTestObservable.Subscribe(consoleObserver);

            Console.WriteLine();
            Console.WriteLine("however, the Observable.Create(...) version does enforce");
            errorTestObservable =
                Observable.Create<int>(o => {
                    o.OnNext(1);
                    o.OnError(new ApplicationException());
                    o.OnNext(2);
                    o.OnCompleted();
                    o.OnNext(3);
                    return Disposable.Empty;
                });
            subscription = errorTestObservable.Subscribe(consoleObserver);
        }

        private static void HandcraftedObservable() {
            Console.WriteLine();
            Demo.DisplayHeader("Handcrafted Observable");

            var numbers = new NumbersObservable(5);
            IDisposable subscription =
                numbers.Subscribe(new ConsoleObserver<int>("numbers"));
        }
    }
}
