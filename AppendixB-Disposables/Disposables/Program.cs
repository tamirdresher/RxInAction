using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
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
            TestMySubscribeOn();

            Console.ReadLine();
        }

        private static void TestMySubscribeOn()
        {
            Console.WriteLine("MainThread Id {0}", Thread.CurrentThread.ManagedThreadId);
            Observable.Return(10)
                .MySubscribeOn(ThreadPoolScheduler.Instance)
                .Do(x => Console.WriteLine("Thread Id " + Thread.CurrentThread.ManagedThreadId))
                .Wait();
        }
    }
}
