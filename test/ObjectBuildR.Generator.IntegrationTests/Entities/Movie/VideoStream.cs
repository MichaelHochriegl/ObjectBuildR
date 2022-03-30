using System;
using System.Text.Json;

namespace ObjectBuildR.Generator.IntegrationTests.Entities.Movie;

public class VideoStream
{
    public double Framerate { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public AspectRatio AspectRatio { get; set; }
    public TimeSpan Duration { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}