namespace ObjectBuildR.Generator.Tests.Models.Movie;

public class Movie
{
    public string Name { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public VideoStream VideoStream { get; set; }
    public IEnumerable<AudioStream> AudioStreams { get; set; }
}