using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingObservables
{
public static  class Extensions
{
    public static IDisposable SubscribeConsole<T>(this IObservable<T> observable,string name="")
    {
        return observable.Subscribe(new ConsoleObserver<T>(name));
    }
}
}
