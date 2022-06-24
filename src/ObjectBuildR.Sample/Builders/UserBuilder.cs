using ObjectBuildR;
using ObjectBuildR.Sample.Entities;

namespace Testing
{
    [BuildRFor(Type = typeof(User))]
    public partial class UserBuilder
    {
        public static UserBuilder Simple()
        {
            var builder = new UserBuilder()
                .WithFirstName("Michael")
                .WithLastName("Hochriegl");

            return builder;
        }
    }
}

