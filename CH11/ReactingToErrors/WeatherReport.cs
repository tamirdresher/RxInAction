namespace ReactingToErrors
{
    internal class WeatherReport 
    {
        public double Temperature { get; set; }
        public string Station { get; set; }

        public override string ToString()
        {
            return string.Format("Station: {0}, Temperature: {1}", Station, Temperature);
        }
    }
}