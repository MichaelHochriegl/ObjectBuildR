using ObjectBuildR.Sample.Entities.Movie;

namespace ObjectBuildR.Sample.Builders;

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