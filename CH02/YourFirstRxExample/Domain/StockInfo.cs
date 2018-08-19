namespace FirstRxExample {
    class StockInfo {
        public StockInfo(string symbol, decimal price) {
            this.Symbol = symbol;
            this.PrevPrice = price;
        }

        public string Symbol { get; set; }
        public decimal PrevPrice { get; set; }
    }
}
