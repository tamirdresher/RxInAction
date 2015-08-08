using System;
using System.Threading.Tasks;

namespace AsyncObservables.Services
{
    class PrimeCheckService
    {
        public virtual async Task<bool> IsPrimeAsync(int number)
        {
            return await Task.Run(() =>
            {
                for (int j = 2; j <= Math.Sqrt(number); j++)
                {
                    if (number % j == 0)
                    {
                        return false;
                    }
                }
                return true;
            });
        }
    }
}