using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagicalPrimeGeneratorExample
{
    public class MagicalPrimeGenerator
    {
        public IEnumerable<int> Generate(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return GeneratePrime(i);
            }
        }

        public async Task<IReadOnlyCollection<int>> GenerateAsync(int amount)
        {
            return await Task.Run(() => Generate(amount).ToList().AsReadOnly());
        }
        public IObservable<int> GeneratePrimes_ManualAsync(int amount)
        {
            var cts = new CancellationTokenSource();
            return Observable.Create<int>(o =>
            {
                Task.Run(() =>
                {
                    foreach (var prime in Generate(amount))
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        o.OnNext(prime);
                    }
                    o.OnCompleted();
                }, cts.Token);

                return new CancellationDisposable(cts);
            });
        }

        public IObservable<int> GeneratePrimes_Sync(int amount)
        {
            return Observable.Create<int>(o =>
            {
                foreach (var prime in Generate(amount))
                {
                    o.OnNext(prime);
                }
                o.OnCompleted();
                return Disposable.Empty;
            });
        }
        public IObservable<int> GeneratePrimes_AsyncCreate(int amount)
        {
            return Observable.Create<int>((o, ct) =>
            {
                return Task.Run(() =>
                {
                    foreach (var prime in Generate(amount))
                    {
                        ct.ThrowIfCancellationRequested();
                        o.OnNext(prime);
                    }
                    o.OnCompleted();
                }, ct);

            });
        }

        public IObservable<int> GeneratePrimes3(int amount)
        {
            return Observable.Create<int>((o, ct) =>
            {
                return Task.Run(() =>
                {
                    foreach (var prime in Generate(amount))
                    {
                        ct.ThrowIfCancellationRequested();
                        o.OnNext(prime);
                    }
                    o.OnCompleted();
                });

            });
        }

        int GeneratePrime(int index)
        {
            // Simulate the hard work 
            Thread.Sleep(2000);

            var firstNumbers = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, };
            if (index < firstNumbers.Length)
            {
                return firstNumbers[index];
            }

            // THIS IS JUST FOR DEMONSTRATION, I DONT REALLY HAVE MAGICAL GENERATOR
            return firstNumbers.Last();
        }


    }
}