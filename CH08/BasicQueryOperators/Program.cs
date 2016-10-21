using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BasicQueryOperators.Examples;
using BasicQueryOperators.Model;
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
            SelectManyNewsImagesExample.Run();
            ChatRoomsExample.Run();

            Where();
            Distinct();
            DistinctNewsItems();
            DistinctUntilChanged();
        }



        private static void Where()
        {
            Demo.DisplayHeader("The Where operator - filters the elements of an observable sequence based on a predicate ");

            var strings = new[] { "aa", "Abc", "Ba", "Ac" }.ToObservable();

            strings.Where(s => s.StartsWith("A"))
                .SubscribeConsole();
        }

        private static void DistinctUntilChanged()
        {
            Demo.DisplayHeader("The DistinctUntilChanged operator - returns an observable sequence that contains only distinct contiguous element");

            var subject = new Subject<int>();//this could have been
            subject.Log()
                .DistinctUntilChanged()
                .SubscribeConsole("DistinctUntilChanged");

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(2);
            subject.OnNext(4);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        private static void Distinct()
        {
            Demo.DisplayHeader("The Distinct operator - filters values that were already emmited by the observable");

            var subject = new Subject<int>();
            subject.Log()
                .Distinct()
                .SubscribeConsole("Distinct");

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(4);
            subject.OnCompleted();


        }
        private static void DistinctNewsItems()
        {
            Demo.DisplayHeader("The Distinct operator - filters values that were already emmited by the observable - by a given keySelector");

            var subject = new Subject<NewsItem>();
            subject.Log()
                .Distinct(n => n.Title)//items with same Title will only be emitted once
                .SubscribeConsole("Distinct");
            subject.OnNext(new NewsItem() { Title = "Title1" });
            subject.OnNext(new NewsItem() { Title = "Title2" });
            subject.OnNext(new NewsItem() { Title = "Title1" });
            subject.OnNext(new NewsItem() { Title = "Title3" });

            subject.OnCompleted();
        }
        private static void SelectingFromAnotherSource()
        {
            Demo.DisplayHeader("The Select operator - using select to load data");

            IObservable<ChatMessage> messages =
                Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(5)
                    .Select(i => new ChatMessage { Content = "Message" + i, Timestamp = DateTime.UtcNow, Sender = "123" + i });
            IObservable<ChatMessageViewModel> messagesViewModels =
                messages
                    .Select(m => new ChatMessageViewModel
                    {
                        MessageContent = m.Content,
                        User = LoadUserFromDb(m.Sender)
                    })
                    .Log();

            messagesViewModels.Wait();
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
            Console.WriteLine("Loading User {0} from DB", sender);
            Thread.Sleep(600);//simulate latency
            return new User() { Name = "My Name", Id = sender };
        }
    }
}
