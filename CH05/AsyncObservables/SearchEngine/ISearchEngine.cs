using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncObservables.SearchEngine
{
    internal interface ISearchEngine
    {
        Task<IEnumerable<string>> SearchAsync(string term);
    }
}