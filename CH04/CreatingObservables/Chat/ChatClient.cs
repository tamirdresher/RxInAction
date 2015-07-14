using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace CreatingObservables.Chat
{
    public class ChatClient
    {
        IList<IChatConnection> _connections = new List<IChatConnection>();
        public IChatConnection Connect(string user, string password)
        {
            Console.WriteLine("Connect");
            var chatConnection = new ChatConnection();
            _connections.Add(chatConnection);
            return chatConnection;
        }


        public IObservable<string> ObserveMessages(string user, string password)
        {
            var connection = Connect(user, password);
            return connection.ToObservable();
        }

public IObservable<string> ObserveMessagesDeferred(string user, string password)
{
    return Observable.Defer(() =>
    {
        var connection = Connect(user, password);
        return connection.ToObservable();
    });
}

        #region Testing Utils
        public void NotifyRecieved(string msg)
        {
            foreach (var chatConnection in _connections)
            {
                chatConnection.NotifyRecieved(msg);
            }
        }
        public void NotifyClosed()
        {
            foreach (var chatConnection in _connections)
            {
                chatConnection.NotifyClosed();
            }
        }
        public void NotifyError()
        {
            foreach (var chatConnection in _connections)
            {
                chatConnection.NotifyError();
            }
        }
        #endregion
    }
}