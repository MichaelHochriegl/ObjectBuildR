using System.Runtime.CompilerServices;

namespace ObjectBuildR.Generator.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
    }
}