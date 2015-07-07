using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FirstRxExample
{
    public class Shoppy
    {
        public void TestShoppy()
        {
            double MIN_ICON_SIZE = 20;
            double MAX_ICON_SIZE = 32;

            var myLocation = Observable.Range(1, 50).Select(i => new Position() { X = i, Y = i * 2 });
            var stores = new[]
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


            var storeLocation = stores.ToObservable()
                .SelectMany(store => myLocation, (store, currentLocation) => new {store, currentLocation})
                ;

var iconSize = from store in stores.ToObservable()
               from currentLocation in myLocation
               let dist = Position.Distance(store.Location, currentLocation)
               where dist < 5
               let calcSize = (5 / dist) * MIN_ICON_SIZE
               let sizeOrMax = Math.Min(calcSize, MAX_ICON_SIZE)
               let sizeOrMin = Math.Max(sizeOrMax, MIN_ICON_SIZE)
               select new { store.Name, StoreX = store.Location.X, StoreY = store.Location.Y, MeX = currentLocation.X, MeY = currentLocation.Y, dist, calcSize, sizeOrMin, sizeOrMax };
            
            var iconSize2 =
                stores.ToObservable()
                    .SelectMany(store => myLocation, (store, currentLocation) => new {store, currentLocation})
                    .Select(@t => new {@t, dist = Position.Distance(@t.store.Location, @t.currentLocation)})
                    .Where(@t => @t.dist < 5)
                    .Select(@t => new {@t, calcSize = (5/@t.dist)*MIN_ICON_SIZE})
                    .Select(@t => new {@t, sizeOrMax = Math.Min(@t.calcSize, MAX_ICON_SIZE)})
                    .Select(@t => new {@t, sizeOrMin = Math.Max(@t.sizeOrMax, MIN_ICON_SIZE)})
                    .Select(
                        @t =>
                            new
                            {
                                @t.@t.@t.@t.@t.store.Name,
                                StoreX = @t.@t.@t.@t.@t.store.Location.X,
                                StoreY = @t.@t.@t.@t.@t.store.Location.Y,
                                MeX = @t.@t.@t.@t.@t.currentLocation.X,
                                MeY = @t.@t.@t.@t.@t.currentLocation.Y,
                                @t.@t.@t.@t.dist,
                                @t.@t.@t.calcSize,
                                @t.sizeOrMin,
                                @t.@t.sizeOrMax
                            });

            iconSize.Subscribe(d => Debug.WriteLine(d));

IObservable<Connectivity> myConnectivity=Observable.Empty<Connectivity>();  
IObservable<IEnumerable<Discount>> newDiscounts = 
    from connectiviy in myConnectivity
    where connectiviy == Connectivity.Online
    from discounts in GetDiscounts()
    select discounts;

newDiscounts.Subscribe(discounts => RefreshView(discounts));
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
    }

    public enum Connectivity
    {
        Online,
        Offline
    }

    internal class Discount
    {
    }

    class Store
    {
        public Position Location { get; set; }
        public string Name { get; set; }

    }

    internal class Position
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static double Distance(Position left, Position right)
        {
            var diff = new Position()
            {
                X = left.X - right.X,
                Y = left.Y = right.Y
            };
            return DotProduct(diff, diff);
        }

        public static double DotProduct(Position left, Position right)
        {
            return Math.Sqrt(left.X * right.X + left.Y * right.Y);
        }
    }
}