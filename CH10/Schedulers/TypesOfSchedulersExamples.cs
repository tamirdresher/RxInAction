using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace Schedulers
{
    static class TypesOfSchedulersExamples
    {
        public static void Run()
        {
            NewThreadSchedulerExample();
        }

        private static void NewThreadSchedulerExample()
        {
            Demo.DisplayHeader("NewThreadScheduler - Creates a new thread for each scheduling");

var newThreadScheduler = NewThreadScheduler.Default;

newThreadScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
    });
newThreadScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
    });
        }
    }
}
