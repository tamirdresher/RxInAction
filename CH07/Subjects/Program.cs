using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace Subjects
{
    class Program
    {
        static void Main(string[] args)
        {
            //SubjectExample();
            //ManySourcesOneCompletionExample();
            //MergingBySubjectPitfall();

            //AsyncSubjectExample();
            TaskToObservableWithAsyncSubject();

            //BehaviorSubjectExample();

            //ReplaySubjectExample();
            ReplaySubjectToKeepHeartRateExample();
            //NotHidingYourSubjects();
            //HidingYourSubjects();

            Console.ReadLine();

        }

        private static void HidingYourSubjects()
        {
            Demo.DisplayHeader("Returning the subject instance can break your encapsulation");

            Subject<int> sbj = new Subject<int>();
            sbj.SubscribeConsole();
            var proxy = sbj.AsObservable();
            var subject = proxy as Subject<int>;
            var observer = proxy as IObserver<int>;
            Console.WriteLine("proxy as subject is {0}", subject == null ? "null" : "not null");
            Console.WriteLine("proxy as observer is {0}", observer == null ? "null" : "not null");

        }

        private static void NotHidingYourSubjects()
        {
            Demo.DisplayHeader("Returning the subject instance can break your encapsulation");

            var acct = new BankAccount();
            acct.MoneyTransactions.SubscribeConsole("Transferring");

            var hackedSubject = acct.MoneyTransactions as Subject<int>;

            // i stole all its money
            hackedSubject.OnNext(-9999);
        }

        private static void ReplaySubjectExample()
        {
            Demo.DisplayHeader("ReplaySubject - publish all notifications to current and future observers");

            ReplaySubject<int> sbj = new ReplaySubject<int>(bufferSize: 3, window: TimeSpan.FromSeconds(1));

            sbj.OnNext(1);
            sbj.OnNext(2);
            sbj.OnNext(3);
            sbj.OnNext(4);
            var subscription = sbj.SubscribeConsole("Replay"); //only 2,3,4 will be observed
            subscription.Dispose();

            Console.WriteLine("Sleeping for 1 sec");
            Thread.Sleep(1000);
            sbj.OnNext(5);


            sbj.SubscribeConsole("Replay2"); //only 5 will be observed - the only value in the last second




        }

        private static void ReplaySubjectToKeepHeartRateExample()
        {
            Demo.DisplayHeader("ReplaySubject - simulating the usage of RxToBand for showin the heart rate - https://github.com/Reactive-Extensions/RxToBand");

IObservable<int> heartRate = Observable.Range(70, 5);
ReplaySubject<int> sbj = new ReplaySubject<int>(bufferSize: 20, window: TimeSpan.FromMinutes(2));

heartRate.Subscribe(sbj);

// after a while (for example, after the user selected to show the heart rate on the screen)
var subscription = sbj.SubscribeConsole("HeartRate Graph"); //only 70-74 will be observed
        }


        private static void BehaviorSubjectExample()
        {
            Demo.DisplayHeader("BehaviorSubject - initialized with a default value and has a memory of the latest value");

            //NetworkConnectivity:  Connected or Disconnected
            BehaviorSubject<NetworkConnectivity> connection = new BehaviorSubject<NetworkConnectivity>(NetworkConnectivity.Disconnected);
            connection.SubscribeConsole("first");
            connection.OnNext(NetworkConnectivity.Connected);
            connection.SubscribeConsole("second");
            Console.WriteLine("Connection is {0}", connection.Value);
        }

        private static void AsyncSubjectExample()
        {
            Demo.DisplayHeader("AsyncSubject - will emit only the last value it observed");

            AsyncSubject<int> sbj = new AsyncSubject<int>();

            sbj.OnNext(1);
            sbj.OnNext(2);
            sbj.OnCompleted();

            sbj.SubscribeConsole();
        }

        private static void TaskToObservableWithAsyncSubject()
        {
            Demo.DisplayHeader("AsyncSubject - the Task.ToObservable() operator uses AsyncSubject");
            var tcs = new TaskCompletionSource<bool>();
            var task = tcs.Task;

            AsyncSubject<bool> sbj = new AsyncSubject<bool>();
            task.ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        sbj.OnNext(t.Result);
                        sbj.OnCompleted();
                        break;
                    case TaskStatus.Faulted:
                        sbj.OnError(t.Exception.InnerException);
                        break;
                    case TaskStatus.Canceled:
                        sbj.OnError(new TaskCanceledException(t));
                        break;
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            tcs.SetResult(true);//making the Task complete before the observer subscribed
            sbj.SubscribeConsole();
        }


        private static void SubjectExample()
        {

            Demo.DisplayHeader("Subject - publish the notification it observe");

            Subject<int> sbj = new Subject<int>();

            sbj.SubscribeConsole("First");
            sbj.SubscribeConsole("Second");

            sbj.OnNext(1);
            sbj.OnNext(2);
            sbj.OnCompleted();


        }

        static void MergingBySubjectPitfall()
        {
            Demo.DisplayHeader("Subject - Merging with a Subject leads to confusion - the LiveMessages will not be observed");

            Subject<string> sbj = new Subject<string>();

            sbj.SubscribeConsole();


            IEnumerable<string> messagesFromDb = new[] { "DBMessage1", "DBMessage2" };
            IObservable<string> realTimeMessages =
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => "LiveMessage" + x);


            messagesFromDb.ToObservable().Subscribe(sbj);//we can omit the ToObservable() since there is builtin "Subscribe" extension method 
            realTimeMessages.Subscribe(sbj);//the messages will not be observed since the first observable already completed
        }

        private static void ManySourcesOneCompletionExample()
        {

            Demo.DisplayHeader("Subject - even with many source, publish the notification it observe, but stops after OnCompleted was called");

            Subject<string> sbj = new Subject<string>();

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(x => "First: " + x)
                .Take(5)
                .Subscribe(sbj);
            Observable.Interval(TimeSpan.FromSeconds(2))
                .Select(x => "Second: " + x)
                .Take(5)
                .Subscribe(sbj);

            sbj.RunExample("");
        }
    }
}
