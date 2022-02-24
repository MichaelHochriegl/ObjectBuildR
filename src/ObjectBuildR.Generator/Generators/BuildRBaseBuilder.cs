using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace ObjectBuildR.Generator.Generators;

internal static class BuildRBaseBuilder
{
    internal static ClassBuilder BuildBuilRBase()
    {
        var builder = CodeBuilder.Create("ObjectBuildR")
            .AddClass("ObjectBuildRBase")
            .MakePublicClass()
            .Abstract()
            .AddNamespaceImport("System")
            .AddGeneric("T", genericBuilder => genericBuilder.AddConstraint("class"))
            .AddProperty("Object", Accessibility.Protected)
            .SetType("Lazy<T>")
            .UseAutoProps()
            .AddMethod("Build", Accessibility.Public)
            .WithReturnType("T")
            .Abstract()
            // .WithBody(w => w.AppendLine("return T"))
            .Class
            .AddMethod("WithObject", Accessibility.Public)
            .WithReturnType("ObjectBuildRBase<T>")
            .AddParameter("T", "value")
            .WithBody(w =>
            {
                w.AppendLine("Object = new System.Lazy<T>(value);");
                w.AppendLine("return this;");
            })
            .Class;
            
        return builder;
    }
}