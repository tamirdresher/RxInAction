using Helpers;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

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

            IDisposable inner = Disposable.Create(() => Console.WriteLine("Disposing inner-disposable"));
            var refCountDisposable = new RefCountDisposable(inner);
            IDisposable d1 = refCountDisposable.GetDisposable();
            IDisposable d2 = refCountDisposable.GetDisposable();
            IDisposable d3 = refCountDisposable.GetDisposable();

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
            compositeDisposable = new CompositeDisposable
            {
                Disposable.Create(() => Console.WriteLine("1st disposed")),
                Disposable.Create(() => Console.WriteLine("2nd disposed"))
            };

            compositeDisposable.Dispose();
        }

        private static void AddToCompositeDisposable()
        {
            Demo.DisplayHeader("AddToCompositeDisposable extensions method - useful for keeping your observable pipeline fluent");

            var compositeDisposable = new CompositeDisposable();
            IObservable<string> observable = ObservableExtensionsHelpers.FromValues("Rx", "For", "The", "Win");

            observable.Where(x => x.Length % 2 == 0)
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
