using System.Threading.Tasks;

namespace AsyncObservables.Services
{
    class VariableTimePrimeCheckService : PrimeCheckService
    {
        private readonly int _numberToDelay;

        public VariableTimePrimeCheckService(int numberToDelay)
        {
            _numberToDelay = numberToDelay;
        }

        public override async Task<bool> IsPrimeAsync(int number)
        {
            if (number == _numberToDelay)
            {
                await Task.Delay(2000);
            }
            return await base.IsPrimeAsync(number);
        }
    }
}