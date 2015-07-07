using System;

namespace FirstRxExample
{
    public class StockTicker
    {
        public event EventHandler<StockTick> StockTick;

        public void Notify(StockTick tick)
        {
            StockTick(this, tick);
        }
    }
}