namespace Hsu.Sg.FluentMember;

internal static partial class Extensions
{
    public static bool IsActionOrFunc(this TypeSyntax syntax, SemanticModel semanticModel)
    {
        var typeInfo = semanticModel.GetTypeInfo(syntax);
        if (typeInfo.Type?.ContainingNamespace?.ToDisplayString() != SystemNamespace) return false;
        switch (typeInfo.Type.MetadataName)
        {
            case ActionType:
            case GenericActionType:
            case FuncType:
            case GenericFuncType:
                return true;
            default:
                return false;
        }
    }

    public static AttributeData? GetAttributeData<T>(this T syntax, SemanticModel semanticModel, string fullName)
        where T : MemberDeclarationSyntax
    {
        var target = semanticModel.Compilation.GetTypeByMetadataName(fullName);
        //var type = semanticModel.GetDeclaredSymbol(syntax);
        ISymbol type;
        if(syntax is BaseFieldDeclarationSyntax fs)
        {
            type = semanticModel.GetDeclaredSymbol(fs.Declaration.Variables.First());
        }
        else
        {
            type = semanticModel.GetDeclaredSymbol(syntax);	 
        }

        return type?.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass != null && SymbolEqualityComparer.Default.Equals(x.AttributeClass, target));
    }
    
    public static bool IsModifier(this MemberDeclarationSyntax syntax, SyntaxKind modifier)
    {
        return syntax.Modifiers.Any(x => x.IsKind(modifier));
    }
}
