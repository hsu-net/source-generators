namespace Hsu.Sg.FluentMember;

/// <summary>
///    To generate fluent member method.
/// </summary>
[Generator(LanguageNames.CSharp)]
public partial class Generator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        #if FM_GEN_DEBUG
        if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();
        #endif

        // Add the marker attribute to the compilation.
        context.RegisterPostInitializationOutput(PostInitializationOutput);
        
        // GlobalOptions
        context.RegisterImplementationSourceOutput(context.AnalyzerConfigOptionsProvider, static (_, analyzer) =>
        {
            analyzer.GlobalOptions.TryGetValue(FluentMemberPrefix,out var prefix);
            GlobalFluentMemberPrefix = prefix;
            Console.WriteLine($"{nameof(GlobalFluentMemberPrefix)}:{GlobalFluentMemberPrefix}");
        });
        
        // Check DefaultImplementationsOfInterfacesSupported
        var vts = context
            .CompilationProvider
            .Select(CheckTransform);
        context.RegisterSourceOutput(vts, static (ctx, _) =>
        {
            Console.WriteLine($"{nameof(DefaultImplementationsOfInterfacesSupported)}:{DefaultImplementationsOfInterfacesSupported}");
        });
        
        // Filter classes annotated with the [FluentMember] attribute. Only filtered Syntax Nodes can trigger code generation.
        /*
         * Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax
         *   - Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax
         *   - Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax
         *   - Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax
         *   - Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax
         */

        var sources = context
            .SyntaxProvider
            .CreateSyntaxProvider((s, _) => s is TypeDeclarationSyntax, TypeDeclarationTransform)
            .Where(t => t is not null)
            .Select<TypeSource?, TypeSource>((x, _) => x!)
            .Collect();

        var collect = context.CompilationProvider.Combine(sources);
        // Generate the source code.
        context.RegisterSourceOutput(collect, GenerateCode);
    }

    private static TypeSource? TypeDeclarationTransform(GeneratorSyntaxContext ctx, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var typeDeclarationSyntax = (TypeDeclarationSyntax)ctx.Node;
        if (!Metadata.TryGet(typeDeclarationSyntax, ctx.SemanticModel, out var attribute) || attribute == null) return null;
        return new TypeSource(typeDeclarationSyntax, attribute);
    }

    private static void GenerateCode(SourceProductionContext ctx, (Compilation Compilation, ImmutableArray<TypeSource> Syntaxes) source)
    {
        var comment = SyntaxFactory
            .ParseLeadingTrivia("// <auto-generated/>")
            .Add(SyntaxFactory.CarriageReturnLineFeed);

        // Go through all filtered type declarations.
        foreach(var group in source.Syntaxes.GroupBy(x=>x.Syntax.Identifier.Text))
        {
            var st = group.First();
            if (!st.Syntax.IsModifier(SyntaxKind.PartialKeyword))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(PartialDiagnosticDescriptor, st.Syntax.GetLocation()));
                continue;
            }
            
            var counter = 0;
            var merged = SyntaxFactory.CompilationUnit();
            TypeDeclarationSyntax type = null!;
            BaseNamespaceDeclarationSyntax nd = null!;
            string typeIdentifier=null!;
            
            foreach(var item in group)
            {
                var root = item.Syntax.SyntaxTree.GetRoot();
                if (root is not CompilationUnitSyntax compilation) continue;

                // We need to get semantic model of the class to retrieve metadata.
                var semanticModel = source.Compilation.GetSemanticModel(root.SyntaxTree);

                // Symbols allow us to get the compile-time information.
                if (semanticModel.GetDeclaredSymbol(item.Syntax) is not { } symbol) continue;
                typeIdentifier = symbol.MetadataName;

                var rewriter = new GenSyntaxRewriter(semanticModel, item.Attribute,symbol.ToDisplayString().Trim());
                if (rewriter.Visit(item.Syntax) is not TypeDeclarationSyntax node || rewriter.Counter == 0) continue;
                counter += rewriter.Counter;
                
                foreach(var diagnostic in rewriter.Diagnostics)
                {
                    ctx.ReportDiagnostic(diagnostic);
                }
                
                if (compilation.Usings.Count > 0)
                {
                    foreach(var @using in compilation.Usings)
                    {
                        if (merged.Usings.Any(a => a.Name==null || a.Name?.ToFullString() == @using.Name?.ToFullString())) continue;
                        merged = merged.AddUsings(@using.WithoutTrivia());
                    }
                }

                BaseNamespaceDeclarationSyntax ns = root
                    .DescendantNodes()
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .First();
                TypeDeclarationSyntax t = node
                    .RemoveNodes(node
                            .DescendantNodes()
                            .OfType<TypeDeclarationSyntax>(),
                        SyntaxRemoveOptions.KeepEndOfLine)!;

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (type == null)
                {
                    type = t
                        .WithoutTrivia()
                        .WithTypeParameterList(t.TypeParameterList?.WithoutTrivia())
                        .WithConstraintClauses(t.ConstraintClauses)
                        .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>());
                    
                    nd = ns
                        .WithoutTrivia()
                        .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>())
                        .WithUsings(SyntaxFactory.List<UsingDirectiveSyntax>());
                }

                if (ns.Usings.Count > 0)
                {
                    foreach(var @using in ns.Usings)
                    {
                        if (nd.Usings.Any(a => a.Name==null || a.Name?.ToFullString() == @using.Name?.ToFullString())) continue;
                        nd = nd.AddUsings(@using.WithoutTrivia());
                    }
                }

                type = type.AddMembers(t.Members.ToArray());
            }
            
            if (counter == 0) continue;
            
            type = SyntaxFactory
                .TypeDeclaration(type.Kind(), type.Identifier)
                .WithMembers(SyntaxFactory
                    .List<MemberDeclarationSyntax>(type
                        .DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                        .ToArray()
                    ))
                .WithTypeParameterList(type.TypeParameterList)
                .WithConstraintClauses(type.ConstraintClauses)
                .WithModifiers(type.Modifiers);

            merged = CompilationUnitMerged(merged, nd, type, comment, counter, st);
            var formatted = CompilationUnitFormat(merged);
            ctx.AddSource($"{nd.Name.ToFullString()}.{typeIdentifier}.{GenSuffix}.g.cs",
                SourceText.From(formatted.ToFullString(), Encoding.UTF8));
        }
    }
}
