using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace HotAndColdObservables
{
    class Program
    {
        static void Main(string[] args)
        {
            ColdObservableMultipleSubscriptionsExample().Wait();

            Console.ReadLine();
        }

        public static async Task ColdObservableMultipleSubscriptionsExample()
        {
            var coldObservable =
                Observable.Create<string>(async o =>
                {
                    o.OnNext("Hello");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    o.OnNext("Rx");
                });

            coldObservable.SubscribeConsole("o1");
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            coldObservable.SubscribeConsole("o2");

        }
    }
}
