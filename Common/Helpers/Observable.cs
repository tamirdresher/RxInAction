using System;
using System.Reactive.Linq;

namespace System.Reactive.Linq
{
    public static partial class ObservableEx
    {
        public static IObservable<T> FromValues<T>(params T[] values)
        {
            return values.ToObservable();
        } 
    }
}