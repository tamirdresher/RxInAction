using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncObservables.SearchEngine
{
    class SearchEngineB : ISearchEngine
    {
        public async Task<IEnumerable<string>> SearchAsync(string term)
        {
            Console.WriteLine("SearchEngine B - SearchAsync()");
            await Task.Delay(1500);//simulate latency
            return new[] { "resultC", "resultD" }.AsEnumerable();
        }
    }
}