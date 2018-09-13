using System;
using System.Reactive.Subjects;

namespace Subjects
{
    class BankAccount
    {
        readonly Subject<int> _inner = new Subject<int>();

        public IObservable<int> MoneyTransactions => this._inner;
    }
}
