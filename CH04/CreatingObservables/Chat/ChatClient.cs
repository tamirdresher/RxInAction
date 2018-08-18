using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace CreatingObservables.Chat {
    public class ChatClient {
        readonly IList<IChatConnection> _connections = new List<IChatConnection>();
        public IChatConnection Connect(string user, string password) {
            Console.WriteLine("Connect");
            var chatConnection = new ChatConnection();
            this._connections.Add(chatConnection);
            return chatConnection;
        }

        public IObservable<string> ObserveMessages(string user, string password) {
            IChatConnection connection = this.Connect(user, password);
            return connection.ToObservable();
        }

        public IObservable<string> ObserveMessagesDeferred(string user, string password) {
            return Observable.Defer(() => {
                IChatConnection connection = this.Connect(user, password);
                return connection.ToObservable();
            });
        }

        #region Testing Utils

        public void NotifyRecieved(string msg) {
            foreach (IChatConnection chatConnection in this._connections) {
                chatConnection.NotifyRecieved(msg);
            }
        }

        public void NotifyClosed() {
            foreach (IChatConnection chatConnection in this._connections) {
                chatConnection.NotifyClosed();
            }
        }

        public void NotifyError() {
            foreach (IChatConnection chatConnection in this._connections) {
                chatConnection.NotifyError();
            }
        }

        #endregion Testing Utils
    }
}
