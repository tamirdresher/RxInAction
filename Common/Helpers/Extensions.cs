using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Subscribe an observer that prints each notificatio to the console output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="name"></param>
        /// <returns>a disposable subscription object</returns>
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }

        /// <summary>
        /// this method does the same as SubscribeConsole but uses Observable.Subscribe() method instead of a handcrafted observer class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDisposable SubscribeTheConsole<T>(this IObservable<T> observable, string name = "")
        {

            return observable.Subscribe(
                x => Console.WriteLine("{0} - OnNext({1})", name, x),
                ex =>
                {
                    Console.WriteLine("{0} - OnError:", name);
                    Console.WriteLine("\t {0}", ex);
                },
                () => Console.WriteLine("{0} - OnCompleted()", name));
        }

        /// <summary>
        /// Adds a log that prints to the console the notification emitted by the <paramref name="observable"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="msg">An optioanl prefix that will be added before each notification</param>
        /// <returns></returns>
        public static IObservable<T> Log<T>(this IObservable<T> observable, string msg = "")
        {
            return observable.Do(
                x => Console.WriteLine("{0} - OnNext({1})", msg, x),
                ex =>
                {
                    Console.WriteLine("{0} - OnError:", msg);
                    Console.WriteLine("\t {0}", ex);
                },
                () => Console.WriteLine("{0} - OnCompleted()", msg));
        }

        /// <summary>
        /// Logs the subscriptions and emissions done on/by the observable
        /// each log message also includes the thread it happens on
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
public static IObservable<T> LogWithThread<T>(this IObservable<T> observable, string msg = "")
{
    return Observable.Defer(() =>
     {
         Console.WriteLine("{0} Subscription happened on Thread: {1}", msg, 
                            Thread.CurrentThread.ManagedThreadId);

         return observable.Do(
             x => Console.WriteLine("{0} - OnNext({1}) Thread: {2}", msg, x,
                                        Thread.CurrentThread.ManagedThreadId),
             ex =>
             {
                 Console.WriteLine("{0} - OnError Thread:{1}", msg,
                                        Thread.CurrentThread.ManagedThreadId);
                 Console.WriteLine("\t {0}", ex);
             },
             () => Console.WriteLine("{0} - OnCompleted() Thread {1}", msg,
                                        Thread.CurrentThread.ManagedThreadId));
     });
}

        /// <summary>
        /// Runs a configureable action when the observable completes or emit error 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable">the source observable</param>
        /// <param name="lastAction">an action to perform when the source observable completes or has error</param>
        /// <param name="delay">the time span to wait before invoking the <paramref name="lastAction"/></param>
        /// <returns></returns>
        public static IObservable<T> DoLast<T>(this IObservable<T> observable, Action lastAction, TimeSpan? delay = null)
        {
            Action delayedLastAction = async () =>
            {
                if (delay.HasValue)
                {
                    await Task.Delay(delay.Value);
                }
                lastAction();
            };
            return observable.Do(
                (_) => { },//empty OnNext
                _ => delayedLastAction(),
                delayedLastAction);
        }



        public static void RunExample<T>(this IObservable<T> observable, string exampleName = "")
        {
            var exampleResetEvent = new AutoResetEvent(false);

            observable
                 .DoLast(() => exampleResetEvent.Set(), TimeSpan.FromSeconds(3))
                 .SubscribeConsole(exampleName);

            exampleResetEvent.WaitOne();

        }
    }
}
