using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace ResourceHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            TraditionalUsingStatement();
            TheUsingOperator();
            DeterministicDisposal();
            Finally();
            FinallyTestCases();
            WeakRerferecneExample();

            Console.WriteLine("Press <Enter> to exit");
            Console.ReadLine();

        }

       


        class DisposableType : IDisposable
        {
            public void Dispose() { /*Freeing the resource*/ }
        }
        private static void TraditionalUsingStatement()
        {
            using (var disposable = new DisposableType())
            {
                //Rest of code
            }
        }

        private static void TheUsingOperator()
        {
            Demo.DisplayHeader("The Using operator - gracefully dispose a resource when the observable terminates");

            string logFilePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "example.log");
            IObservable<SensorData> sensorData =
                Observable.Range(1, 3)
                    .Select(x => new SensorData(x));

            var sensorDataWithLogging =
                Observable.Using(() => new StreamWriter(logFilePath),
                    writer =>
                    {
                        return sensorData.Do(x => writer.WriteLine(x.Data));
                    });

            sensorDataWithLogging.SubscribeConsole("sensor");


        }
        

        private static void DeterministicDisposal()
        {
            Demo.DisplayHeader("The Using operator will make sure that the resource is disposed no matter what caused the observable to stop");

            Subject<int> subject = new Subject<int>();
            var observable =
                Observable.Using(() => Disposable.Create(() => { Console.WriteLine("DISPOSED"); }),
                    _ => subject);

            Console.WriteLine();
            Console.WriteLine("Disposed when completed");
            observable.SubscribeConsole();
            subject.OnCompleted();

            Console.WriteLine();
            Console.WriteLine("Disposed when error occurs");
            subject = new Subject<int>();
            observable.SubscribeConsole();
            subject.OnError(new Exception("error"));

            Console.WriteLine();
            Console.WriteLine("Disposed when subscription disposed");
            subject = new Subject<int>();
            var subscription =
                observable.SubscribeConsole();
            subscription.Dispose();
        }

        private static void WeakRerferecneExample()
        {
            Demo.DisplayHeader("WeakReference");

            object obj =new object();
            WeakReference weak = new WeakReference(obj);

            GC.Collect();
            Console.WriteLine("IsAlive: {0} obj!=null is {1}", weak.IsAlive,obj!=null);

            obj = null;
            GC.Collect();
            Console.WriteLine("IsAlive: {0}", weak.IsAlive);


        }

        private static void Finally()
        {
            IObservable<int> progress =
                Observable.Range(1, 3);
            
            progress
                .Finally(() =>{/*close the window*/})
                .Subscribe(x =>{/*Update the UI*/});

        }

        private static void FinallyTestCases()
        {
            Demo.DisplayHeader("The Finally operator - runs an action when the observable terminates, wither gracefully or due to an error");

            Console.WriteLine();
            Console.WriteLine("Successful complete");
            Observable.Empty<int>()
                .Finally(() => Console.WriteLine("Finally Code"))
                .SubscribeConsole();

            Console.WriteLine();
            Console.WriteLine("Error termination");
            Observable.Throw<Exception>(new Exception("error"))
                .Finally(() => Console.WriteLine("Finally Code"))
                .SubscribeConsole();

            Console.WriteLine();
            Console.WriteLine("Unsubscribing");
            Subject<int> subject=new Subject<int>();
            var subscription =
                subject.AsObservable()
                    .Finally(() => Console.WriteLine("Finally Code"))
                    .SubscribeConsole();
            subscription.Dispose();
            




        }
    }
}
