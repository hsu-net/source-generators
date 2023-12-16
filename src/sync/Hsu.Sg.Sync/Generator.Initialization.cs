namespace Hsu.Sg.Sync;

public partial class Generator
{
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

            if (file.EndsWith(".ValueTask.g.cs") || file.EndsWith(".Task.g.cs"))
            {
                if (!ValueTaskTypes.ContainsKey(file)) ValueTaskTypes.Add(file, sourceText);
                continue;
            }

            ctx.AddSource(file, sourceText);
        }
    }
    
    private sealed record TypeSource(TypeDeclarationSyntax Syntax, Metadata Attribute)
    {
        public TypeDeclarationSyntax Syntax { get; } = Syntax;
        public Metadata Attribute { get; } = Attribute;
    }
}