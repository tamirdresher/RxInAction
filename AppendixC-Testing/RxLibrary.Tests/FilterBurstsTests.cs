using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using RxLibrary;

namespace RxLibrary.Tests
{
    public class FilterBurstsTests : ReactiveTest
    {
        [Fact]
        public static void FilterBursts_SeuquenceOf10AndBurstSize5_TwoEmissions()
        {
            var seqeucneSize = 10;
            var burstSize = 5;
            var expected = new[] { 0, 5 };
            var observable = Observable.Range(0, seqeucneSize);

            observable.FilterBursts(burstSize)
                .AssertEqual(expected.ToObservable());
        }

        [Theory]
        [InlineData(1, 1, new[] { 0 })]
        [InlineData(5, 1, new[] { 0 })]
        [InlineData(1, 5, new[] { 0, 1, 2, 3, 4 })]
        [InlineData(5, 5, new[] { 0 })]
        [InlineData(5, 8, new[] { 0, 5 })]
        public void FilterBursts(int burstSize, int sequenceSize, int[] expected)
        {
            var observable = Observable.Range(0, sequenceSize);
            observable.FilterBursts(burstSize)
                .AssertEqual(expected.ToObservable());
        }

        [Fact]
        public void FilterBursts_TwoBurstAndGapInEachBurst_FirstInEachBurstEmitted()
        {
            
            var scheduler = new TestScheduler();
            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnNext(260, 2),
                OnNext(270, 3),

                OnNext(400, -1),
                OnNext(401, -2),
                OnNext(405, -3),

                OnCompleted<int>(500)
                );

            var res = scheduler.Start(() => xs.FilterBursts(3));

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(400, -1),
                OnCompleted<int>(500));

            xs.Subscriptions.AssertEqual(
                Subscribe(Subscribed, 500));
        }

        
    }
}