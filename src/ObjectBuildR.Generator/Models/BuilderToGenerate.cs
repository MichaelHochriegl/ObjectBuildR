namespace ObjectBuildR.Generator.Models;

public struct BuilderToGenerate
{
    public readonly string Name;
    public readonly Type EntityToBuild;

    public BuilderToGenerate(string name, Type entityToBuild)
    {
        Name = name;
        EntityToBuild = entityToBuild;
    }
}