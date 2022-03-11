using ObjectBuildR.Sample.Entities.Movie;

namespace ObjectBuildR.Sample.Builders;

[BuildRFor(Type = typeof(Movie))]
public partial class MovieBuilder
{
    public static MovieBuilder Simple()
    {
        var builder = new MovieBuilder()
            .WithName("The Big Lebowski")
            .WithReleaseDate(DateTime.Parse("15.02.1998"))
            .WithVideoStream(() => VideoStreamBuilder.Simple().Build())
            .WithAudioStreams(() => new List<AudioStream>{AudioStreamBuilder.Simple().Build()});

        return builder;
    }
}