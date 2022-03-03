using Microsoft.CodeAnalysis;

namespace ObjectBuildR.Generator.Models;

public struct BuilderToGenerate
{
    public readonly string BuilderName;
    public readonly string EntityToBuild;
    public readonly string EntityToBuildNamespace;
    public readonly IEnumerable<IPropertySymbol> EntityToBuildProperties;
    public readonly string BuildRNamespace;

    public BuilderToGenerate(string builderName,
        string buildRNamespace,
        string entityToBuild,
        string entityToBuildNamespace,
        IEnumerable<IPropertySymbol> entityToBuildProperties)
    {
        BuilderName = builderName;
        EntityToBuild = entityToBuild;
        BuildRNamespace = buildRNamespace;
        EntityToBuildProperties = entityToBuildProperties;
        EntityToBuildNamespace = entityToBuildNamespace;
    }
}