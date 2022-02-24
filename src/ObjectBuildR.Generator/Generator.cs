using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ObjectBuildR.Generator.Generators;
using ObjectBuildR.Generator.Models;

namespace ObjectBuildR.Generator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "BuildRForAttribute.g.cs", 
            SourceText.From(BuildRForAttributeBuilder.BuildBuildRForAttribute().Build(), Encoding.UTF8)));
        
        // Add the builder base class
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "BuildRBase.g.cs",
            SourceText.From(BuildRBaseBuilder.BuildBuilRBase().Build(), Encoding.UTF8)));
        
        // Do a simple filter for classes
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select classes with attributes
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // select the class with the [BuildRFor] attribute
            .Where(static m => m is not null)!; // filter out attributed classes that we don't care about

        // Combine the selected enums with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndEnums
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // Generate the source using the compilation and classes
        // context.RegisterSourceOutput(compilationAndEnums,
        //     static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
    {
        // if (classes.IsDefaultOrEmpty)
        // {
        //     // nothing to do yet
        //     return;
        // }
        //
        // // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        // IEnumerable<ClassDeclarationSyntax> distinctClasses = classes.Distinct();
        //
        // // Convert each EnumDeclarationSyntax to an EnumToGenerate
        // List<BuilderToGenerate> buildersToGenerate = GetTypesToGenerate(compilation, distinctClasses, context.CancellationToken);
        //
        // // If there were errors in the ClassDeclerationSyntax, we won't create an
        // // BuilderToGenerate for it, so make sure we have something to generate
        // if (buildersToGenerate.Count > 0)
        // {
        //     // generate the source code and add it to the output
        //     // string result = SourceGenerationHelper.GenerateExtensionClass(buildersToGenerate);
        //     // context.AddSource("EnumExtensions.g.cs", SourceText.From(result, Encoding.UTF8));
        // }
    }

    private static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        // we know the node is a EnumDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    // weird, we couldn't get the symbol, ignore it
                    continue;
                }

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [EnumExtensions] attribute?
                if (fullName == "ObjectBuildR.BuildRForAttribute")
                {
                    // return the class
                    return classDeclarationSyntax;
                }
            }
        }

        // we didn't find the attribute we were looking for
        return null;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode) 
        => syntaxNode is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;
    
    
}