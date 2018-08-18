using Helpers;
using System;

namespace CreatingObservables.Chat {
    static class ChatExample {
        public static void Run() {
            var chatClient = new ChatClient();
            //var subscription = CreateObservableConnection(chatClient);
            //var subscription = CreateObservableConnectionFluenttly(chatClient);
            IDisposable subscription = CreateDefferedObservableConnection(chatClient);

            while (true) {
                Console.WriteLine("write a message to be sent, E for error, or X to exit");
                var msg = Console.ReadLine();
                if (msg == "X") {
                    chatClient.NotifyClosed();
                    break;
                }
                if (msg == "E") {
                    chatClient.NotifyError();
                }

                chatClient.NotifyRecieved(msg);
            }
        }

        private static IDisposable CreateObservableConnection(ChatClient chatClient) {
            IChatConnection connection = chatClient.Connect("guest", "guest");
            IObservable<string> observableConnection =
                new ObservableConnection(connection);

            IDisposable subscription =
                observableConnection.SubscribeConsole("reciever");

            return subscription;
        }

        private static IDisposable CreateObservableConnectionFluenttly(ChatClient chatClient) {
            IDisposable subscription =
                chatClient.Connect("guest", "guest")
                .ToObservable()
                .SubscribeConsole();

            return subscription;
        }

        private static IDisposable CreateDefferedObservableConnection(ChatClient chatClient) {
            IObservable<string> messages = chatClient.ObserveMessagesDeferred("user", "password");
            Console.WriteLine("Press Enter to subscribe to the deffered observable");
            Console.ReadLine();
            IDisposable subscription = messages.SubscribeConsole();
            IDisposable subscription2 = messages.SubscribeConsole();

            return subscription;
        }
    }
}
