using System.Reactive.Concurrency;

namespace RxLibrary
{
    public interface IConcurrencyProvider
    {
        IScheduler TimeBasedOperations { get;  }
        IScheduler Task { get;  }
        IScheduler Thread { get;  }
        IScheduler Dispatcher { get;  }
    }
}