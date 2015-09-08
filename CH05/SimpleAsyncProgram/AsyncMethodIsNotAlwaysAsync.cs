using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace SimpleAsyncProgram
{
    class AsyncMethodIsNotAlwaysAsync
    {
        public static async Task AsyncMethodCaller()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Async methods are not always async");

            bool isSame = await MyAsyncMethod(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Caller thread is the same as executing thread: {0}", isSame);//this will print 'true'
        }

        static async Task<bool> MyAsyncMethod(int callingThreadId)
        {
            return Thread.CurrentThread.ManagedThreadId == callingThreadId;
        }

    }
}
