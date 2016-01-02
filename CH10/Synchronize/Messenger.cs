using System;

namespace Synchronize
{
    class Messenger
    {
        public event EventHandler<string> MessageRecieved = delegate { };
        public event EventHandler<FriendRequest> FriendRequestRecieved = delegate { };

        //Rest of the Messanger code
        public void Notify(string msg)
        {
            MessageRecieved(this, msg);
        }

        public void Notify(FriendRequest user)
        {
            FriendRequestRecieved(this, user);
        }
    }

    internal class FriendRequest   
    {
        public string UserId { get; set; }

    }
}