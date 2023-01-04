using System;

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
