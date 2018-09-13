using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace AsyncObservables.SearchEngine
{
    class SearchEngineExample
    {
        public static IObservable<string> Search_WithAsyncAwait(string term)
        {
            return Observable.Create<string>(async o => {
                var searchEngineA = new SearchEngineA();
                var searchEngineB = new SearchEngineB();
                IEnumerable<string> resultsA = await searchEngineA.SearchAsync(term);
                foreach (var result in resultsA)
                {
                    o.OnNext(result);
                }
                IEnumerable<string> resultsB = await searchEngineB.SearchAsync(term);
                foreach (var result in resultsB)
                {
                    o.OnNext(result);
                }
                o.OnCompleted();
            });
        }

        public static IObservable<string> Search_WithCancellation(string term)
        {
            return Observable.Create<string>(async (o, cancellationToken) => {
                var searchEngineA = new SearchEngineA();
                var searchEngineB = new SearchEngineB();
                IEnumerable<string> resultsA = await searchEngineA.SearchAsync(term);
                foreach (var result in resultsA)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    o.OnNext(result);
                }
                cancellationToken.ThrowIfCancellationRequested();
                IEnumerable<string> resultsB = await searchEngineB.SearchAsync(term);
                foreach (var result in resultsB)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    o.OnNext(result);
                }
                o.OnCompleted();
            });
        }

        public static IObservable<string> Search_ConcatingTasks(string term)
        {
            var searchEngineA = new SearchEngineA();
            var searchEngineB = new SearchEngineB();
            IObservable<IEnumerable<string>> resultsA =
                searchEngineA.SearchAsync(term).ToObservable();
            IObservable<IEnumerable<string>> resultsB =
                searchEngineB.SearchAsync(term).ToObservable();

            return resultsA
                .Concat(resultsB)
                .SelectMany(x => x);
        }

        public static IObservable<string> Search_DefferedConcatingTasks(string term)
        {
            var searchEngineA = new SearchEngineA();
            var searchEngineB = new SearchEngineB();
            IObservable<IEnumerable<string>> resultsA = Observable.Defer(() => searchEngineA.SearchAsync(term).ToObservable());
            IObservable<IEnumerable<string>> resultsB = Observable.Defer(() => searchEngineB.SearchAsync(term).ToObservable());
            return resultsA
                .Concat(resultsB)
                .SelectMany(x => x);
        }
    }
}
