using System;

namespace BasicQueryOperators.Model
{
    internal class ChatMessageViewModel
    {
        public ChatMessageViewModel()
        {
        }

        public ChatMessageViewModel(ChatMessage chatMessage)
        {
            this.MessageContent = chatMessage.Content;
            this.Room = chatMessage.Room;

            // this is for demostrating purposes only, useally the user will be fetched from a real repository
            this.User = new User() { Id = chatMessage.Sender, Name = "User" + chatMessage.Sender };
        }

        public string MessageContent { get; set; }
        public User User { get; set; }
        public string Room { get; set; }
        public override string ToString()
        {
            return String.Format("Room: {0} , Message: \"{1}\" was sent by {2}", this.Room, this.MessageContent, this.User);
        }
    }
}
