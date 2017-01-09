using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using AsyncObservables.SearchEngine;
using AsyncObservables.Services;
using Helpers;

namespace AsyncObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uncomment the example you wish to run

            SearchingWithAsyncAwait();
            SearchingWithCancellation();
            SearchingWithConcatingTasks();
            SearchingWithDefferedAsync();

            RunningAsyncCodeInWhere();
            ContrlingOrderOfAsyncCode();
            Console.ReadLine();
        }

        private static void RunningAsyncCodeInWhere()
        {
            
            Demo.DisplayHeader("Running async code int the pipeline - order is not determenistic");

            var svc = new PrimeCheckService();

            //
            // this wont Compile
            //
            //var subscription = Observable.Range(1, 10)
            //    .Where(async x => await svc.IsPrimeAsync(x))
            //    .SubscribeConsole("AsyncWhere");

            //
            // This compiles and runs - query syntax
            //
            IObservable<int> primes =
                from number in Observable.Range(1, 10)
                from isPrime in svc.IsPrimeAsync(number)
                where isPrime
                select number;

            //
            // The same, but in methods chain
            //
            primes =
                Observable.Range(1, 10)
                    .SelectMany((number) => svc.IsPrimeAsync(number),
                                 (number, isPrime) => new { number, isPrime })
                    .Where(x => x.isPrime)
                    .Select(x => x.number);

            var exampleResetEvent = new AutoResetEvent(false);
            primes
                .DoLast(() => exampleResetEvent.Set(), delay: TimeSpan.FromSeconds(1))
                .SubscribeConsole("primes");

            exampleResetEvent.WaitOne();
        }

        private static void ContrlingOrderOfAsyncCode()
        {
            
            Demo.DisplayHeader("Contrling the order of async code with Concat");

            var resetEvent = new AutoResetEvent(false);

            Console.WriteLine("Using SelectMany wont maintain items order");
            var svc = new VariableTimePrimeCheckService(numberToDelay: 3);
            IObservable<int> primes =
                from number in Observable.Range(1, 10)
                from isPrime in svc.IsPrimeAsync(number)
                where isPrime
                select number;

            primes
                .DoLast(() => resetEvent.Set(), delay: TimeSpan.FromSeconds(1))
                .SubscribeConsole("primes - unordered");

            // Waiting for the previous example to finish
            resetEvent.WaitOne();

            Console.WriteLine("Using concat does enforce order");
            primes =
                Observable.Range(1, 10)
                    .Select(async (number) => new { number, IsPrime = await svc.IsPrimeAsync(number) })
                    .Concat()
                    .Where(x => x.IsPrime)
                    .Select(x => x.number);

            primes.SubscribeConsole("primes");

            // Waiting for the previous example to finish
            resetEvent.WaitOne();
        }



        public static void SearchingWithAsyncAwait()
        {
            Demo.DisplayHeader("Creating async observable with async-await");

            var results = SearchEngineExample.Search_WithAsyncAwait("Rx");
            results.RunExample("search async-await");
        }

        public static void SearchingWithCancellation()
        {
            Demo.DisplayHeader("Creating async observable with async-await and cancellation");

            var exampleResetEvent = new AutoResetEvent(false);

            // Change the index to when you want the subscription disposed
            int cancelIndex = 1;

            var results = SearchEngineExample.Search_WithCancellation("Rx");

            IDisposable subscription = Disposable.Empty;
            subscription = results
                .Select((result, index) => new { result, index }) //adding the item index to the notification
                .Do(x =>
                {
                    if (x.index == cancelIndex)
                    {
                        Console.WriteLine("Cancelling on index {0}", cancelIndex);
                        subscription.Dispose();
                        exampleResetEvent.Set();
                    }
                })
                .Select(x => x.result) //rollback the observable to be IObservable<string> 
                 .DoLast(() => exampleResetEvent.Set(), delay: TimeSpan.FromSeconds(1))
                .SubscribeConsole("results");

            exampleResetEvent.WaitOne();
        }

        public static void SearchingWithConcatingTasks()
        {
            
            Demo.DisplayHeader("Converting Tasks to observables");

            var results = SearchEngineExample.Search_ConcatingTasks("Rx");

            results
             .RunExample("tasks to observables");
        }

        public static void SearchingWithDefferedAsync()
        {
            
            Demo.DisplayHeader("Defferd async");

            var results = SearchEngineExample.Search_DefferedConcatingTasks("Rx");
            results.RunExample("defered");
        }
    }
}
