using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace AsyncObservables
{
    class SearchEngineExample
    {
      

        public static IObservable<string> Search_WithAsyncAwait(string term)
        {
            return Observable.Create<string>(async o =>
            {
                var searchEngineA = new SearchEngineA();
                var searchEngineB = new SearchEngineB();
                var resultsA = await searchEngineA.SearchAsync(term);
                foreach (var result in resultsA)
                {
                    o.OnNext(result);
                }
                var resultsB = await searchEngineB.SearchAsync(term);
                foreach (var result in resultsB)
                {
                    o.OnNext(result);
                }
                o.OnCompleted();
            });
        }

        public static IObservable<string> Search_WithCancellation(string term)
        {
            return Observable.Create<string>(async (o, cancellationToken) =>
            {
                var searchEngineA = new SearchEngineA();
                var searchEngineB = new SearchEngineB();
                var resultsA = await searchEngineA.SearchAsync(term);
                foreach (var result in resultsA)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    o.OnNext(result);
                }
                cancellationToken.ThrowIfCancellationRequested();
                var resultsB = await searchEngineB.SearchAsync(term);
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
    IObservable<IEnumerable<string>> resultsA = searchEngineA.SearchAsync(term).ToObservable();
    IObservable<IEnumerable<string>> resultsB = searchEngineB.SearchAsync(term).ToObservable();
    return resultsA
        .Concat(resultsB)
        .SelectMany(x => x);
}

        public static IObservable<string> Search_DefferedConcatingTasks(string term)
        {
            var searchEngineA = new SearchEngineA();
            var searchEngineB = new SearchEngineB();
            var resultsA = Observable.Defer(() => searchEngineA.SearchAsync(term).ToObservable());
            var resultsB = Observable.Defer(() => searchEngineB.SearchAsync(term).ToObservable());
            return resultsA
                .Concat(resultsB)
                .SelectMany(x => x);
        }


        class SearchEngineA : ISearchEngine
        {
            public async Task<IEnumerable<string>> SearchAsync(string term)
            {
                Console.WriteLine("SearchEngine A - SearchAsync()");

                await Task.Delay(1500);//simulate latency
                return new[] { "resultA", "resultB" };
            }
        }

        class SearchEngineB : ISearchEngine
        {
            public async Task<IEnumerable<string>> SearchAsync(string term)
            {
                Console.WriteLine("SearchEngine B - SearchAsync()");
                await Task.Delay(1500);//simulate latency
                return new[] { "resultC", "resultD" }.AsEnumerable();
            }
        }
    }

    internal interface ISearchEngine
    {
        Task<IEnumerable<string>> SearchAsync(string term);
    }
}
