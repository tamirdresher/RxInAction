using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncObservables.SearchEngine
{
    class SearchEngineA : ISearchEngine
    {
        public async Task<IEnumerable<string>> SearchAsync(string term)
        {
            Console.WriteLine("SearchEngine A - SearchAsync()");

            await Task.Delay(1500);//simulate latency
            return new[] { "resultA", "resultB" };
        }
    }
}