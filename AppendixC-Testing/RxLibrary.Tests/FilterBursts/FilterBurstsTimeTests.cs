using Microsoft.Reactive.Testing;
using System;
using Xunit;
using RxLibrary;


namespace RxLibrary.Tests
{
    public class FilterBurstsTimeTests : ReactiveTest
    {
        [Fact]
        public void FilterBurstsInHotObservable()
        {
            var scheduler = new TestScheduler();
            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnNext(258, 2),
                OnNext(262, 3),

                OnNext(450, -1),
                OnNext(451, -2),
                OnNext(460, -3),

                OnCompleted<int>(500)
                );

            var res = scheduler.Start(() => xs.FilterBursts(TimeSpan.FromTicks(10),scheduler));

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(450, -1),
                OnCompleted<int>(500));

            xs.Subscriptions.AssertEqual(
                Subscribe(Subscribed, 500));
        }
        
        [Fact]
        public void FilterBurstsInColdObservable()
        {
            var scheduler = new TestScheduler();

            // A cold observable will begin emitting when the observer subscribes
            // in this case, each emission defined for the observable will be realtive to the observer subscription time
            // which by default is 200 (defined in ReactiveTest.Subscribed)
            var xs = scheduler.CreateColdObservable(
                OnNext(250, 1),
                OnNext(258, 2),
                OnNext(262, 3),

                OnNext(450, -1),
                OnNext(451, -2),
                OnNext(460, -3),

                OnCompleted<int>(500)
                );

            var res = scheduler.Start(() => xs.FilterBursts(TimeSpan.FromTicks(10), scheduler));

            res.Messages.AssertEqual(
                OnNext(450, 1),
                OnNext(650, -1),
                OnCompleted<int>(700));

            xs.Subscriptions.AssertEqual(
                Subscribe(ReactiveTest.Subscribed, 700));
        }

        [Fact]
        public void FilterBurstsWithATestObserver()
        {
            var scheduler = new TestScheduler();
            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnNext(258, 2),
                OnNext(262, 3),

                OnNext(450, -1),
                OnNext(451, -2),
                OnNext(460, -3),

                OnCompleted<int>(500)
                );

            var testObserver = scheduler.CreateObserver<int>();
            scheduler.AdvanceTo(200);
            xs.FilterBursts(TimeSpan.FromTicks(10), scheduler)
                .Subscribe(testObserver);

            scheduler.Start();

            testObserver.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(450, -1),
                OnCompleted<int>(500));

            xs.Subscriptions.AssertEqual(
                Subscribe(Subscribed, 500));
        }

        [Fact]
        public void FilterBursts_MaximalBurstDurationReachedTwice_FourValuesEmitted()
        {
            var scheduler = new TestScheduler();
            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnNext(251, 2),
                OnNext(252, 3),
                OnNext(260, 4),
                
                OnNext(261, 5),
                OnNext(262, 10),
                OnNext(263, 11),
                OnNext(264, 12),
                OnNext(265, 12),
                OnNext(270, 12),
                
                OnNext(272, 12),
                

                OnNext(450, -1),
                OnNext(451, -2),
                OnNext(460, -3),

                OnCompleted<int>(500)
                );


            var res = scheduler.Start(() => xs.FilterBursts(TimeSpan.FromTicks(10), TimeSpan.FromTicks(10), scheduler));


            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(261, 5),
                OnNext(272, 12),
                OnNext(450, -1),
                OnCompleted<int>(500));

            xs.Subscriptions.AssertEqual(
                Subscribe(Subscribed, 500));
        }
    }
}