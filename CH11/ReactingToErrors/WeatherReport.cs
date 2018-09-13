namespace ReactingToErrors
{
    internal class WeatherReport
    {
        public double Temperature { get; set; }
        public string Station { get; set; }

        public override string ToString()
        {
            return System.String.Format("Station: {0}, Temperature: {1}", this.Station, this.Temperature);
        }
    }
}
