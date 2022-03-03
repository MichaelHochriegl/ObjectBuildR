using System.Collections;
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
        context.RegisterSourceOutput(compilationAndEnums,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }
        
        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        IEnumerable<ClassDeclarationSyntax> distinctClasses = classes.Distinct();
        
        // Convert each EnumDeclarationSyntax to an EnumToGenerate
        List<BuilderToGenerate> buildersToGenerate = GetTypesToGenerate(compilation, distinctClasses, context.CancellationToken);
        
        // If there were errors in the ClassDeclerationSyntax, we won't create an
        // BuilderToGenerate for it, so make sure we have something to generate
        if (buildersToGenerate.Count > 0)
        {
            // generate the source code and add it to the output
            // string result = SourceGenerationHelper.GenerateExtensionClass(buildersToGenerate);
            // context.AddSource("EnumExtensions.g.cs", SourceText.From(result, Encoding.UTF8));
            foreach (var builderToGenerate in buildersToGenerate)
            {
                var builder = BuildRBuilder.BuildBuildR(builderToGenerate);
                context.AddSource($"{builderToGenerate.BuilderName}.g.cs", SourceText.From(builder.Build(), Encoding.UTF8));
            }
        }
    }

    private static List<BuilderToGenerate> GetTypesToGenerate(Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> classes,
        CancellationToken cancellationToken)
    {
        // Create a list to hold our output
        var buildersToGenerate = new List<BuilderToGenerate>();
        // Get the semantic representation of our marker attribute 
        INamedTypeSymbol? builderAttribute = compilation.GetTypeByMetadataName("ObjectBuildR.BuildRForAttribute");

        if (builderAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return buildersToGenerate;
        }

        foreach (ClassDeclarationSyntax classDeclarationSyntax in classes)
        {
            // stop if we're asked to
            cancellationToken.ThrowIfCancellationRequested();

            // Get the semantic representation of the enum syntax
            SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol builderSymbol)
            {
                // something went wrong, bail out
                continue;
            }
            
            // Get the semantic model of the enum symbol
            INamedTypeSymbol enumSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;

            // Set the default extension name
            var entityToBuildName = "object";
            var entityToBuildNamespace = "ObjectBuildR";
            IEnumerable<IPropertySymbol> entityToBuildProperties = null;

            // Loop through all of the attributes on the enum
            foreach (AttributeData attributeData in enumSymbol.GetAttributes())
            {
                if (!builderAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
                {
                    // This isn't the [EnumExtensions] attribute
                    continue;
                }
                
                // This is the attribute, check all of the named arguments
                foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
                {
                    // Is this the ExtensionClassName argument?
                    if (namedArgument.Key == "Type"
                        && namedArgument.Value.Value != null )
                    {
                        var entityToBuild = (ISymbol)namedArgument.Value.Value;
                        if (entityToBuild is null)
                        {
                            // We couldn't get the symbol, so bail out
                            break;
                        }
                        entityToBuildName = entityToBuild.Name;
                        entityToBuildNamespace = entityToBuild.ContainingNamespace.ToString()!;
                        entityToBuildProperties = ((INamedTypeSymbol)entityToBuild).GetMembers()
                            .OfType<IPropertySymbol>()
                            .Where(_ => _.SetMethod is not null &&
                                        _.SetMethod.DeclaredAccessibility == Accessibility.Public);
                    }
                }

                break;
            }

            // Get the full type name of the enum e.g. Colour, 
            // or OuterClass<T>.Colour if it was nested in a generic type (for example)
            string builderClassName = builderSymbol.Name;
            string builderNamespace = builderSymbol.ContainingNamespace.ToString();

            // Get all the members in the enum
            ImmutableArray<ISymbol> classMembers = builderSymbol.GetMembers();
            var members = new List<string>(classMembers.Length);

            // Get all the fields from the enum, and add their name to the list
            foreach (ISymbol member in classMembers)
            {
                if (member is IFieldSymbol field && field.ConstantValue is not null)
                {
                    members.Add(member.Name);
                }
            }

            // Create an EnumToGenerate for use in the generation phase
            buildersToGenerate.Add(new BuilderToGenerate(builderClassName,
                builderNamespace,
                entityToBuildName,
                entityToBuildNamespace,
                entityToBuildProperties));
        }

        return buildersToGenerate;
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
                // #TODO: refactor this code here to be a single line again
                var attSymbolInfo = context.SemanticModel.GetSymbolInfo(attributeSyntax);
                var attSymbol = attSymbolInfo.Symbol;
                if (attSymbol is not IMethodSymbol attributeSymbol)
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