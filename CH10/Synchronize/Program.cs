using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

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
            var messages =
                Observable.FromEventPattern<string>(
                    h => messenger.MessageRecieved += h,
                    h => messenger.MessageRecieved -= h);

            messages
                .Select(evt => evt.EventArgs)
                .Synchronize()
                .Subscribe(msg =>
                {
                    Console.WriteLine("Message {0} arrived", msg);
                    Thread.Sleep(1000);
                    Console.WriteLine("Message {0} exit", msg);
                });

            for (int i = 0; i < 3; i++)
            {
                string msg = "msg" + i;
                ThreadPool.QueueUserWorkItem((_) =>
                {
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
            var messages =
                Observable.FromEventPattern<string>(
                    h => messenger.MessageRecieved += h,
                    h => messenger.MessageRecieved -= h);

            var friendRequests =
                Observable.FromEventPattern<FriendRequest>(
                    h => messenger.FriendRequestRecieved += h,
                    h => messenger.FriendRequestRecieved -= h);

var gate = new object();

messages
    .Select(evt => evt.EventArgs)
    .Synchronize(gate)
    .Subscribe(msg =>
    {
        Console.WriteLine("Message {0} arrived", msg);
        Thread.Sleep(1000);
        Console.WriteLine("Message {0} exit", msg);
    });


friendRequests 
    .Select(evt => evt.EventArgs)
    .Synchronize(gate)
    .Subscribe(request =>
    {
        Console.WriteLine("user {0} sent request", request.UserId);
        Thread.Sleep(1000);
        Console.WriteLine("user {0} approved", request.UserId);
    });
            for (int i = 0; i < 3; i++)
            {
                string msg = "msg" + i;
                string userId = "user" + i;
                ThreadPool.QueueUserWorkItem((_) =>
                {
                    messenger.Notify(msg);
                });

                ThreadPool.QueueUserWorkItem((_) =>
                {
                    messenger.Notify(new FriendRequest() {UserId = userId});
                });
            }

            //waiting for all the other threads to complete before proceeding
            Thread.Sleep(3000);
        }


    }
}
