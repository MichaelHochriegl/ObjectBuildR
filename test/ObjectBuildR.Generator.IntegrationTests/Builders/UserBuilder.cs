using ObjectBuildR.Generator.IntegrationTests.Entities;

namespace ObjectBuildR.Generator.IntegrationTests.Builders;

[BuildRFor(Type = typeof(User))]
public partial class UserBuilder
{
    public static UserBuilder Simple()
    {
        var builder = new UserBuilder()
            .WithFirstName("Jeffrey")
            .WithLastName("Lebowski");

        return builder;
    }
}