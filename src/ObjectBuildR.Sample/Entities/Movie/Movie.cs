using System.Text.Json;

namespace ObjectBuildR.Sample.Entities.Movie;

public class Movie
{
    public string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public VideoStream VideoStream { get; set; }
    public IEnumerable<AudioStream> AudioStreams { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}