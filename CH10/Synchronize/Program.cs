using Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace Synchronize
{
    class Program
    {
        static void Main(string[] args)
        {
            OneObservableSynchronization();
            MultipleObservableSynchronization();

            Console.ReadLine();
        }

        private static void OneObservableSynchronization()
        {
            Demo.DisplayHeader("The Synchrnoize operator - synchronizes the notifications so they will be received in a seriazlied way");

            var messenger = new Messenger();
            IObservable<System.Reactive.EventPattern<string>> messages =
                Observable.FromEventPattern<string>(
                    h => messenger.MessageRecieved += h,
                    h => messenger.MessageRecieved -= h);

            messages
                .Select(evt => evt.EventArgs)
                .Synchronize()
                .Subscribe(msg => {
                    Console.WriteLine("Message {0} arrived", msg);
                    Thread.Sleep(1000);
                    Console.WriteLine("Message {0} exit", msg);
                });

            for (var i = 0; i < 3; i++)
            {
                var msg = "msg" + i;
                ThreadPool.QueueUserWorkItem((_) => {
                    messenger.Notify(msg);
                });
            }

            //waiting for all the other threads to complete before proceeding
            Thread.Sleep(2000);
        }

        private static void MultipleObservableSynchronization()
        {
            Demo.DisplayHeader("The Synchrnoize operator - can synchronizes the notifications from multiple observables by passing the gate object");

            var messenger = new Messenger();
            IObservable<System.Reactive.EventPattern<string>> messages =
                Observable.FromEventPattern<string>(
                    h => messenger.MessageRecieved += h,
                    h => messenger.MessageRecieved -= h);

            IObservable<System.Reactive.EventPattern<FriendRequest>> friendRequests =
                Observable.FromEventPattern<FriendRequest>(
                    h => messenger.FriendRequestRecieved += h,
                    h => messenger.FriendRequestRecieved -= h);

            var gate = new object();

            messages
                .Select(evt => evt.EventArgs)
                .Synchronize(gate)
                .Subscribe(msg => {
                    Console.WriteLine("Message {0} arrived", msg);
                    Thread.Sleep(1000);
                    Console.WriteLine("Message {0} exit", msg);
                });

            friendRequests
                .Select(evt => evt.EventArgs)
                .Synchronize(gate)
                .Subscribe(request => {
                    Console.WriteLine("user {0} sent request", request.UserId);
                    Thread.Sleep(1000);
                    Console.WriteLine("user {0} approved", request.UserId);
                });
            for (var i = 0; i < 3; i++)
            {
                var msg = "msg" + i;
                var userId = "user" + i;
                ThreadPool.QueueUserWorkItem((_) => {
                    messenger.Notify(msg);
                });

                ThreadPool.QueueUserWorkItem((_) => {
                    messenger.Notify(new FriendRequest() { UserId = userId });
                });
            }

            //waiting for all the other threads to complete before proceeding
            Thread.Sleep(3000);
        }
    }
}
