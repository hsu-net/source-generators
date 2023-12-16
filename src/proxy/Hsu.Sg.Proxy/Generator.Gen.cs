// ReSharper disable RedundantNameQualifier
using static System.String;

namespace Hsu.Sg.Proxy;

public partial class Generator
{
    private static System.Collections.Generic.IList<TypeSource> ParseAdditionalFiles(Compilation compilation, ImmutableArray<AdditionalText> additions)
    {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8);
        System.Collections.Generic.List<TypeSource> list = new();
        foreach(var file in additions)
        {
            var code = file.GetText();
            if (code == null) continue;
            var syntaxTree = CSharpSyntaxTree.ParseText(code,options);
            var compile = compilation;
            if (!compile.ContainsSyntaxTree(syntaxTree))
            {
                compile = compile.AddSyntaxTrees(syntaxTree);
            }
            var root = syntaxTree.GetCompilationUnitRoot();
            var types = root.DescendantNodes().OfType<TypeDeclarationSyntax>();
            foreach(var type in types)
            {
                try
                {
                    var semanticModel = compile.GetSemanticModel(type.SyntaxTree);
                    var ret = TypeDeclarationTransform(type, semanticModel);
                    if (ret == null) continue;
                    list.Add(ret);
                }
                catch (Exception)
                {
                }
            }
        }

        return list;
    }
    
    private static TypeDeclarationSyntax TypeDeclaration(TypeDeclarationSyntax type, TypeSource st)
    {
        var className = type switch
        {
            InterfaceDeclarationSyntax when type.Identifier.Text.StartsWith("I") && char.IsUpper(type.Identifier.Text[1]) => type.Identifier.Text.Substring(1),
            _ => type.Identifier.Text
        };

        var identifier = !IsNullOrWhiteSpace(st.Attribute.Identifier)
            ? SyntaxFactory.Identifier($"{st.Attribute.Identifier}")
            : SyntaxFactory
                .Identifier($"{className}{Suffix()}");

        var fieldDeclaration = SyntaxFactory
            .FieldDeclaration(SyntaxFactory
                .VariableDeclaration(SyntaxFactory
                    .ParseTypeName(type.Identifier.Text))
                .WithVariables(SyntaxFactory
                    .SingletonSeparatedList(SyntaxFactory
                        .VariableDeclarator("_proxy"))))
            .WithModifiers(SyntaxFactory
                .TokenList(SyntaxFactory
                        .Token(st.Attribute.Sealed ? SyntaxKind.PrivateKeyword : SyntaxKind.ProtectedKeyword),
                    SyntaxFactory
                        .Token(SyntaxKind.ReadOnlyKeyword)
                ));

        var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier("proxy"))
            .WithType(SyntaxFactory.ParseTypeName(type.Identifier.Text));

        var constructor = SyntaxFactory.ConstructorDeclaration(identifier)
            .WithModifiers(SyntaxFactory
                .TokenList(SyntaxFactory.Token(st.Attribute.Abstract
                    ? SyntaxKind.ProtectedKeyword
                    : SyntaxKind.PublicKeyword)))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName("_proxy"),
                    SyntaxFactory.IdentifierName("proxy"))
            ));

        var modifiers = SyntaxFactory
            .TokenList(SyntaxFactory
                .Token(type.IsModifier(SyntaxKind.InternalKeyword)
                    ? SyntaxKind.InternalKeyword
                    : SyntaxKind.PublicKeyword));

        if (st.Attribute.Abstract) modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.AbstractKeyword));
        if (st.Attribute.Sealed) modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

        var interfaces = st.Attribute.Interfaces?.Length > 0
            ? SyntaxFactory
                .BaseList(SyntaxFactory
                    .SeparatedList<BaseTypeSyntax>(st.Attribute.Interfaces
                        .Select(x => SyntaxFactory
                            .SimpleBaseType(SyntaxFactory
                                .ParseTypeName(x)))
                        .ToArray()
                    ))
            : SyntaxFactory.BaseList();

        type = SyntaxFactory
            .ClassDeclaration(identifier)
            .WithMembers(type.Members)
            .WithModifiers(modifiers)
            .AddMembers(fieldDeclaration, constructor);
        
        return interfaces.Types.Count==0 ? type: type.WithBaseList(interfaces);

        string Suffix() => IsNullOrWhiteSpace(st.Attribute.Suffix) ? "Proxy" : st.Attribute.Suffix;
    }

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
            .Add(SyntaxFactory.Comment($"// Generated {counter} proxy methods by ProxyGenerator"))
            .Add(SyntaxFactory.CarriageReturnLineFeed)
            .Add(SyntaxFactory.Comment($"// {st.Attribute}"))
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
