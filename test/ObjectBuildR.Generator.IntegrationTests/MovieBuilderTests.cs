using System;
using System.Collections.Generic;
using FluentAssertions;
using ObjectBuildR.Generator.IntegrationTests.Builders;
using ObjectBuildR.Generator.IntegrationTests.Entities.Movie;
using Xunit;

namespace ObjectBuildR.Generator.IntegrationTests;

public class MovieBuilderTests
{
    [Fact]
    public void Should_Build_Simple_Movie()
    {
        #region Arrange

        var videoStream = new VideoStream
        {
            Framerate = 23.976,
            Width = 1920,
            Height = 800,
            AspectRatio = AspectRatio.CinemaScope,
            Duration = new TimeSpan(1, 57, 00)
        };
        var audioStreams = new List<AudioStream> { new AudioStream
        {
            Codec = "DTS",
            Bitrate = 768000,
            Channel = 6,
            Language = "English"
        }};
        var expectedMovie = new Movie
        {
            Name = "The Big Lebowski",
            ReleaseDate = DateTime.Parse("15.02.1998"),
            VideoStream = videoStream,
            AudioStreams = audioStreams
        };
        var sut = MovieBuilder.Simple();

        #endregion

        #region Act

        var result = sut.Build();

        #endregion

        #region Assert

        result.Should().BeEquivalentTo(expectedMovie);

        #endregion
    }
}