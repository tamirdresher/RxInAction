using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncEnumerables
{
    class QueueItemsEnumerator : IAsyncEnumerator<QueueItem>
    {
        private readonly QueueClient _client;

        public QueueItemsEnumerator(QueueClient client)
        {
            this._client = client;
        }

        public void Dispose()
        {
            this._client.Dispose();
        }

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }
            await Task.Delay(2000);
            this.Current = await this._client.ReadNextItemAsync();
            return true;
        }

        public QueueItem Current { get; private set; }
    }
}
