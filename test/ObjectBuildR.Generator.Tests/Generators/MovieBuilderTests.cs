namespace ObjectBuildR.Generator.Tests.Generators;

[UsesVerify]
public class MovieBuilderTests
{
    [Fact]
    public Task Should_Generate_MovieBuilder()
    {
        var source = @"using ObjectBuildR;
using ObjectBuildR.Generator.Tests.Models.Movie;
namespace MovieBuildR.Tests
{
[BuildRFor(Type = typeof(Movie))]
public partial class MovieBuilder
{

}
}";

        return TestHelper.Verify(source);
    }
}