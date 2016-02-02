using System.Collections.Generic;

namespace AsyncEnumerables
{
    class QueueItems : IAsyncEnumerable<QueueItem>
    {
        public IAsyncEnumerator<QueueItem> GetEnumerator()
        {
            return new QueueItemsEnumerator(new QueueClient());
        }
    }
}