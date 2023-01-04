using System;

namespace RxLibrary
{
    public class Alert
    {
        public string Message { get; set; }
        public DateTimeOffset Time { get; set; }

        public Alert(string message, DateTimeOffset time)
        {
            this.Message = message;
            this.Time = time;
        }
    }
}
