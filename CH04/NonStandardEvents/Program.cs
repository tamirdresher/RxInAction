using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace NonStandardEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new WifiScanner();

            //
            // this code snippet will result in an ArgumentException, since the NetworkFoundEventHandler delegate is not convertible to the standard EventHandler<TEventArgs> type
            //
            //var networks = Observable.FromEventPattern<NetworkFoundEventHandler, string>(
            //      (h) => { listener.NetworkFound += h; },
            //      (h) => { listener.NetworkFound -= h; });
            //networks.SubscribeConsole();

            //
            // when the target event has only one parameter, its easy to make it into observable
            //
            //IObservable<string> networks = Observable.FromEvent<NetworkFoundEventHandler, string>(
            //      (h) => { listener.NetworkFound += h; },
            //      (h) => { listener.NetworkFound -= h; });

            //
            // When the target event has more than one parameter, a conversion method is needed to turn them into a single object
            //
            IObservable<Tuple<string, int>> networks = Observable.FromEvent<ExtendedNetworkFoundEventHandler, Tuple<String, int>>(
                rxHandler =>
                    (ssid, strength) => rxHandler(Tuple.Create(ssid, strength)),
                h => listener.ExtendedNetworkFound += h,
                h => listener.ExtendedNetworkFound -= h);


            networks.SubscribeConsole();
            networks.SubscribeConsole();

            while (true)
            {
                Console.WriteLine("Enter the network ssid or X to exit");
                var ssid = Console.ReadLine();
                if (ssid == "X")
                {
                    break;
                }
                Console.WriteLine("Enter the network strength - 1 to 10");
                var strength = int.Parse(Console.ReadLine());

                listener.RaiseFound(ssid, strength);
            }
        }


    }

    public delegate void NetworkFoundEventHandler(string ssid);
    public delegate void ExtendedNetworkFoundEventHandler(string ssid, int strength);
    class WifiScanner
    {
        public event NetworkFoundEventHandler NetworkFound = delegate { };
        public event ExtendedNetworkFoundEventHandler ExtendedNetworkFound = delegate { };

        // rest of the code
        public void RaiseFound(string ssid, int strength = 0)
        {
            NetworkFound(ssid);
            ExtendedNetworkFound(ssid, strength);


        }
    }
}
