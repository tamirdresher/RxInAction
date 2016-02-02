using System;
using System.Threading.Tasks;

internal class QueueClient : IDisposable
{
    public void Dispose()
    {

    }

    public Task<QueueItem> ReadNextItemAsync()
    {
        return Task.FromResult(new QueueItem() { Data = 1 });
    }
}