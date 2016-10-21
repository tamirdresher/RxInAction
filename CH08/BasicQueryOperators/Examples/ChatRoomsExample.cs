using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using BasicQueryOperators.Model;
using Helpers;

namespace BasicQueryOperators.Examples
{
    public class ChatRoomsExample
    {
        public static void Run()
        {
            FlatenningObservableOfObservables();
            ProccessingTheSourceAndTheResult();
        }

        private static void ProccessingTheSourceAndTheResult()
        {
            Demo.DisplayHeader("The SelectMany operator - with resultSelector you can have access to the original emitted item");

            var roomsSubject = new Subject<ChatRoom>();

            IObservable<ChatRoom> rooms = roomsSubject.AsObservable();

            rooms
                .Log("Rooms")
                .SelectMany(r => r.Messages,
                            (room, msg) => new ChatMessageViewModel(msg) { Room = room.Id })
                .Subscribe(vm => AddToDashboard(vm));


            var room1 = new Subject<ChatMessage>();
            roomsSubject.OnNext(new ChatRoom { Id = "Room1", Messages = room1 });

            room1.OnNext(new ChatMessage { Content = "First Message", Sender = "1" });
            room1.OnNext(new ChatMessage { Content = "Second Message", Sender = "1" });

            var room2 = new Subject<ChatMessage>();
            roomsSubject.OnNext(new ChatRoom { Id = "Room2", Messages = room2 });


            room2.OnNext(new ChatMessage { Content = "Hello World", Sender = "2" });
            room1.OnNext(new ChatMessage { Content = "Another Message", Sender = "1" });
        }

        private static void FlatenningObservableOfObservables()
        {
            Demo.DisplayHeader("The SelectMany operator - flattening observables of message to one stream of messages");

            var roomsSubject = new Subject<ChatRoom>();

            IObservable<ChatRoom> rooms = roomsSubject.AsObservable();

            rooms
                .Log("Rooms")
                .SelectMany(r => r.Messages)
                .Select(m => new ChatMessageViewModel(m))
                .Subscribe(vm => AddToDashboard(vm));

            //
            // This is how the same example would look without SelectMany
            //
            //rooms
            //    .Log("Rooms")
            //    .Select(r => r.Messages)
            //    .Select(messages => messages.Select(m => new ChatMessageViewModel(m)))
            //    .Subscribe(roomMessages => roomMessages.Subscribe(m => AddToDashboard(m)));

            var room1 = new Subject<ChatMessage>();
            roomsSubject.OnNext(new ChatRoom { Id = "Room1", Messages = room1.Do(m => m.Room = "Room1") });

            room1.OnNext(new ChatMessage { Content = "First Message", Sender = "1" });
            room1.OnNext(new ChatMessage { Content = "Second Message", Sender = "1" });

            var room2 = new Subject<ChatMessage>();
            roomsSubject.OnNext(new ChatRoom { Id = "Room2", Messages = room2.Do(m => m.Room = "Room2") });


            room2.OnNext(new ChatMessage { Content = "Hello World", Sender = "2" });
            room1.OnNext(new ChatMessage { Content = "Another Message", Sender = "1" });
        }

        private static void AddToDashboard(ChatMessageViewModel chatMessage)
        {
            Console.WriteLine(chatMessage);
        }
    }
}