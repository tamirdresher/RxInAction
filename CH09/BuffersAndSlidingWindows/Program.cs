using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Helpers;


namespace BuffersAndSlidingWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            //ZippingTwoObservables();
            //CombiningLatestValues();
            //ConcatTwoAsyncOperations();
            //MergingTwoAsyncOperations();
            //MergingObservableOfObservables();
            //ControllingTheMergingConcurrency();
            Switch();
            //UsingBufferWithAmount();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        private static void Switch()
        {
            Demo.DisplayHeader("The Switch operator - ");

            Subject<string> first=new Subject<string>();
            Subject<string> second=new Subject<string>();
            Subject<IObservable<string>> observables=new Subject<IObservable<string>>();

            observables.Switch()
                .SubscribeConsole();

            observables.OnNext(first);
            observables.OnNext(second);

            first.OnNext("f1");
            observables.OnNext(first);
            second.OnNext("s1");
            first.OnNext("f2");

        }

        private static void ControllingTheMergingConcurrency()
        {
            Demo.DisplayHeader("The Merge operator - you can controll the amount of concurrent subscription made by Merge ");
IObservable<string> first = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i=>"First"+i).Take(2);
IObservable<string> second = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i=> "Second" + i).Take(2);
IObservable<string> third = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i=> "Third" + i).Take(2);
new[] {first,second,third}.ToObservable()
    .Merge(2)
    .RunExample("Merge with 2 concurrent subscriptions");
        }

        private static void MergingObservableOfObservables()
        {
            Demo.DisplayHeader("The Merge operator - allows also to merge observables emitted from another observable");

IObservable<string> texts = ObservableEx.FromValues("Hello", "World");
texts
    .Select(txt => Observable.Return(txt + "-Result"))
    .Merge()
    .SubscribeConsole("Merging from observable");


        }

        private static void MergingTwoAsyncOperations()
        {
            Demo.DisplayHeader("The Merge operator - merges the notifications from the source observables into a single observable sequence");

Task<string[]> facebookMessages = Task.Delay(10).ContinueWith(_ => new[] { "Facebook1", "Facebook2" });//this will finish after 10 milis
Task<string[]> twitterStatuses = Task.FromResult(new[] { "Twitter1", "Twitter2" }); //this will finish immidiatly

Observable.Merge(
    facebookMessages.ToObservable(),
    twitterStatuses.ToObservable())
    .SelectMany(messages => messages)
    .RunExample("Merged Messages");

        }

        private static void ConcatTwoAsyncOperations()
        {
            Demo.DisplayHeader("The Concat operator - Concatenates the second observable sequence to the first observable sequence upon successful termination of the first");

Task<string[]> facebookMessages = Task.Delay(10).ContinueWith(_=>new[] {"Facebook1", "Facebook2"});//this will finish after 10 milis
Task<string[]> twitterStatuses = Task.FromResult(new[] {"Twitter1", "Twitter2"}); //this will finish immidiatly

Observable.Concat(
    facebookMessages.ToObservable(),
    twitterStatuses.ToObservable())
    .SelectMany(messages=>messages)
    .RunExample("Concat Messages");


        }

        private static void CombiningLatestValues()
        {
            Demo.DisplayHeader("The CombineLatest operator - combines values from the observables whenever any of the observable sequences produces an element");

            Subject<int> heartRate = new Subject<int>();
            Subject<int> speed = new Subject<int>();

            speed
                .CombineLatest(heartRate,
                               (s, h) => String.Format("Heart:{0} Speed:{1}", h, s))
                .SubscribeConsole("Metrics");

            heartRate.OnNext(200);
            heartRate.OnNext(201);
            heartRate.OnNext(202);
            speed.OnNext(30);
            speed.OnNext(31);
            heartRate.OnNext(203);
            heartRate.OnNext(204);
        }

        private static void ZippingTwoObservables()
        {
            Demo.DisplayHeader("The Zip operator - combines values with the same index from two observables");

            //temperatures from two sensors (in celsius)
            IObservable<double> temp1 = ObservableEx.FromValues(20.0, 21, 22);
            IObservable<double> temp2 = ObservableEx.FromValues(22.0, 21, 24);

            temp1
                .Zip(temp2, (t1, t2) => (t1 + t2) / 2)
                .SubscribeConsole("Avg Temp.");

        }

        private static void UsingBufferWithAmount()
        {
            Demo.DisplayHeader("The Buffer operator - gather items from an Observable into bundles.");



            IObservable<double> speedReadings = new[] { 50.0, 51.0, 51.5, 53.0, 52.0, 52.5, 53.0 } //in MPH
                .ToObservable();

            double timeDelta = 0.0002777777777777778; //1 second in hours unit

            var accelrations =
    from buffer in speedReadings.Buffer(count: 2, skip: 1)
    where buffer.Count == 2
    let speedDelta = buffer[1] - buffer[0]
    select speedDelta / timeDelta;

            accelrations.RunExample("Acceleration");
        }
    }
}
