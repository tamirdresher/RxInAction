using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;
using MagicalPrimeGeneratorExample;

namespace Schedulers
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicScheduling();
            BasicSchedulingEveryTwoSeconds();
            ObservableIntervalOnCurrentThread();
            ParametrizedConcurrency();
            TypesOfSchedulersExamples.Run();

            FixedGeneratePrimesFromChapter5();
            Console.WriteLine("Press <Enter> to continue...");
            Console.ReadLine();
        }

      
        private static void BasicScheduling()
        {
            Demo.DisplayHeader("Basic Scheduling - Scheduling an action to run after two seconds");

            IScheduler scheduler = NewThreadScheduler.Default;

            IDisposable scheduling =
                scheduler.Schedule(
                    Unit.Default,
                    TimeSpan.FromSeconds(2),
                    (scdlr, _) =>
                    {
                        Console.WriteLine("Hello World, Now: {0}", scdlr.Now);
                        return Disposable.Empty;
                    });

            Console.WriteLine("sleeping for 3 seconds so the scheduling will take place");
            Thread.Sleep(TimeSpan.FromSeconds(3));

        }

        private static void BasicSchedulingEveryTwoSeconds()
        {
            Demo.DisplayHeader("Basic Scheduling - Scheduling an action to run recursively every two seconds");

            IScheduler scheduler = NewThreadScheduler.Default;
            Func<IScheduler, int, IDisposable> action = null;
            action = (scdlr, callNumber) =>
            {
                Console.WriteLine("Hello {0}, Now: {1}, Thread: {2}", callNumber, scdlr.Now,
                    Thread.CurrentThread.ManagedThreadId);
                return scdlr.Schedule(callNumber + 1, TimeSpan.FromSeconds(2), action);
            };
            var scheduling =
                scheduler.Schedule(
                    0,
                    TimeSpan.FromSeconds(2),
                    action);

            Console.WriteLine("sleeping for 5 seconds and then disposing");

            Thread.Sleep(TimeSpan.FromSeconds(5));
            scheduling.Dispose();

            Console.WriteLine("scheduling disposed, Now: {0}", scheduler.Now);
        }

        private static void ObservableIntervalOnCurrentThread()
        {
            Demo.DisplayHeader(
                "Parametrizing concurrency - passing the CurrentThreadScheduler to the Interval operator so the emissions will be on the calling thread");

            Console.WriteLine("Before - Thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Observable.Interval(TimeSpan.FromSeconds(1), CurrentThreadScheduler.Instance)
                .Timestamp()
                .Take(3)
                .Do(x => Console.WriteLine("Inside - {0} - Thread: {1}", x, Thread.CurrentThread.ManagedThreadId))
                .RunExample();
        }

        public static void ParametrizedConcurrency()
        {
            Demo.DisplayHeader(
                "Parametrizing concurrency - passing the NewThreadScheduler to the Range operator so the emissions will be on another thread");

            var subscription =
                Observable.Range(1, 5, NewThreadScheduler.Default)
                    //without passing the scheduler, this will run infinitely
                    .Repeat()
                    .SubscribeConsole("Range on another thread");
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            subscription.Dispose();
        }

        private static void FixedGeneratePrimesFromChapter5()
        {
            Demo.DisplayHeader(
                "Using schedulers in your observable generators (or operators) - fixing the PrimesGenerator from chapter 5");

            GeneratePrimes(20, TaskPoolScheduler.Default)
                    .RunExample("primes observable");
            
        }
        public static IObservable<int> GeneratePrimes(int amount, IScheduler schdeuler = null)
        {
            schdeuler = schdeuler ?? DefaultScheduler.Instance;
            return Observable.Create<int>(o =>
            {
                var cancellation = new CancellationDisposable();
                var scheduledWork = schdeuler.Schedule(() =>
                {
                    try
                    {
                        var magicalPrimeGenerator = new MagicalPrimeGenerator();
                        foreach (var prime in magicalPrimeGenerator.Generate(amount))
                        {
                            cancellation.Token.ThrowIfCancellationRequested();
                            o.OnNext(prime);
                        }
                        o.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        o.OnError(ex);
                    }
                });
                return new CompositeDisposable(scheduledWork, cancellation);
            });
        }


    }
}
