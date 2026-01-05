// ReSharper disable StringLiteralTypo
using System.Diagnostics;
using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;

namespace Hsu.Sg.FluentMember;

// ReSharper disable IdentifierTypo
#pragma warning disable CS8509
#pragma warning disable S125
#pragma warning disable S3776

internal sealed class GenSyntaxRewriter(SemanticModel semanticModel, Metadata attribute, string typeIdentifier) : CSharpSyntaxRewriter
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

    public override SyntaxNode? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
#if DEBUG
        Debug.WriteLine("VisitFieldDeclaration:");
        Debug.WriteLine("\t-" + node.Modifiers);
        Debug.WriteLine("\t-" + node.Type.ToFullString());
        Debug.WriteLine("\t-" + node.Identifier.ToFullString()); 
#endif
        var @readonly = node.Modifiers.Any(a => a.IsKind(SyntaxKind.ReadOnlyKeyword)) || 
            node.AccessorList == null ||
            node.AccessorList.Accessors.Any(a => 
                a.IsKind(SyntaxKind.InitKeyword) || 
                a.IsKind(SyntaxKind.InitAccessorDeclaration));

#if DEBUG
        Debug.WriteLine("\t-readonly:" + @readonly); 
#endif
        if (@readonly) return null;
        //return base.VisitPropertyDeclaration(node);

        return MethodDeclaration(node,
            node.Identifier.ToFullString(),
            node.Type.ToFullString(),
            node.Type.IsActionOrFunc(semanticModel)
        );
    }

    public override SyntaxNode? VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        Debug.WriteLine("VisitFieldDeclaration:");
        Debug.WriteLine("\t-" + node.Modifiers);
        Debug.WriteLine("\t-" + node.Declaration.Type.ToFullString());
        Debug.WriteLine("\t-" + node.Declaration.Variables.First().Identifier.ToFullString());
        var @readonly = node.Modifiers.Any(a => a.IsKind(SyntaxKind.ReadOnlyKeyword));
        Debug.WriteLine("\t-readonly:" + @readonly);
        if (@readonly) return null;
        //return base.VisitFieldDeclaration(node);
        
        return MethodDeclaration(node,
            node.Declaration.Variables.First().Identifier.ToFullString(),
            node.Declaration.Type.ToFullString(),
            node.Declaration.Type.IsActionOrFunc(semanticModel)
        );
    }

    public override SyntaxNode? VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        Debug.WriteLine("VisitEventFieldDeclaration:");
        Debug.WriteLine("\t-" + node.Modifiers);
        Debug.WriteLine("\t-" + node.Declaration.Type.ToFullString());
        Debug.WriteLine("\t-" + node.Declaration.Variables.First().Identifier.ToFullString());
        var @readonly = node.Modifiers.Any(a => a.IsKind(SyntaxKind.ReadOnlyKeyword));
        Debug.WriteLine("\t-readonly:" + @readonly);
        if (@readonly) return null;
        //return base.VisitEventFieldDeclaration(node);

        return MethodDeclaration(node,
            node.Declaration.Variables.First().Identifier.ToFullString(),
            node.Declaration.Type.ToFullString(),
            true
        );
    }

    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        return null;
    }

    private MethodDeclarationSyntax? MethodDeclaration<T>(T node,string member,string type,bool isEvent=false) where T:MemberDeclarationSyntax
    {
        member = member.Trim();
        if (node.Modifiers.Any(x => x.IsKind(SyntaxKind.ConstKeyword))) return null;
        if (GenMetadata.TryGet(node, semanticModel, out var metadata) && metadata is { Ignore: true }) return null;
        if (!IsGen(node, attribute,metadata?.Ignore, _diagnostics)) return null;

        _counter = Interlocked.Increment(ref _counter);
        
        // Modifiers
        var modifiers = SyntaxFactory.TokenList();
        var modifierTokens = node.Modifiers.Where(x=>!x.IsKind(SyntaxKind.StaticKeyword)).ToArray();
        if (modifierTokens.Length > 0)
        {
            var modifiersArray = new SyntaxToken[modifierTokens.Length];
            for (var i = 0; i < modifiersArray.Length; i++)
            {
                modifiersArray[i] = SyntaxFactory.Token(node.Modifiers[i].Kind());
            }
            
            if (metadata != null && metadata.Modifier != InnerAccessibility.Inherit)
            {
                var replaced = false;
                // if (modifiersArray.Any(x => x.IsKind(SyntaxKind.InternalKeyword)))
                // {
                //     var index = 0;
                //     for (var i = 0; i < modifierTokens.Length; i++)
                //     {
                //         if(replaced) break;
                //         switch (modifiersArray[i].Kind())
                //         {
                //             case SyntaxKind.PrivateKeyword:
                //             case SyntaxKind.ProtectedKeyword:
                //             case SyntaxKind.PublicKeyword:
                //                 Replace(i);
                //                 replaced = true;
                //                 break;
                //             case SyntaxKind.InternalKeyword:
                //                 index = i;
                //                 continue;
                //             default:
                //                 continue;
                //         }
                //     }
                //
                //     if (!replaced)
                //     {
                //         Replace(index);
                //     }
                // }
                
                for (var i = 0; i < modifierTokens.Length; i++)
                {
                    if (replaced) break;
                    switch (modifiersArray[i].Kind())
                    {
                        case SyntaxKind.PrivateKeyword:
                        case SyntaxKind.ProtectedKeyword:
                        case SyntaxKind.InternalKeyword:
                        case SyntaxKind.PublicKeyword:
                            Replace(i);
                            replaced = true;
                            break;
                        default:
                            continue;
                    }
                }

                void Replace(int index)
                {
                    modifiersArray[index] = SyntaxFactory.Token(metadata.Modifier switch
                    {
                        InnerAccessibility.Private => SyntaxKind.PrivateKeyword,
                        InnerAccessibility.Protected => SyntaxKind.ProtectedKeyword,
                        InnerAccessibility.Internal => SyntaxKind.InternalKeyword,
                        InnerAccessibility.Public => SyntaxKind.PublicKeyword
                    });
                }
            }
            
            modifiers = new SyntaxTokenList(modifiersArray);
        }

        // Identifier
        var idx = 0;
        for (var i = 0; i < member.Length; i++)
        {
            if (!char.IsLetter(member, i)) continue;
            idx = i;
            break;
        }

        var id = char.IsLower(member, idx)
            ? $"{char.ToUpper(member[idx])}{member.Substring(idx+1)}"
            : member.Substring(idx);
        
        var name = !string.IsNullOrWhiteSpace(metadata?.Identifier)
            ? metadata!.Identifier.Trim()
            : $"{Prefix()}{id}";
        var identifier = SyntaxFactory.Identifier(name);
        
        var comments = SyntaxFactory
            .TriviaList()
            .Add( SyntaxFactory
                .Comment($"/// <inheritdoc cref=\"{member}\" />"))
            .Add( SyntaxFactory
                .Comment($"/// <remarks>{metadata}</remarks>"));
        
        // ParameterList
        var parameterList = SyntaxFactory
            .SeparatedList<ParameterSyntax>()
            .Add(SyntaxFactory
                .Parameter(SyntaxFactory.Identifier("val"))
                .WithType(SyntaxFactory.ParseTypeName(type)));
        
        if (isEvent)
        {
            parameterList = parameterList.Add(SyntaxFactory
                .Parameter(SyntaxFactory.Identifier("assignable"))
                .WithType(SyntaxFactory.ParseTypeName($"{Namespace}.EventAssignable"))
                .WithDefault(SyntaxFactory
                    .EqualsValueClause(SyntaxFactory
                        .DefaultExpression(SyntaxFactory
                            .ParseTypeName($"{Namespace}.EventAssignable"))))
            );
        }
        
        var parameters = SyntaxFactory
            .ParameterList(parameterList);

        // Body
        StatementSyntax assignment;
        if (isEvent)
        {
            assignment = SyntaxFactory
                .SwitchStatement(SyntaxFactory.IdentifierName("assignable"),
                    SyntaxFactory.List<SwitchSectionSyntax>()
                        .Add(SyntaxFactory.SwitchSection()
                            .AddLabels(SyntaxFactory
                                .CaseSwitchLabel(SyntaxFactory
                                    .IdentifierName($"{Namespace}.EventAssignable.Add")))
                            .AddStatements(SyntaxFactory.ExpressionStatement(SyntaxFactory
                                    .AssignmentExpression(SyntaxKind.AddAssignmentExpression,
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(member)),
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("val")))),
                                SyntaxFactory.BreakStatement()
                            )
                        )
                        .Add(SyntaxFactory.SwitchSection()
                            .AddLabels(SyntaxFactory
                                .CaseSwitchLabel(SyntaxFactory
                                    .IdentifierName($"{Namespace}.EventAssignable.Remove")))
                            .AddStatements(SyntaxFactory.ExpressionStatement(SyntaxFactory
                                    .AssignmentExpression(SyntaxKind.SubtractAssignmentExpression,
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(member)),
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("val")))),
                                SyntaxFactory.BreakStatement()
                            )
                        )
                        .Add(SyntaxFactory.SwitchSection()
                            .AddLabels(SyntaxFactory
                                .CaseSwitchLabel(SyntaxFactory
                                    .IdentifierName($"{Namespace}.EventAssignable.Assign")))
                            .AddStatements(SyntaxFactory.ExpressionStatement(SyntaxFactory
                                    .AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(member)),
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("val")))),
                                SyntaxFactory.BreakStatement()
                            )
                        )
                );
        }
        else
        {
            assignment = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(member)),
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("val"))
                )
            );
        }
        
        var body = SyntaxFactory
            .Block(assignment,
                SyntaxFactory
                    .ReturnStatement(SyntaxFactory
                        .ThisExpression()));

        return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(typeIdentifier), identifier)
            .WithModifiers(modifiers)
            .WithLeadingTrivia(comments)
            .WithParameterList(parameters)
            .WithBody(body);

        string Prefix()
        {
            if (!string.IsNullOrWhiteSpace(metadata?.Prefix)) return metadata!.Prefix;
            if (!string.IsNullOrWhiteSpace(attribute.Prefix)) return attribute.Prefix;
            return !string.IsNullOrWhiteSpace(Generator.GlobalFluentMemberPrefix)
                ? Generator.GlobalFluentMemberPrefix!
                : "With";
        }
    }

    private static bool IsGen(MemberDeclarationSyntax node, Metadata metadata, bool? ignore, ImmutableArray<Diagnostic>.Builder diagnostics)
    {
        if (ignore != null)
        {
            diagnostics.Add(Diagnostic.Create(new DiagnosticDescriptor(
                    "FM101",
                    "Fluent Member Gen",
                    $"{node.GetLocation()} {node.GetText()} ignore:{ignore}",
                    "Usage",
                    DiagnosticSeverity.Info,
                    true),
                node.GetLocation()));
        }
        
        if (metadata.Only && (ignore == null || ignore.Value)) return false;
        if (!metadata.Public && node.IsModifier(SyntaxKind.PublicKeyword)) return false;
        if (!metadata.Internal && node.IsModifier(SyntaxKind.InternalKeyword)) return false;
        if (!metadata.Private && node.IsModifier(SyntaxKind.PrivateKeyword)) return false;

        return true;
    }

    private static SyntaxNode? ClearTypeDeclaration(SyntaxNode? node)
    {
        if (node is not TypeDeclarationSyntax t) return node;

        return t
            .WithTypeParameterList(t.TypeParameterList?.WithoutTrivia())
            .WithConstraintClauses(GetConstraintClauses(t.ConstraintClauses))
            .WithAttributeLists([]);
    }

    private static SyntaxList<TypeParameterConstraintClauseSyntax> GetConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> list)
    {
        SyntaxList<TypeParameterConstraintClauseSyntax> result = [];
        foreach (var item in list)
        {
            SeparatedSyntaxList<TypeParameterConstraintSyntax> sl = [];
            foreach (var cst in item.Constraints)
            {
                sl = sl.Add(cst.WithoutTrivia());
            }

            result = result.Add(item
                .WithConstraints(sl)
                .WithoutTrivia()
            );
        }

        return result;
    }
}