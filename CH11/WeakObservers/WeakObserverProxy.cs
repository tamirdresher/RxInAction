using System;

namespace WeakObservers
{
    class WeakObserverProxy<T> : IObserver<T>
    {
        public WeakObserverProxy(IObserver<T> observer)
        {
            this._weakObserver = new WeakReference<IObserver<T>>(observer);
        }

        private readonly WeakReference<IObserver<T>> _weakObserver;

        internal void SetSubscription(IDisposable subscriptionToSource)
        {
            this._subscriptionToSource = subscriptionToSource;
        }

        private IDisposable _subscriptionToSource;

        //执行下一步(next,error,complete)之前，检查弱引用对象是否存在。
        private void NotifyObserver(Action<IObserver<T>> action)
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
