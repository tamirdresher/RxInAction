using System;
using System.Reactive.Linq;

namespace WeakObservers
{
    public static class WeakObserverExtensions
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

        class WeakObserverProxy<T> : IObserver<T>
        {
            private IDisposable _subscriptionToSource;
            private readonly WeakReference<IObserver<T>> _weakObserver;

            public WeakObserverProxy(IObserver<T> observer)
            {
                this._weakObserver = new WeakReference<IObserver<T>>(observer);
            }

            internal void SetSubscription(IDisposable subscriptionToSource)
            {
                this._subscriptionToSource = subscriptionToSource;
            }

            void NotifyObserver(Action<IObserver<T>> action)
            {
                if (this._weakObserver.TryGetTarget(out IObserver<T> observer))
                {
                    action(observer);
                }
                else
                {
                    this._subscriptionToSource.Dispose();
                }
            }

            public void OnNext(T value)
            {
                this.NotifyObserver(observer => observer.OnNext(value));
            }

            public void OnError(Exception error)
            {
                this.NotifyObserver(observer => observer.OnError(error));
            }

            public void OnCompleted()
            {
                this.NotifyObserver(observer => observer.OnCompleted());
            }

            public IDisposable AsDisposable()
            {
                return this._subscriptionToSource;
            }
        }
    }
}
