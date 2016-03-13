using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncEnumerables
{
    class QueueItemsEnumerator : IAsyncEnumerator<QueueItem>
    {
        private QueueClient _client;

        public QueueItemsEnumerator(QueueClient client)
        {
            _client = client;
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }
            await Task.Delay(2000);
            Current = await _client.ReadNextItemAsync();
            return true;
        }

        public QueueItem Current { get; private set; }
    }
}