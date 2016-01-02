using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace ObserveOnAndSubscribeOn
{
    class Program
    {
        static void Main(string[] args)
        {
            //SubscribeOnExample();
            //SubscribeOnConfusion();
            SubscribeOnAndObserveOn();
            
            Console.ReadLine();
        }

        public static void SubscribeOnAndObserveOn()
        {
            Demo.DisplayHeader("using SubscribeOn and ObserveOn together and their effect");

            ObservableEx.FromValues(0,1,2,3,4,5)
    .Take(3)
    .LogWithThread("A")
    .Where(x => x%2 == 0)
    .LogWithThread("B")
    .SubscribeOn(NewThreadScheduler.Default)
    .LogWithThread("C")
    .Select(x => x*x)
    .LogWithThread("D")
    .ObserveOn(TaskPoolScheduler.Default)
    .LogWithThread("E")
    .RunExample("squares by time");
        }

        private static void SubscribeOnConfusion()
        {
            Demo.DisplayHeader("SubscribeOn operator - running the unsubscrition on another schdeudler might be confusing since it can take long time to complete");

var eventLoopScheduler = new EventLoopScheduler();
var subscription =
    Observable.Interval(TimeSpan.FromSeconds(1))
        .Do(x => Console.WriteLine("Inside Do"))
        .SubscribeOn(eventLoopScheduler)
        .SubscribeConsole();

eventLoopScheduler.Schedule(1,
    (s, state) =>
    {
        Console.WriteLine("Before sleep");
        Thread.Sleep(TimeSpan.FromSeconds(3));
        Console.WriteLine("After sleep");
        return Disposable.Empty;

    });

subscription.Dispose();
            Console.WriteLine("Subscription disposed");


        }

        public static void SubscribeOnExample()
        {
            Demo.DisplayHeader("SubscribeOn operator - runs the observer subscription and unsubscription on the specified Scheduler");

            var observable =
    Observable.Create<int>(o =>
    {
        Thread.Sleep(TimeSpan.FromSeconds(5));
        o.OnNext(1);
        o.OnCompleted();
        return Disposable.Empty;
    });
            Console.WriteLine("Time of subscription: {0}", DateTime.Now);
            observable.SubscribeConsole("LongOperationA");
            Console.WriteLine("Time after subscription (without ObserveOn): {0}", DateTime.Now);

            var autoResetEvent = new AutoResetEvent(false);
            Console.WriteLine("Time of subscription: {0}", DateTime.Now);
            observable.SubscribeOn(NewThreadScheduler.Default)
                .Do(_ => { },
                    () => autoResetEvent.Set())
                .SubscribeConsole("LongOperationA");
            Console.WriteLine("Time after subscription (with ObserveOn): {0}", DateTime.Now);

            autoResetEvent.WaitOne();

        }
    }
}
