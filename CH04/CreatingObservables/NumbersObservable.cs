using System;
using System.Reactive.Disposables;

namespace CreatingObservables
{
    public class NumbersObservable : IObservable<int>
    {
        private readonly int _amount;

        public NumbersObservable(int amount)
        {
            _amount = amount;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            for (int i = 0; i < _amount; i++)
            {
                observer.OnNext(i);
            }
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}