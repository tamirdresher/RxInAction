using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingObservables.Chat
{
public static class ChatExtensions
{
    public static IObservable<string> ToObservable(this IChatConnection connection)
    {
        return new ObservableConnection(connection);
    }
}
}
