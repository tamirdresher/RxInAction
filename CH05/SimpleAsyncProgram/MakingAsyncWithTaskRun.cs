using System;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace SimpleAsyncProgram
{
    class MakingAsyncWithTaskRun
    {
        public static async Task AsyncMethodCaller()
        {
            Console.WriteLine();
            Demo.DisplayHeader("Using Task.Run(...) to create async code");

            bool isSame = await MyAsyncMethod(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Caller thread is the same as executing thread: {0}", isSame);//this will print 'false'
        }

        static async Task<bool> MyAsyncMethod(int callingThreadId)
        {
            return await Task.Run(() => Thread.CurrentThread.ManagedThreadId == callingThreadId);
        }

    }
}