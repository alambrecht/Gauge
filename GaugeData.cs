namespace Gauge
{
    public class GaugeData
    {
        public Range[] Ranges { get; set; }
        public double Value { get; set; }
        public string Label { get; set; }
    }

    public class Range
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public string Color { get; set; }
    }
}
