namespace ResourceHandling
{
    internal class SensorData
    {
        public long Data { get; set; }

        public SensorData(long data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("SensorData: {0}", Data);
        }
    }
}