using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace Disposables
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestMySubscribeOn(); 
            RefCountDisposable();
            CompositeDisposable();
            AddToCompositeDisposable();
            BooleanDisposable();
            Console.ReadLine();
        }

       

        private static void TestMySubscribeOn()
        {
            Demo.DisplayHeader("Using a simplified version of SubscribrOn()");

            Console.WriteLine("MainThread Id {0}", Thread.CurrentThread.ManagedThreadId);
            Observable.Return(10)
                .MySubscribeOn(ThreadPoolScheduler.Instance)
                .Do(x => Console.WriteLine("Thread Id " + Thread.CurrentThread.ManagedThreadId))
                .Wait();
        }
        private static void RefCountDisposable()
        {
            Demo.DisplayHeader("The RefCountDisposable - dispose the underlying disposable when all referencing disposables are dispsosed");

            var inner = Disposable.Create(() => Console.WriteLine("Disposing inner-disposable"));
            var refCountDisposable = new RefCountDisposable(inner);
            var d1 = refCountDisposable.GetDisposable();
            var d2 = refCountDisposable.GetDisposable();
            var d3 = refCountDisposable.GetDisposable();

            refCountDisposable.Dispose();
            Console.WriteLine("Disposing 1st");
            d1.Dispose();
            Console.WriteLine("Disposing 2nd");
            d2.Dispose();
            Console.WriteLine("Disposing 3rd");
            d3.Dispose();

        }

        private static void CompositeDisposable()
        {
            Demo.DisplayHeader("The CompositeDisposable - groups multiple disposables and dispose them together");

            var compositeDisposable = new CompositeDisposable(
                Disposable.Create(() => Console.WriteLine("1st disposed")),
                Disposable.Create(() => Console.WriteLine("2nd disposed")));

            compositeDisposable.Dispose();

            //The same can also be written using the Add() method
            compositeDisposable = new CompositeDisposable();
            compositeDisposable.Add(Disposable.Create(() => Console.WriteLine("1st disposed")));
            compositeDisposable.Add(Disposable.Create(() => Console.WriteLine("2nd disposed")));

            compositeDisposable.Dispose();
        }

        private static void AddToCompositeDisposable()
        {
            Demo.DisplayHeader("AddToCompositeDisposable extensions method - useful for keeping your observable pipeline fluent");

            var compositeDisposable = new CompositeDisposable();
            IObservable<string> observable = ObservableEx.FromValues("Rx", "For", "The", "Win");

            observable.Where(x => x.Length%2 == 0)
                .Select(x => x.ToUpper())
                .Subscribe(x => Console.WriteLine(x))
                .AddToCompositeDisposable(compositeDisposable);

            observable.Where(x => x.Length % 2 == 1)
                .Select(x => x.ToLower())
                .Subscribe(x => Console.WriteLine(x))
                .AddToCompositeDisposable(compositeDisposable);
        }

        private static void BooleanDisposable()
        {
            Demo.DisplayHeader("The BooleanDispoable - sets a boolean flag upon disposal");

            var booleanDisposable = new BooleanDisposable();
            Console.WriteLine("Before dispose, booleanDisposable.IsDisposed = {0}", booleanDisposable.IsDisposed);
            booleanDisposable.Dispose();
            Console.WriteLine("After dispose, booleanDisposable.IsDisposed = {0}", booleanDisposable.IsDisposed);


        }
    }
}
