using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Extensions
    {
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
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



        public static void RunExample<T>(this IObservable<T> observable, string exampleName)
        {
            var exampleResetEvent = new AutoResetEvent(false);

            observable
                 .DoLast(() => exampleResetEvent.Set(), TimeSpan.FromSeconds(3))
                 .SubscribeConsole(exampleName);

            exampleResetEvent.WaitOne();

        }
    }
}
