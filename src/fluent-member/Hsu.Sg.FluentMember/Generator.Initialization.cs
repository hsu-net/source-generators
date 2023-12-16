using Microsoft.CodeAnalysis.Diagnostics;

namespace Hsu.Sg.FluentMember;

public partial class Generator
{
    internal static bool DefaultImplementationsOfInterfacesSupported;
    internal static string? GlobalFluentMemberPrefix;
    
    private void PostInitializationOutput(IncrementalGeneratorPostInitializationContext ctx)
    {
        var assembly = typeof(Generator).Assembly;
        var names = assembly.GetManifestResourceNames();

        foreach (var name in names)
        {
            using var stream = assembly.GetManifestResourceStream(name);
            if(stream==null) continue;
            var sourceText = SourceText.From(stream, Encoding.UTF8, canBeEmbedded: true);
            var file = name.Replace(".Assets", "");
            ctx.AddSource(file, sourceText);
        }
    }
    
    private static bool CheckTransform(Compilation compilation, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DefaultImplementationsOfInterfacesSupported = compilation
            .SupportsRuntimeCapability(RuntimeCapability.DefaultImplementationsOfInterfaces);
        
        return DefaultImplementationsOfInterfacesSupported;
    }

    private sealed record TypeSource(TypeDeclarationSyntax Syntax, Metadata Attribute)
    {
        public TypeDeclarationSyntax Syntax { get; } = Syntax;
        public Metadata Attribute { get; } = Attribute;
    }
}