using ObjectBuildR;
using ObjectBuildR.Sample.Entities;

namespace Testing
{
    [BuildRFor(Type = typeof(User))]
    public partial class UserBuilder
    {
    }
}

