// ReSharper disable StringLiteralTypo
using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;

namespace Hsu.Sg.Sync.Rewriter;

// ReSharper disable IdentifierTypo
#pragma warning disable S125
#pragma warning disable S3358
#pragma warning disable S3776

internal sealed class DefinableSyncGenSyntaxRewriter(SemanticModel semanticModel, Metadata attribute, bool isInterface, string identifier)
    : GenSyntaxRewriter(semanticModel, attribute, identifier)
{
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (!_attribute.Definable) return null;
        
        TypeSyntax returnType;
        switch (node.ReturnType)
        {
            case IdentifierNameSyntax:
                {
                    returnType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
                    break;
                }
            case GenericNameSyntax gns:
                {
                    returnType = gns.TypeArgumentList.Arguments[0];
                    break;
                }
            default:
                return null;
        }

        if (GenMetadata.TryGet(node, _semanticModel, out var metadata) && metadata is { Ignore: true }) return null;
        if (!ReturnTypeIsValid(node) || !IsGen(node, _attribute,metadata?.Ignore,isInterface, _diagnostics)) return null;

        _counter = Interlocked.Increment(ref _counter);

        // Modifiers
        var modifiers = SyntaxFactory.TokenList();
        var modifierTokens = node.Modifiers.Where(x => !x.IsKind(SyntaxKind.AsyncKeyword)).ToArray();
        if (node.Modifiers.Count > 0)
        {
            var modifiersArray = new SyntaxToken[modifierTokens.Length];
            for (var i = 0; i < modifiersArray.Length; i++)
            {
                modifiersArray[i] = SyntaxFactory.Token(node.Modifiers[i].Kind());
            }

            modifiers = new SyntaxTokenList(modifiersArray);
        }
        
        // Identifier
        var name = !string.IsNullOrWhiteSpace(metadata?.Identifier)
            ? metadata!.Identifier
            : node.Identifier.Text.EndsWith("Async")
                ? node.Identifier.Text.Substring(0, node.Identifier.Text.Length - 5)
                : $"{node.Identifier.Text}{Suffix(metadata)}";
        
        // Identifier
        var id = SyntaxFactory.Identifier(name);

        // Parameters
        var parameterTypes = string.Empty;
        if (node.ParameterList.Parameters.Count > 0)
        {
            parameterTypes = string.Join(",", node.ParameterList.Parameters
                    .Where(x=>x.Type!=null)
                    .Select(x => x.Type
                        !.WithoutTrivia()
                        .ToFullString()))
                .Replace("<","{")
                .Replace(">","}");
        }
        
        var comments = SyntaxFactory
            .TriviaList()
            .Add( SyntaxFactory
                .Comment($"/// <inheritdoc cref=\"{node.Identifier.Text.Trim()}({parameterTypes})\" />"))
            .Add( SyntaxFactory
                .Comment($"/// <remarks>{metadata}</remarks>"));

        // ConstraintClauses
        var clauses = GetConstraintClauses(node.ConstraintClauses);
        
        // Attributes
        var excludes = new List<string> { GenName };
        if (_attribute.Attribute && !(_attribute.AttributeIncludes?.Length > 0) && _attribute.AttributeExcludes?.Length > 0)
        {
            excludes.AddRange(_attribute.AttributeExcludes);
        }
        var attrs = _attribute.Attribute 
            && (_attribute.AttributeIncludes?.Length > 0 || _attribute.AttributeExcludes?.Length > 0)
            ? node.GetAttributeLists(excludes.ToArray(),_attribute.AttributeIncludes)
            : SyntaxFactory.List<AttributeListSyntax>();
        
        var parameterList = node.ParameterList.WithoutAttributeLists();

        return SyntaxFactory.MethodDeclaration(returnType, id)
            .WithModifiers(modifiers)
            .WithAttributeLists(attrs)
            .WithLeadingTrivia(comments)
            .WithTypeParameterList(node.TypeParameterList)
            .WithParameterList(parameterList)
            .WithConstraintClauses(clauses)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    private static bool IsGen(MethodDeclarationSyntax node, Metadata metadata, bool? ignore, bool isInterface, ImmutableArray<Diagnostic>.Builder diagnostics)
    {
        if (ignore != null)
        {
            diagnostics.Add(Diagnostic.Create(new DiagnosticDescriptor(
                    "HS102",
                    "Method Sync Gen",
                    $"{node.GetLocation()} {node.Identifier.Text} ignore:{ignore}",
                    "Usage",
                    DiagnosticSeverity.Info,
                    true),
                node.GetLocation()));
        }
        
        if (metadata.Only && (ignore == null || ignore.Value)) return false;
        if (metadata.Definable)
        {
            return isInterface || node.IsModifier(SyntaxKind.AbstractKeyword);
        }
        
        if (!metadata.Public && node.IsModifier(SyntaxKind.PublicKeyword)) return false;
        if (!metadata.Internal && node.IsModifier(SyntaxKind.InternalKeyword)) return false;
        if (!metadata.Private && node.IsModifier(SyntaxKind.PrivateKeyword)) return false;

        return true;
    }
}