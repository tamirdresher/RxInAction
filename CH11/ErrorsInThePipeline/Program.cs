using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace ErrorsInThePipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExceptionDuringSubscription();
            ExceptionInAnOperator();

            //
            // Beware, throwing from the observer will crash your process
            //
            //ExceptionInOnNext();

            Console.ReadLine();
        }

        private static void ExceptionInAnOperator()
        {
            Demo.DisplayHeader("Throwing exception in an operator code - Rx guidleine is that the operator redirect the exception to OnError");

            Observable.Interval(TimeSpan.FromMilliseconds(500))
                 .Do(_ => { throw new Exception("Do"); })
                 .Subscribe(
                     _ => { },
                     e => Console.WriteLine("exception caught: {0}", e.Message));
        }

        private static void ExceptionInOnNext()
        {
            Demo.DisplayHeader("Throwing exception in OnNext() - this will shut down the process, press any key to continue");
            Console.ReadLine();

            Observable.Interval(TimeSpan.FromMilliseconds(500))
                .Subscribe(
                    x => { throw new Exception("Exception in OnNext"); },
                    e =>
                    {
                        /*we wont get here since the exception originated in the observer*/
                    });
        }


        private static void ExceptionDuringSubscription()
        {
            Demo.DisplayHeader("Throwing exception in Subscribe()");

            var observable = Observable.Create<int>(o =>
            {
                throw new Exception("Exception in subscription");
                return Disposable.Empty;
            });
            observable.SubscribeConsole("DuringSubscription");
        }
    }
}
