using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
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
            NewThreadSchedulerRecursiveSchedulingExample();
            ThreadPoolSchedulerExample();
            ThreadPoolSchedulerRecursiveSchedulingExample();
            CurrentThreadSchedulerExample();
            CurrentThreadSchedulerRecursiveSchedulingExample();
            ImmediateSchedulerExample();
            ImmediateSchedulerRecursiveSchedulingExample();
            ImmediateSchedulerFutureSchedulingExample();
            EventLoopSchedulerExample();
            EventLoopSchedulerRecursiveSchedulingExample();
            SynchronizationContextSchedulerExample();
        }

        private static void NewThreadSchedulerExample()
        {
            Demo.DisplayHeader("NewThreadScheduler - Creates a new thread for each scheduling");

            var newThreadScheduler = NewThreadScheduler.Default;

            var countdownEvent = new CountdownEvent(2);

            newThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            newThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });

            countdownEvent.Wait();

        }

        private static void NewThreadSchedulerRecursiveSchedulingExample()
        {
            Demo.DisplayHeader("NewThreadScheduler - Recursive scheduling will schedule the action on the same thread");

            var newThreadScheduler = NewThreadScheduler.Default;

            var countdownEvent = new CountdownEvent(2);

            newThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    return Disposable.Empty;
                });

            countdownEvent.Wait();

        }
        private static void ThreadPoolSchedulerExample()
        {
            Demo.DisplayHeader("ThreadPoolScheduler - Uses the ThreadPool for each scheduling");

            var threadPoolScheduler = ThreadPoolScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            threadPoolScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            threadPoolScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            countdownEvent.Wait();

        }

        private static void ThreadPoolSchedulerRecursiveSchedulingExample()
        {
            Demo.DisplayHeader("ThreadPoolScheduler - Recursive scheduling will queue the action on thread pool");

            var threadPoolScheduler = ThreadPoolScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            threadPoolScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            Console.WriteLine("Inner Action - Done:{0}", Thread.CurrentThread.ManagedThreadId);
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    Console.WriteLine("Outer Action - Done");

                    return Disposable.Empty;
                });

            countdownEvent.Wait();

        }


        private static void CurrentThreadSchedulerExample()
        {
            Demo.DisplayHeader("CurrentThreadScheduler - Uses the current thread (the caller thread) for each scheduling");


            var currentThreadScheduler = CurrentThreadScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            Console.WriteLine("Calling thread: {0}", Thread.CurrentThread.ManagedThreadId);

            currentThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            currentThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            countdownEvent.Wait();

        }

        private static void CurrentThreadSchedulerRecursiveSchedulingExample()
        {
            Demo.DisplayHeader("CurrentThreadScheduler - Recursive scheduling will queue the action on caller thread");

            var currentThreadScheduler = CurrentThreadScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            currentThreadScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            Console.WriteLine("Inner Action - Done:{0}", Thread.CurrentThread.ManagedThreadId);
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    Console.WriteLine("Outer Action - Done");

                    return Disposable.Empty;
                });

            countdownEvent.Wait();

        }



        private static void ImmediateSchedulerExample()
        {
            Demo.DisplayHeader("ImmediateScheduler - Uses the current thread (the caller thread) for each scheduling but runs the scheduled action immediatly");


            var immediateScheduler = ImmediateScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            Console.WriteLine("Calling thread: {0}", Thread.CurrentThread.ManagedThreadId);

            immediateScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            immediateScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    countdownEvent.Signal();
                });
            countdownEvent.Wait();

        }

        private static void ImmediateSchedulerRecursiveSchedulingExample()
        {
            Demo.DisplayHeader("ImmediateScheduler - Recursive scheduling will run the action immediatly on caller thread");

            var immediateScheduler = ImmediateScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);

            immediateScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            Console.WriteLine("Inner Action - Done:{0}", Thread.CurrentThread.ManagedThreadId);
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    Console.WriteLine("Outer Action - Done");

                    return Disposable.Empty;
                });

            countdownEvent.Wait();

        }

        private static void ImmediateSchedulerFutureSchedulingExample()
        {
            Demo.DisplayHeader("ImmediateScheduler - Future dueTime will cause the ImmediateScheduler block until the dueTime");

            var immediateScheduler = ImmediateScheduler.Instance;

            var countdownEvent = new CountdownEvent(2);
            Console.WriteLine("Calling thread: {0} Current time: {1}", Thread.CurrentThread.ManagedThreadId, immediateScheduler.Now);

            immediateScheduler.Schedule(Unit.Default,
                TimeSpan.FromSeconds(2),
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            Console.WriteLine("Inner Action - Done:{0}", Thread.CurrentThread.ManagedThreadId);
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    Console.WriteLine("Outer Action - Done");

                    return Disposable.Empty;
                });

            Console.WriteLine("After the Schedule, Time: {0}", immediateScheduler.Now);

            countdownEvent.Wait();

        }



        private static void EventLoopSchedulerExample()
        {
            Demo.DisplayHeader("EventLoopScheduler - Creates a designated thread that will run all scheduled actions");


var eventLoopScheduler = new EventLoopScheduler();

var countdownEvent = new CountdownEvent(2);

Console.WriteLine("Calling thread: {0}", Thread.CurrentThread.ManagedThreadId);

eventLoopScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
        countdownEvent.Signal();
    });
eventLoopScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
        countdownEvent.Signal();
    });
countdownEvent.Wait();

        }

        private static void EventLoopSchedulerRecursiveSchedulingExample()
        {
            Demo.DisplayHeader("EventLoopScheduler - Recursive scheduling will be enqueued. When an action finish, the next one is dequeued");

            var eventLoopScheduler = new EventLoopScheduler();

            var countdownEvent = new CountdownEvent(2);

            eventLoopScheduler.Schedule(Unit.Default,
                (s, _) =>
                {
                    Console.WriteLine("Outer Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                    s.Schedule(Unit.Default,
                        (s2, __) =>
                        {
                            Console.WriteLine("Inner Action - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                            countdownEvent.Signal();
                            Console.WriteLine("Inner Action - Done:{0}", Thread.CurrentThread.ManagedThreadId);
                            return Disposable.Empty;
                        });
                    countdownEvent.Signal();
                    Console.WriteLine("Outer Action - Done");

                    return Disposable.Empty;
                });

            countdownEvent.Wait();

        }

        private static void SynchronizationContextSchedulerExample()
        {
            Demo.DisplayHeader("SynchronizationContextScheduler - A bridge between Rx schedulers world and .NET SynchronizationContext");

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

var syncContextScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);

var countdownEvent = new CountdownEvent(2);

Console.WriteLine("Calling thread: {0}", Thread.CurrentThread.ManagedThreadId);

syncContextScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action1 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
        countdownEvent.Signal();
    });
syncContextScheduler.Schedule(Unit.Default,
    (s, _) =>
    {
        Console.WriteLine("Action2 - Thread:{0}", Thread.CurrentThread.ManagedThreadId);
        countdownEvent.Signal();
    });
            countdownEvent.Wait();

        }
    }
}
