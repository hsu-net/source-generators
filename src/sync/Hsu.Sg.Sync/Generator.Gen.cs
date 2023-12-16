namespace Hsu.Sg.Sync;

public partial class Generator
{
    private static CompilationUnitSyntax CompilationUnitMerged(CompilationUnitSyntax merged,
        BaseNamespaceDeclarationSyntax nd,
        TypeDeclarationSyntax type,
        SyntaxTriviaList comment,
        int counter,
        TypeSource st
    )
    {
        var us = merged.Usings;
        var nus = us.FirstOrDefault(x => x.Name?.ToFullString() == Namespace);
        if (nus is not null) us = us.Remove(nus);
        merged = merged
            .WithUsings(us)
            .AddMembers(nd.AddMembers(type));

        var comments = comment
            .Add(SyntaxFactory.Comment($"// Generated {counter} sync methods by SyncGenerator"))
            .Add(SyntaxFactory.CarriageReturnLineFeed)
            .Add(SyntaxFactory.Comment($"// {nameof(ValueTaskSupported)} : {ValueTaskSupported}"))
            .Add(SyntaxFactory.CarriageReturnLineFeed)
            .Add(SyntaxFactory.Comment($"// {nameof(DefaultImplementationsOfInterfacesSupported)} : {DefaultImplementationsOfInterfacesSupported}"))
            .Add(SyntaxFactory.CarriageReturnLineFeed)
            .Add(SyntaxFactory.Comment($"// {st.Attribute}"))
            .Add(SyntaxFactory.CarriageReturnLineFeed)
            .Add(SyntaxFactory.CarriageReturnLineFeed);

        merged = merged
            .WithLeadingTrivia(comments
                .Add(SyntaxFactory.CarriageReturnLineFeed)
                .AddRange(merged.GetLeadingTrivia().ToArray()));
        return merged;
    }

    private static CompilationUnitSyntax CompilationUnitFormat(CompilationUnitSyntax merged)
    {
        var formatted = merged.NormalizeWhitespace();
        formatted = formatted
            .WithLeadingTrivia(formatted
                .GetLeadingTrivia()
                .Add(SyntaxFactory.CarriageReturnLineFeed));

        var baseNamespaces = formatted.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
        if (baseNamespaces.Any())
        {
            var typeDeclaration = formatted.DescendantNodes().OfType<TypeDeclarationSyntax>();

            formatted = formatted.ReplaceNodes(typeDeclaration.Take(1), (f, s) => s
                .WithLeadingTrivia(SyntaxFactory
                    .TriviaList(SyntaxFactory.CarriageReturnLineFeed)
                    .AddRange(f.GetLeadingTrivia().ToArray())));
        }

        var methods = formatted.DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();
        formatted = formatted.ReplaceNodes(methods, (f, s) =>
        {
            var leading = f.GetLeadingTrivia().First();
            var list = SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed, leading);

            var trailing = f.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed);
            return s
                .WithExpressionBody(f.ExpressionBody?.WithLeadingTrivia(list.Add(SyntaxFactory.Tab)))
                // .WithLeadingTrivia(f.GetLeadingTrivia().AddRange(list.ToArray()))
                .WithTrailingTrivia(trailing);
        });

        var types = formatted.DescendantNodes().OfType<TypeDeclarationSyntax>().ToArray();
        formatted = formatted.ReplaceNodes(types, (s, f) =>
        {
            var member = f.Members.Last();
            member = member.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            return s.WithMembers(f.Members
                .RemoveAt(f.Members.Count - 1)
                .Add(member));
        });
        return formatted;
    }
}
