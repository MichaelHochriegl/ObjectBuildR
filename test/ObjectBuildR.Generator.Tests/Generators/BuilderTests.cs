namespace ObjectBuildR.Generator.Tests.Generators;

[UsesVerify]
public class BuilderTests
{
    [Fact]
    public Task Should_Generate_UserBuilder()
    {
        var source = @"using ObjectBuildR;
using ObjectBuildR.Generator.Tests.Models;
namespace Testing
{
[BuildRFor(Type = typeof(ObjectBuildR.Generator.Tests.Models.User))]
public partial class UserBuilder : BuildRBase<User>
{
    public override User Build()
    {
        return new User();
    }
}
}";

        return TestHelper.Verify(source);
    }
}