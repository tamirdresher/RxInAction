using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace MagicalPrimeGeneratorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //GeneratingSynchronously();
            GeneratingAsynchronously();
            Console.ReadLine();
        }

        private static void GeneratingSynchronously()
        {

            Console.WriteLine();
            Console.WriteLine("----- Using synchronous enumerable ----");

            var generator = new MagicalPrimeGenerator();
            // this will block the main thread for a few seconds
            foreach (var prime in generator.Generate(5))
            {
                Console.Write("{0}, ", prime);
            }
        }

        private static void GeneratingAsynchronously()
        {
            Console.WriteLine();
            Console.WriteLine("----- Using observable ----");

            var generator = new MagicalPrimeGenerator();

            var subscription=generator.ObservePrimes(5)
                .SubscribeConsole("async observable");

            Console.WriteLine("Proving we're not blocked. you can continue typing. type X to dispose and exit");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "X")
                {
                    subscription.Dispose();
                    return;
                }

                Console.WriteLine("\t {0}", input);
            }
        }
        
    }
}
