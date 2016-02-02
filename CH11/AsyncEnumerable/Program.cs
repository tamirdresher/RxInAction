using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncEnumerables
{
    class Program
    {
    
        static void Main(string[] args)
        {
            IAsyncEnumerable<Task<int>> asyncEnumerable = Enumerable.Empty<Task<int>>().ToAsyncEnumerable();
            IAsyncEnumerable<int> enumerable = Task.FromResult(4).ToAsyncEnumerable();

            var queueItems = new QueueItems();
            //.Select(async i => 
            //{
            //    Console.WriteLine("inside "+i);
            //    await Task.Delay(2000);
            //    return i;
            //})
            queueItems
            .Do(x => Console.WriteLine(DateTime.Now))
            .ForEachAsync(x => Console.WriteLine("out " + x));


            Console.ReadLine();

        }
    }

    
}