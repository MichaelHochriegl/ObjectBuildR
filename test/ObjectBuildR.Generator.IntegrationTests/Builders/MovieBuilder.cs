using System;
using System.Collections.Generic;
using ObjectBuildR.Generator.IntegrationTests.Entities.Movie;

namespace ObjectBuildR.Generator.IntegrationTests.Builders;

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