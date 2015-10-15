using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace BasicQueryOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            Selecting();
            SelectingFromAnotherSource();
            SelectManyObservables();
        }

        private static void SelectingFromAnotherSource()
        {
            Demo.DisplayHeader("The Select operator - using select to load data");

            var messages =
                Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(5)
                    .Select(i =>new ChatMessage {Content = "Message" + i, Timestamp = DateTime.UtcNow, Sender = "123" + i});

            messages.Select(m => new {MessageContent=m.Content, User=LoadUserFromDb(m.Sender)})
                .Log()
                .Wait();
        }

       

        private static void Selecting()
        {
            Demo.DisplayHeader("The Select operator - taking only some of the properties in a ChatMessage");

            Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(5)
                .Select(i => new ChatMessage { Content = "Message" + i, Timestamp = DateTime.UtcNow })
                .Select(m => new { Message = m.Content, LocalTime = m.Timestamp.ToLocalTime() })
                .Log()
                .Wait();
        }

        private static void SelectManyObservables()
        {
            Demo.DisplayHeader("SelectMany(Observables)");

            var observables =
                Observable.Interval(TimeSpan.FromSeconds(2))
                    .Take(3)
                    .Select(i => Observable.Interval(TimeSpan.FromSeconds(1))
                        .Select(x => "Interval" + i + " - " + x)
                        .Take(3));

            observables.SelectMany(xs => xs)
                .Log()
                .Wait();

        }


        private static User LoadUserFromDb(string sender)
        {
            Console.WriteLine("Loading User {0} from DB",sender);
            Thread.Sleep(600);//simulate latency
            return  new User() {Name = "My Name",Id = sender};
        }
    }

    internal class User
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return String.Format("Id={0} Name:{1}",Id,Name);
        }
    }

    internal class ChatMessage
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
    }
}
