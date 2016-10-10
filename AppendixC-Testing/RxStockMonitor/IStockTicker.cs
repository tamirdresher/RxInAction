using System;

namespace RxStockMonitor
{
    public interface IStockTicker
    {
        event EventHandler<StockTick> StockTick;
    }
}