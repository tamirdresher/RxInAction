using System;
using System.Reactive.Disposables;

namespace CreatingObservables
{
    public class NumbersObservable : IObservable<int>
    {
        private readonly int _amount;

        public NumbersObservable(int amount)
        {
            this._amount = amount;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            for (var i = 0; i < this._amount; i++)
            {
                observer.OnNext(i);
            }
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
