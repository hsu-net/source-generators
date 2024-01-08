using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Hsu.Sg.Sync.Rewriter;

// ReSharper disable IdentifierTypo
#pragma warning disable S125

internal abstract class GenSyntaxRewriter(SemanticModel semanticModel, Metadata attribute, string identifier) : CSharpSyntaxRewriter
{
    protected readonly Metadata _attribute = attribute;
    protected readonly string _identifier = identifier;
    protected readonly ImmutableArray<Diagnostic>.Builder _diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
    protected readonly SemanticModel _semanticModel = semanticModel;
    protected int _counter = 0;

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

    protected static SyntaxList<TypeParameterConstraintClauseSyntax> GetConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> list)
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

    private static SyntaxNode? ClearTypeDeclaration(SyntaxNode? node)
    {
        if (node is not TypeDeclarationSyntax t) return node;

        return t
            .WithTypeParameterList(t.TypeParameterList?.WithoutTrivia())
            .WithConstraintClauses(GetConstraintClauses(t.ConstraintClauses))
            .WithAttributeLists([]);
    }
    
    protected string Suffix(GenMetadata? attribute)
    {
        if(!string.IsNullOrWhiteSpace(attribute?.Suffix)) return  attribute!.Suffix;
        return !string.IsNullOrWhiteSpace(_attribute.Suffix) ? _attribute.Suffix : "Sync";
    }
    
    protected bool ReturnTypeIsValid(MethodDeclarationSyntax node)
    {
        var typeInfo = _semanticModel.GetTypeInfo(node.ReturnType);
        //if (typeInfo.Type?.ContainingNamespace == null) return false;
        if (typeInfo.Type?.ContainingNamespace?.ToDisplayString() != TaskNamespace) return false;
        switch (typeInfo.Type.MetadataName)
        {
            case TaskType:
            case GenericTaskType:
                return true;
            case ValueTaskType:
            case GenericValueTaskType:
                break;
            default:
                return false;
        }

        if (Generator.ValueTaskSupported) return true;
        _diagnostics.Add(Diagnostic.Create(new DiagnosticDescriptor(
                "HS101",
                "ValueTask not supported",
                $"{node.GetLocation()} {node.Identifier.Text} not include `{ValueTaskType}`",
                "Usage",
                DiagnosticSeverity.Warning,
                true),
            node.GetLocation()));

        return false;
    }
}