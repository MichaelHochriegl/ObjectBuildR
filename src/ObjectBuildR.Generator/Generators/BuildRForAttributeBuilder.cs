using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace ObjectBuildR.Generator.Generators;

internal static class BuildRForAttributeBuilder
{
    /// <summary>
    /// Generates the attribute to mark builders with.
    /// </summary>
    /// <returns><see cref="ClassBuilder"/> the attribute builder</returns>
    internal static ClassBuilder BuildBuildRForAttribute()
    {
        var typeType = typeof(Type);

        var builder = CodeBuilder.Create("ObjectBuildR")
            .AddClass("BuildRForAttribute")
            .AddAttribute("AttributeUsage(AttributeTargets.Class)")
            .SetBaseClass("Attribute")
            .AddProperty("Type", Accessibility.Public)
            .SetType(typeType)
            .UseAutoProps();
            // .AddConstructor(Accessibility.Public)
            // .AddParameter("Type", "type")
            // .WithBody(w =>
            // {
            //     w.AppendLine("Type = type;");
            // })
            // .Class;

        return builder;
    }
}