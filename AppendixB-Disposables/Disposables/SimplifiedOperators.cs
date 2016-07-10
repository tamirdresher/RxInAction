using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Disposables
{
    public static class SimplifiedOperators
    {
        public static IObservable<T> Return<T>(T value)
        {
            return Observable.Create<T>(o =>
            {
                o.OnNext(value);
                o.OnCompleted();
                return Disposable.Empty;
            });
        }

        public static IObservable<TSource> MySubscribeOn<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            return Observable.Create<TSource>(observer =>
            {
                var d = new SerialDisposable();

                d.Disposable = scheduler.Schedule(() =>
                {
                    d.Disposable = new ScheduledDisposable(scheduler, source.SubscribeSafe(observer));
                });

                return d;
            });
        }

    }
}