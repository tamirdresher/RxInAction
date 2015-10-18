using System;

namespace BasicQueryOperators.Model
{
    class ChatRoom
    {
        public string Id { get; set; }
        public IObservable<ChatMessage> Messages { get; set; }

        public override string ToString()
        {
            return "ChatRoom: " + Id;
        }
    }
}