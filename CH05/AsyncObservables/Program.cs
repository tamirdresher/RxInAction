using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace AsyncObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uncomment the example you wish to run

            //SearchingWithAsyncAwait();
            //SearchingWithCancellation();
            //SearchingWithConcatingTasks();
            //SearchingWithDefferedAsync();

            RunningAsyncCodeInWhere();
            Console.ReadLine();
        }

        private static void RunningAsyncCodeInWhere()
        {
            var svc = new PrimeCheckService();
            //
            // this wont Compile
            //
            //var subscription = Observable.Range(1, 10)
            //    .Where(async x => await svc.IsPrimeAsync(x))
            //    .SubscribeConsole("AsyncWhere");
            IObservable<int> primes;
            primes = from number in Observable.Range(1, 10)
                     from isPrime in svc.IsPrimeAsync(number)
                     where isPrime
                     select number;

            primes =
                Observable.Range(1, 10)
                    .SelectMany((number) => svc.IsPrimeAsync(number),
                                 (number, isPrime) => new { number, isPrime })
                    .Where(x => x.isPrime)
                    .Select(x => x.number);


            primes =
                Observable.Range(1, 10)
                    .Select(async (number) => new {number, IsPrime = await svc.IsPrimeAsync(number)})
                    .Concat()
                    .Where(x => x.IsPrime)
                    .Select(x => x.number);

            primes.SubscribeConsole("primes");
        }

        private class PrimeCheckService
        {
            public async Task<bool> IsPrimeAsync(int number)
            {
                return await Task.Run(async () =>
                 {
                     if (number == 3)
                     {
                         await Task.Delay(2000);
                     }
                     for (int j = 2; j <= Math.Sqrt(number); j++)
                     {
                         if (number % j == 0)
                         {
                             return false;
                         }
                     }
                     return true;
                 });
            }
        }

        public static void SearchingWithAsyncAwait()
        {
            Console.WriteLine();
            Console.WriteLine("----- Creating async observable with async-await ----");

            var results = SearchEngineExample.Search_WithAsyncAwait("Rx");
            int i = 0;
            var subscription = Disposable.Empty;
            subscription =
                results.SubscribeConsole("results");
        }

        public static void SearchingWithCancellation()
        {
            Console.WriteLine();
            Console.WriteLine("----- Creating async observable with async-await and cancellation----");


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
                    }
                })
                .Select(x => x.result) //rollback the observable to be IObservable<string> 
                .SubscribeConsole("results");

        }

        public static void SearchingWithConcatingTasks()
        {
            Console.WriteLine();
            Console.WriteLine("----- Converting Tasks to observables ----");

            var results = SearchEngineExample.Search_ConcatingTasks("Rx");
            int i = 0;
            var subscription = Disposable.Empty;
            subscription =
                results.SubscribeConsole("results");
        }

        public static void SearchingWithDefferedAsync()
        {
            Console.WriteLine();
            Console.WriteLine("----- Defferd async ----");

            var results = SearchEngineExample.Search_DefferedConcatingTasks("Rx");
            int i = 0;
            var subscription = Disposable.Empty;
            subscription =
                results.SubscribeConsole("results");
        }
    }
}
