using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace CreationalOpeartors
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateSequence();
            CreatingRangeObservable();
            ReadingAFileWithGenerate();
            CreatingSingleItemObservable();
            CreatingNeverendingObservable();
            CreatingObservableTheThrows();
            CreatingEmptyObservable();
            Console.ReadLine();
        }

        private static void CreatingEmptyObservable()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating an empty observable");

Observable.Empty<string>()
    .SubscribeConsole("Empty");
        }

        private static void CreatingObservableTheThrows()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating observable the throws");

Observable.Throw<ApplicationException>(new ApplicationException("something bad happened"))
    .SubscribeConsole("Throw"); 

        }

        private static void CreatingNeverendingObservable()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating a neverending observable");

Observable.Never<string>()
    .SubscribeConsole("Never"); //nothing will be printed

            Console.WriteLine("No notifications will be pushed, press enter to contine");
        }

        private static void CreatingSingleItemObservable()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating a single item observable");

            Observable.Return("Hello World")
                .SubscribeConsole("Return");

        }


        private static void ReadingAFileWithGenerate()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Generate Sequence from a file");


            //
            // this will read the file, but the created StreamReader wont get disposed when readings complete
            //
            //IObservable<string> lines =
            //    Observable.Generate(
            //        File.OpenText("TextFile.txt"), //beware, this will open the stream but nothing will dispose it
            //        s => !s.EndOfStream,
            //        s => s,
            //        s => s.ReadLine());


            IObservable<string> lines =
                Observable.Using(
                    () => File.OpenText("TextFile.txt"),    // opens the file and returns the stream we work with
                    stream =>
                        Observable.Generate(
                            stream,                 //initial state
                            s => !s.EndOfStream,    //we continue until we reach the end of the file
                            s => s,                 //the stream is our state, it holds the position in the file 
                            s => s.ReadLine())      //each iteration will emit the current line (and moves to the next)
                    );

            lines.SubscribeConsole("lines");
        }

        public static void GenerateSequence()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Generate Sequence");

            IObservable<int> observable =
                Observable.Generate(
                    0,              //initial state
                    i => i < 10,    //condition (false means terminate)
                    i => i + 1,     //next iteration step
                    i => i * 2);      //the value in each iteration

            // this will print the values: 0,2,4,6,8,10,12,14,16,18
            observable.SubscribeConsole("Generate sequence");

        }

        public static void CreatingRangeObservable()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using Range");

            IObservable<int> observable =
                Observable
                    .Range(0, 10)
                    .Select(i => i * 2);

            // this will print the values: 0,2,4,6,8,10,12,14,16,18
            observable.SubscribeConsole("range");

        }


    }
}
