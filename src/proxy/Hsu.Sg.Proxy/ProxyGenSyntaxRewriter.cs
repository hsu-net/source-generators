namespace Hsu.Sg.Proxy;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
#pragma warning disable S125
#pragma warning disable S3776

internal sealed class ProxyGenSyntaxRewriter(SemanticModel semanticModel, TypeDeclarationSyntax type, Metadata attribute) : CSharpSyntaxRewriter
{
    private readonly ImmutableArray<Diagnostic>.Builder _diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
    private int _counter;

    /// <summary>
    ///     Gets the diagnostics messages.
    /// </summary>
    public ImmutableArray<Diagnostic> Diagnostics => _diagnostics.ToImmutable();

    public int Counter => _counter;

    public override SyntaxNode? VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        return ClearTypeDeclaration(base.VisitInterfaceDeclaration(node));
    }

    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        return ClearTypeDeclaration(base.VisitNamespaceDeclaration(node));
    }

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        return ClearTypeDeclaration(base.VisitClassDeclaration(node));
    }

    public override SyntaxNode? VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        return ClearTypeDeclaration(base.VisitRecordDeclaration(node));
    }

    public override SyntaxNode? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        return ClearTypeDeclaration(base.VisitStructDeclaration(node));
    }
    
    static bool HasSetter(PropertyDeclarationSyntax property)
    {
        // Check if there's a setter (a set accessor) in the property's accessor list.
        return property.AccessorList?.Accessors.Any(accessor => accessor.IsKind(SyntaxKind.SetAccessorDeclaration)) ?? false;
    }

    public override SyntaxNode? VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        return null;
        //return base.VisitFieldDeclaration(node);
    }

    public override SyntaxNode? VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        return null;
        //return base.VisitEventFieldDeclaration(node);
    }

    public override SyntaxNode? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        if (GenMetadata.TryGet(node, semanticModel, out var metadata) && metadata is { Ignore: true }) return null;
        if (!IsGen(node, attribute,metadata?.Ignore)) return null;

        var identifier = node.IsModifier(SyntaxKind.StaticKeyword) ? $"{type.Identifier}.{node.Identifier}" : $"_proxy.{node.Identifier}";
        
        var comments = SyntaxFactory
            .TriviaList()
            .Add( SyntaxFactory
                .Comment($"/// <inheritdoc cref=\"{type.Identifier}.{node.Identifier}\" />"))
            .Add( SyntaxFactory
                .Comment($"/// <remarks>{metadata}</remarks>"));

        var modifiers = node.Modifiers;
        if (IsVirtual(node.Modifiers))
        {
            modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));
        }

        var property = SyntaxFactory
            .PropertyDeclaration(node.Type, node.Identifier)
            .WithModifiers(modifiers)
            .WithLeadingTrivia(comments)
            .WithTrailingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed));

        if (node.IsModifier(SyntaxKind.ReadOnlyKeyword) || !HasSetter(node))
        {
            property = property
                .WithExpressionBody(SyntaxFactory
                    .ArrowExpressionClause(SyntaxFactory
                        .ParseExpression(identifier)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
        else
        {
            var getter = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithExpressionBody(SyntaxFactory
                    .ArrowExpressionClause(SyntaxFactory
                        .ParseExpression(identifier)))
                .WithSemicolonToken(SyntaxFactory
                    .Token(SyntaxKind.SemicolonToken));

            var setter = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithExpressionBody(SyntaxFactory
                    .ArrowExpressionClause(SyntaxFactory
                        .AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(identifier),
                            SyntaxFactory.IdentifierName("value"))))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            property = property
                .AddAccessorListAccessors(getter, setter);
        }

        return property;
    }

    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (GenMetadata.TryGet(node, semanticModel, out var metadata) && metadata is { Ignore: true }) return null;
        if (!IsGen(node, attribute,metadata?.Ignore)) return null;
        
        _counter = Interlocked.Increment(ref _counter);

        // Modifiers
        var modifiers =SyntaxFactory.TokenList();
        if (node.Modifiers.Count > 0)
        {
            var modifiersArray = new List<SyntaxToken>();
            foreach(var modifier in node.Modifiers)
            {
                switch (modifier.Kind())
                {
                    case SyntaxKind.PrivateKeyword:
                    case SyntaxKind.InternalKeyword:
                    case SyntaxKind.PublicKeyword:
                    case SyntaxKind.OverrideKeyword:
                    case SyntaxKind.StaticKeyword:
                    case SyntaxKind.AbstractKeyword:
                    case SyntaxKind.VirtualKeyword:
                        break;
                    default: continue;
                }

                modifiersArray.Add(SyntaxFactory.Token(modifier.Kind()));
            }

            modifiers = new SyntaxTokenList(modifiersArray);
        }

        if (IsVirtual(node.Modifiers))
        {
            modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));
        }

        if (type is InterfaceDeclarationSyntax)
        {
            if (type.IsModifier(SyntaxKind.InterfaceKeyword))
            {
                modifiers = modifiers.Insert(0,SyntaxFactory.Token(SyntaxKind.InternalKeyword));
            }
            else if (type.IsModifier(SyntaxKind.PublicKeyword))
            {
                modifiers = modifiers.Insert(0, SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            }
        }

        // Identifier
        var identifier = node.Identifier;

        // Parameters
        var parameters = string.Empty;
        var parameterTypes = string.Empty;
        if (node.ParameterList.Parameters.Count > 0)
        {
            parameters = string.Join(",", node.ParameterList.Parameters.Select(x => x.Identifier.ToFullString()));
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
                .Comment($"/// <inheritdoc cref=\"{type.Identifier.Text.Trim()}.{node.Identifier.Text.Trim()}({parameterTypes})\" />"))
            .Add( SyntaxFactory
                .Comment($"/// <remarks>{metadata}</remarks>"));
        
        // ConstraintClauses
        var clauses = GetConstraintClauses(node.ConstraintClauses);
        
        // Attributes
        var excludes = new List<string> { GenName };
        if (attribute.Attribute && !(attribute.AttributeIncludes?.Length > 0) && attribute.AttributeExcludes?.Length > 0)
        {
            excludes.AddRange(attribute.AttributeExcludes);
        }
        var attrs = attribute.Attribute 
            ? node.GetAttributeLists(excludes.ToArray(),attribute.AttributeIncludes)
            : SyntaxFactory.List<AttributeListSyntax>();
        
        // Body
        var tpl = node.TypeParameterList?.ToFullString();
        var expression = node.IsModifier(SyntaxKind.StaticKeyword)
            ? SyntaxFactory
                .ParseExpression($"{type.Identifier.Text}.{node.Identifier}{tpl}({parameters})")
            : SyntaxFactory
                .ParseExpression($"_proxy.{node.Identifier}{tpl}({parameters})");

        var body = SyntaxFactory
            .ArrowExpressionClause(expression)
            .WithLeadingTrivia(SyntaxFactory.Tab);

        return SyntaxFactory.MethodDeclaration(node.ReturnType.WithoutTrivia(), identifier)
            .WithModifiers(modifiers)
            .WithAttributeLists(attrs)
            .WithLeadingTrivia(comments)
            .WithTypeParameterList(node.TypeParameterList?.WithoutTrivia()) 
            .WithParameterList(node.ParameterList.WithoutAttributeLists())
            .WithConstraintClauses(clauses)
            .WithExpressionBody(body)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    private bool IsVirtual(SyntaxTokenList modifiers)
    {
        if (attribute.Sealed) return false;
        if (!attribute.Virtual) return false;
        if (modifiers.Any(x =>
                x.IsKind(SyntaxKind.ConstKeyword) ||
                x.IsKind(SyntaxKind.StaticKeyword) ||
                x.IsKind(SyntaxKind.VirtualKeyword) ||
                x.IsKind(SyntaxKind.AbstractKeyword))) return false;

        return true;
    }

    private bool IsGen(MemberDeclarationSyntax node, Metadata sync,bool? ignore)
    {
        var identifier = node switch
        {
            MethodDeclarationSyntax method => $"{type.Identifier.Text}.{method.Identifier.Text}",
            PropertyDeclarationSyntax property => $"{type.Identifier.Text}.{property.Identifier.Text}",
            _ => string.Empty
        };
        
        if (string.IsNullOrWhiteSpace(identifier)) return false;
        if (node.IsModifier(SyntaxKind.PrivateKeyword)) return false;

        if (sync.Only && (ignore == null || ignore.Value)) return false;
        if (!sync.Static && node.IsModifier(SyntaxKind.StaticKeyword)) return false;
        if (!sync.Public && node.IsModifier(SyntaxKind.PublicKeyword)) return false;
        if (!sync.Internal && node.IsModifier(SyntaxKind.InternalKeyword)) return false;

        return true;
    }

    private static SyntaxList<TypeParameterConstraintClauseSyntax> GetConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> list)
    {
        SyntaxList<TypeParameterConstraintClauseSyntax> result = new();
        foreach(var item in list)
        {
            SeparatedSyntaxList<TypeParameterConstraintSyntax> sl = new();
            foreach(var cst in item.Constraints)
            {
                sl = sl.Add(cst.WithoutTrivia());
            }

            result = result.Add(item
                .WithConstraints(sl)
                .WithoutTrivia());
        }

        return result;
    }

    private SyntaxNode? ClearTypeDeclaration(SyntaxNode? node)
    {
        return node is not TypeDeclarationSyntax t || t.Identifier.Text != type.Identifier.Text ? null : node;
    }
}
