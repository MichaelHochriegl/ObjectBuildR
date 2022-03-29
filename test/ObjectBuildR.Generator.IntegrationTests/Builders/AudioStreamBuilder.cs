using ObjectBuildR.Generator.IntegrationTests.Entities.Movie;

namespace ObjectBuildR.Generator.IntegrationTests.Builders;

[BuildRFor(Type = typeof(AudioStream))]
public partial class AudioStreamBuilder
{
    public static AudioStreamBuilder Simple()
    {
        var builder = new AudioStreamBuilder()
            .WithCodec("DTS")
            .WithBitrate(768000)
            .WithChannel(6)
            .WithLanguage("English");

        return builder;
    }
}