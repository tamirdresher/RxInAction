using System;

namespace BasicQueryOperators.Model
{
    internal class ChatMessage
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
        public string Room { get; set; }
    }
}