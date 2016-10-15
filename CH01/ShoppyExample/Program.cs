using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppyExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var shoppy = new Shoppy();
            shoppy.TestShoppy();
        }

        public class Shoppy
        {
            public void TestShoppy()
            {
                StoreIconExample();

                ConnectivityExample();
            }

            [Description("Shows how you can combine multiple observables")]
            private static void StoreIconExample()
            {
                double MIN_ICON_SIZE = 20;
                double MAX_ICON_SIZE = 32;

                IObservable<Position> myLocation = CreateDummyLocations();
                Store[] stores = CreateDummyStores();


                var storeLocation = stores.ToObservable()
                    .SelectMany(store => myLocation, (store, currentLocation) => new { store, currentLocation });

                var iconSize = from store in stores.ToObservable()
                               from currentLocation in myLocation
                               let dist = Position.Distance(store.Location, currentLocation)
                               where dist < 5
                               let calcSize = (5 / dist) * MIN_ICON_SIZE
                               let sizeOrMax = Math.Min(calcSize, MAX_ICON_SIZE)
                               let sizeOrMin = Math.Max(sizeOrMax, MIN_ICON_SIZE)
                               select
                                   new
                                   {
                                       store.Name,
                                       StoreX = store.Location.X,
                                       StoreY = store.Location.Y,
                                       MeX = currentLocation.X,
                                       MeY = currentLocation.Y,
                                       dist,
                                       calcSize,
                                       sizeOrMin,
                                       sizeOrMax
                                   };


                iconSize.Subscribe(d => Debug.WriteLine(d));
            }

            [Description("Shows how asynchrnous code execution get be part of the observable pipline")]
            private void ConnectivityExample()
            {
                IObservable<Connectivity> myConnectivity = Observable.Empty<Connectivity>();
                IObservable<IEnumerable<Discount>> newDiscounts =
                    from connectivity in myConnectivity
                    where connectivity == Connectivity.Online
                    from discounts in GetDiscounts()
                    select discounts;

                newDiscounts.Subscribe(discounts => RefreshView(discounts));
            }

            #region Helper Methods
            private static IObservable<Position> CreateDummyLocations()
            {
                return Observable.Range(1, 50).Select(i => new Position() { X = i, Y = i * 2 });
            }

            private static Store[] CreateDummyStores()
            {
                return new[]
                {
                    new Store()
                    {
                        Location = new Position() {X = 10, Y = 15},
                        Name = "ShopA"
                    },

                    new Store()
                    {
                        Location = new Position() {X = 2, Y = 3},
                        Name = "ShopB"
                    },

                    new Store()
                    {
                        Location = new Position() {X = 7, Y = 12},
                        Name = "ShopC"
                    },
                };
            }

            private void RefreshView(IEnumerable<Discount> discounts)
            {
                throw new NotImplementedException();
            }

            private Task<IEnumerable<Discount>> GetDiscounts()
            {
                //Sends request to the server and recieves the collection of discounts
                return Task.FromResult(Enumerable.Empty<Discount>());
            }

            private Task<IEnumerable<Discount>> GetDiscounts(Position currentLocation)
            {
                //Sends request to the server and recieves the collection of discounts
                return Task.FromResult(Enumerable.Empty<Discount>());
            }
            #endregion
        }

    }
}
