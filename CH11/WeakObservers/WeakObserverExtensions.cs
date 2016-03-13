using System;
using System.Reactive.Linq;

namespace WeakObservers
{
    public static class WeakObserverExtensions
    {
        public static IObservable<T> AsWeakObservable<T>(this IObservable<T> source)
        {
            return Observable.Create<T>(o =>
            {
                var weakObserverProxy = new WeakObserverProxy<T>(o);
                var subscription = source.Subscribe(weakObserverProxy);
                weakObserverProxy.SetSubscription(subscription);
                return weakObserverProxy.AsDisposable();
            });
        }

        class WeakObserverProxy<T>:IObserver<T>
        {
            private IDisposable _subscriptionToSource;
            private WeakReference<IObserver<T>> _weakObserver;

            public WeakObserverProxy(IObserver<T> observer)
            {
                _weakObserver = new WeakReference<IObserver<T>>(observer);
            }

            internal void SetSubscription(IDisposable subscriptionToSource)
            {
                _subscriptionToSource = subscriptionToSource;
            }

            void NotifyObserver(Action<IObserver<T>> action)
            {
                IObserver<T> observer;
                if (_weakObserver.TryGetTarget(out observer))
                {
                    action(observer);
                }
                else
                {
                    _subscriptionToSource.Dispose();
                }
            }
            public void OnNext(T value)
            {
                NotifyObserver(observer=>observer.OnNext(value));
            }

            public void OnError(Exception error)
            {
                NotifyObserver(observer => observer.OnError(error));
            }

            public void OnCompleted()
            {
                NotifyObserver(observer => observer.OnCompleted());
            }
            
            public IDisposable AsDisposable()
            {
                return _subscriptionToSource;
            }
        }

        
    }
}