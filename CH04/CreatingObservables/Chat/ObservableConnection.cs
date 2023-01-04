using System;
using System.Reactive;
using System.Reactive.Disposables;

namespace CreatingObservables.Chat
{
    public class ObservableConnection : ObservableBase<string>
    {
        private readonly IChatConnection _chatConnection;

        public ObservableConnection(IChatConnection chatConnection)
        {
            this._chatConnection = chatConnection;
        }

        protected override IDisposable SubscribeCore(IObserver<string> observer)
        {
            Action<string> received = message => {
                observer.OnNext(message);
            };

            Action closed = () => {
                observer.OnCompleted();
            };

            Action<Exception> error = ex => {
                observer.OnError(ex);
            };

            this._chatConnection.Received += received;
            this._chatConnection.Closed += closed;
            this._chatConnection.Error += error;

            return Disposable.Create(() => {
                this._chatConnection.Received -= received;
                this._chatConnection.Closed -= closed;
                this._chatConnection.Error -= error;
                this._chatConnection.Disconnect();
            });
        }
    }
}
