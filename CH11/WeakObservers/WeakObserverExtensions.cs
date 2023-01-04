using System;
using System.Reactive.Linq;

namespace WeakObservers
{
    public static partial class WeakObserverExtensions
    {
        public static IObservable<T> AsWeakObservable<T>(this IObservable<T> source)
        {
            return Observable.Create<T>(o => {
                var weakObserverProxy = new WeakObserverProxy<T>(o);
                IDisposable subscription = source.Subscribe(weakObserverProxy);
                weakObserverProxy.SetSubscription(subscription);
                return weakObserverProxy.AsDisposable();
            });
        }
    }
}
