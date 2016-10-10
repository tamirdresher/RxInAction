using System;

namespace FirstRxExample
{
    public interface IStockTicker
    {
        event EventHandler<StockTick> StockTick;
    }
}