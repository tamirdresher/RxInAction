using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagicalPrimeGeneratorExample
{
    class MagicalPrimeGenerator
    {
        public IEnumerable<int> Generate(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return GeneratePrime(i);
            }
        }

        public IObservable<int> ObservePrimes(int amount)
        {
            CancellationTokenSource cts=new CancellationTokenSource(); 
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
                });

                return new CancellationDisposable(cts);
            });
        } 

        int GeneratePrime(int index)
        {
            // Simulate the hard work 
            Thread.Sleep(2000);

            var firstNumbers = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, };
            if (index<firstNumbers.Length)
            {
                return firstNumbers[index];
            }
            
            // THIS IS JUST FOR DEMONSTRATION, I DONT REALLY HAVE MAGICAL GENERATOR
            return firstNumbers.Last();
        }
       
        
    }
}