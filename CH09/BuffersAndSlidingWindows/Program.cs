using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;


namespace BuffersAndSlidingWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            

            

           

            //UsingBufferWithAmount();
            BufferingHiRateChatMessages();
            //Window();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

      


      


       
       

        private static void UsingBufferWithAmount()
        {
            Demo.DisplayHeader("The Buffer operator - gather items from an Observable into bundles.");
            
            IObservable<double> speedReadings = new[] {50.0, 51.0, 51.5, 53.0, 52.0, 52.5, 53.0} //in MPH
                .ToObservable();

            double timeDelta = 0.0002777777777777778; //1 second in hours unit

            var accelrations =
                from buffer in speedReadings.Buffer(count: 2, skip: 1)
                where buffer.Count == 2
                let speedDelta = buffer[1] - buffer[0]
                select speedDelta/timeDelta;

            accelrations.RunExample("Acceleration");
        }
        private static void BufferingHiRateChatMessages()
        {
            Demo.DisplayHeader("The Buffer operator - can be used to slow high-rate stream by taking it by chunks");

var coldMessages = Observable.Interval(TimeSpan.FromMilliseconds(50))
    .Take(4)
    .Select(x => "Message " + x);

IObservable<string> messages =
    coldMessages.Concat(
         coldMessages.DelaySubscription(TimeSpan.FromMilliseconds(200)))
        .Publish()
        .RefCount();


messages.Buffer(messages.Throttle(TimeSpan.FromMilliseconds(100)))
    .SelectMany((b, i) => b.Select(m => string.Format("Buffer {0} - {1}", i, m)))
    .RunExample("Hi-Rate Messages");

        }

        private static void Window()
        {
            //var numbers = Observable.Range(1, 10);
            //numbers.Window()
        }
    }
}
