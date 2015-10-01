# Gauge
A simple .NET gauge generator

Create a gauge image from a group of ranges and values

Example of usage:


```
private static void Main()
{
    var data = new GaugeData
    {
        Ranges = new[]
        {
            new Range{Color = "#00FF00", MinValue = 0, MaxValue = 33}, 
            new Range{Color = "#FFFF00", MinValue = 34, MaxValue = 66}, 
            new Range{Color = "#FF0000", MinValue = 67, MaxValue = 100}
        },
        Value = 45,
        Label = "label"
    };
    File.WriteAllBytes(@"C:\testing.png", data.Generate(200, ImageFormat.Png));
}
```

Which outputs:

![alt text](https://raw.githubusercontent.com/alambrecht/Gauge/master/testing.png "Sample gauge")

