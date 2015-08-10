using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeriodicAndTimeBasedObservables
{
    class UpdatesWebService
    {
        public async Task<IEnumerable<string>> GetUpdatesAsync()
        {
            Console.WriteLine("GetUpdatesAsync was called");
            await Task.Delay(1000);//simulate latency

            return Enumerable.Range(1, 5).Select(x=>String.Format("An Update {0}, {1}" ,x, DateTime.Now.ToLongTimeString()));
        }
    }
}