using System.Reactive.Concurrency;

namespace RxLibrary
{
    class ConcurrencyProvider : IConcurrencyProvider
    {
        public ConcurrencyProvider()
        {
            this.TimeBasedOperations = DefaultScheduler.Instance;
            this.Task = TaskPoolScheduler.Default;
            this.Thread = NewThreadScheduler.Default;

#if HAS_DISPATCHER
            Dispatcher=DispatcherScheduler.Current;
#else
            //workaround to whenever there's no dispatcher
            this.Dispatcher = CurrentThreadScheduler.Instance;
#endif
        }

        public IScheduler TimeBasedOperations { get; }
        public IScheduler Task { get; }
        public IScheduler Thread { get; }
        public IScheduler Dispatcher { get; }
    }
}
