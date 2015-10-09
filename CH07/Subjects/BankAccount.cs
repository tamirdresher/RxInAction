using System;
using System.Reactive.Subjects;

namespace Subjects
{
 class BankAccount
 {
     Subject<int> _inner = new Subject<int>();

     public IObservable<int> MoneyTransactions { get { return _inner; } }  
 }
}