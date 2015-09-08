using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace SimpleAsyncProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreatingThread();
            //UsingThreadPool();
            //GetPageWithTask();
            //GetPageWithContinuation();
            //ContinuationIsLengthy();
            //GetPageAsync().Wait();
            AsyncMethodIsNotAlwaysAsync.AsyncMethodCaller().Wait();
            MakingAsyncWithTaskRun.AsyncMethodCaller().Wait();
            Console.WriteLine("Press <Enter> to exit");
            Console.ReadLine();
        }




        private static async Task<string> GetPageAsync()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Getting reactivex.io with async-await");

            var httpClient = new HttpClient();

            // The program wont block here, so this method will return right after this call
            var response = await httpClient.GetAsync("http://ReactiveX.io");
            var page = await response.Content.ReadAsStringAsync();
            Console.WriteLine(page);
            return page;

        }

        private static void ContinuationIsLengthy()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Task Continuation can be lengthy");

            var httpClient = new HttpClient();

            // The program wont block here, so this method will return right after this call
            httpClient.GetAsync("http://ReactiveX.io")
                .ContinueWith(requestTask =>
                {
                    HttpContent httpContent = requestTask.Result.Content;
                    httpContent.ReadAsStringAsync()
                        .ContinueWith(contentTask =>
                        {
                            Console.WriteLine(contentTask.Result);
                        });
                });
        }

        private static void GetPageWithTask()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Download headers with Task");

            var httpClient = new HttpClient();
            Task<HttpResponseMessage> requestTask = httpClient.GetAsync("http://ReactiveX.io");

            Console.WriteLine("the request was sent, status:{0}", requestTask.Status);

            Console.WriteLine(requestTask.Result.Headers);
        }

        private static void GetPageWithContinuation()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Download headers with Task Continuation");

            var httpClient = new HttpClient();

            // The program wont block here, so this method will return right after this call
            httpClient.GetAsync("http://ReactiveX.io")
                .ContinueWith(requestTask =>
                {
                    Console.WriteLine("the request was sent, status:{0}", requestTask.Status);
                    Console.WriteLine(requestTask.Result.Headers);
                });


        }

        private static void CreatingThread()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Creating a Thread");

            var thread = new Thread(() =>
            {
                //simulating a long work of five secnods
                Thread.Sleep(TimeSpan.FromSeconds(5));

                Console.WriteLine("Long work is done");
            });

            Console.WriteLine("Starting the long work");
            thread.Start();

            Console.WriteLine("Waiting for long work to finish, but you can press <Enter> any time to continue");
            Console.ReadLine();
        }

        private static void UsingThreadPool()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using the ThreadPool");

            Console.WriteLine("Starting the long work");
            ThreadPool.QueueUserWorkItem((_) =>
            {
                //simulating a long work of five secnods
                Thread.Sleep(TimeSpan.FromSeconds(5));

                Console.WriteLine("Long work is done");
            });

            Console.WriteLine("Waiting for long work to finish, but you can press <Enter> any time to continue");
            Console.ReadLine();
        }
    }
}
