namespace ResourceHandling
{
    internal class SensorData
    {
        public long Data { get; set; }

        public SensorData(long data)
        {
            this.Data = data;
        }

        public override string ToString()
        {
            return System.String.Format("SensorData: {0}", this.Data);
        }
    }
}
