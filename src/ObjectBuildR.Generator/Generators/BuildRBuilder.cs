using CodeGenHelpers;
using Microsoft.CodeAnalysis;
using ObjectBuildR.Generator.Models;

namespace ObjectBuildR.Generator.Generators;

internal static class BuildRBuilder
{
    internal static ClassBuilder BuildBuildR(BuilderToGenerate builderToGenerate)
    {
        var builder = CodeBuilder.Create(builderToGenerate.BuildRNamespace)
            .AddClass(builderToGenerate.BuilderName)
            .BuildBuildRClassBody(builderToGenerate)
            .BuildProperties(builderToGenerate)
            .BuildBuildMethod(builderToGenerate);

        return builder;
    }

    private static ClassBuilder BuildBuildRClassBody(this ClassBuilder builder, BuilderToGenerate builderToGenerate)
    {
        builder
            .MakePublicClass()
            .AddNamespaceImport("System")
            .AddNamespaceImport("System.CodeDom.Compiler")
            .AddNamespaceImport("ObjectBuildR")
            .AddNamespaceImport(builderToGenerate.EntityToBuildNamespace)
            .AddNamespaceImport(builderToGenerate.BuildRNamespace)
            .SetBaseClass($"BuildRBase<{builderToGenerate.EntityToBuild}>");

        return builder;
    }

    private static ClassBuilder BuildBuildMethod(this ClassBuilder builder, BuilderToGenerate builderToGenerate)
    {
        builder.AddMethod("Build", Accessibility.Public)
            .Override(true)
            .WithReturnType(builderToGenerate.EntityToBuild)
            .WithBody(w =>
            {
                using (w.Block("if (Object?.IsValueCreated != true)"))
                {
                    // #TODO: add PreBuildProcess hook
                    using (w.Block("Object = new(() =>"))
                    {
                        using (w.Block($"var result = new {builderToGenerate.EntityToBuild}"))
                        {
                            foreach (var propertySymbol in builderToGenerate.EntityToBuildProperties)
                            {
                                var propertyName = propertySymbol.Name;
                                w.AppendLine($"{propertyName} = {propertyName}.Value,");
                            }
                        }
                        w.AppendLine(";");
                        w.AppendLine("return result;");
                    }
                    w.AppendLine(");");
                    // #TODO: add PostBuildProcess hook 
                }
                w.AppendLine("return Object.Value;");
            });
        
        return builder;
    }

    private static ClassBuilder BuildProperties(this ClassBuilder builder,
        BuilderToGenerate builderToGenerate)
    {
        foreach (var propertySymbol in builderToGenerate.EntityToBuildProperties)
        {
            var propertyName = propertySymbol.Name;
            var propertyType = propertySymbol.Type;

            builder
                .BuildLazyProperty(propertyName, propertyType)
                .BuildWithMethodsForProperty(builderToGenerate.BuilderName, propertyName, propertyType);
        }

        return builder;
    }

    private static ClassBuilder BuildLazyProperty(this ClassBuilder builder,
        string propertyName,
        ITypeSymbol propertyType)
    {
        builder.AddProperty(propertyName, Accessibility.Public)
            .SetType($"Lazy<{propertyType}>")
            .WithValue($"new Lazy<{propertyType}>(() => default({propertyType}))");

        return builder;
    }
    
    private static ClassBuilder BuildWithMethodsForProperty(this ClassBuilder builder,
        string builderName,
        string propertyName,
        ITypeSymbol propertyType)
    {
        builder.AddMethod($"With{propertyName}", Accessibility.Public)
            .AddParameter($"Func<{propertyType}>", "func")
            .WithReturnType(builderName)
            .WithBody(w =>
            {
                w.AppendLine($"{propertyName} = new Lazy<{propertyType}>(func);");
                w.AppendLine($"return this;");
            });
            
        builder.AddMethod($"With{propertyName}", Accessibility.Public)
            .AddParameter(propertyType, "value")
            .WithReturnType(builderName)
            .WithBody(w => 
                w.AppendLine($"return With{propertyName}(() => value);"));

        return builder;
    }
}