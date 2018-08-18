using System;
using System.Reactive.Disposables;

namespace CreatingObservables {
    /// <summary>
    /// This demonstrates what happens when we write our own observables, the contract between the
    /// observable and the observer needs to be enforced by us. A call to OnNext after a call to
    /// OnCompleted will still be observed by the observer
    /// </summary>
    public class ErrorNumbersObservable : IObservable<int> {
        private readonly int _amount;

        public ErrorNumbersObservable(int amount) {
            this._amount = amount;
        }

        public IDisposable Subscribe(IObserver<int> observer) {
            for (var i = 0; i < this._amount; i++) {
                observer.OnNext(i);
            }
            observer.OnCompleted();

            // this will be observed by the observer
            observer.OnNext(this._amount);
            return Disposable.Empty;
        }
    }
}
