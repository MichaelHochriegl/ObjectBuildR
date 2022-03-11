using ObjectBuildR.Sample.Entities.Movie;

namespace ObjectBuildR.Sample.Builders;

[BuildRFor(Type = typeof(VideoStream))]
public partial class VideoStreamBuilder
{
    public static VideoStreamBuilder Simple()
    {
        var builder = new VideoStreamBuilder()
            .WithFramerate(23.976)
            .WithWidth(1920)
            .WithHeight(800)
            .WithAspectRatio(Entities.Movie.AspectRatio.CinemaScope)
            .WithDuration(new TimeSpan(1, 57, 00));

        return builder;
    }
}