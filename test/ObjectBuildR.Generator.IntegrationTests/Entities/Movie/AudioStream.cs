using System.Text.Json;

namespace ObjectBuildR.Generator.IntegrationTests.Entities.Movie;

public class AudioStream
{
    public string Codec { get; set; }
    public int Bitrate { get; set; }
    public int Channel { get; set; }
    public string Language { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}