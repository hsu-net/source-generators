namespace Hsu.Sg.Sync;

internal static class Extensions
{
    public static AttributeData? GetAttributeData(this MemberDeclarationSyntax syntax, SemanticModel semanticModel, string fullName)
    {
        var target = semanticModel.Compilation.GetTypeByMetadataName(fullName);
        var type = semanticModel.GetDeclaredSymbol(syntax);

        return type?.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass != null && SymbolEqualityComparer.Default.Equals(x.AttributeClass, target));
    }

    public static SyntaxList<AttributeListSyntax> GetAttributeLists(this MemberDeclarationSyntax syntax,string[]? excludes, params string[]? attributes)
    {
        excludes ??= [];
        attributes ??= [];

        if (syntax.AttributeLists.Count == 0 || attributes.Length == 0 && excludes.Length == 0) return syntax.AttributeLists;
        var attrs = SyntaxFactory.List<AttributeListSyntax>();
        foreach(var attr in syntax.AttributeLists)
        {
            if (attr.Attributes.Count == 0) continue;
            var list = SyntaxFactory.AttributeList();
            foreach(var attribute in attr.Attributes)
            {
                var name = attribute.Name.ToFullString().Trim();
                if (attributes.Length > 0)
                {
                    if (Array.Exists(attributes, a => a == name))
                    {
                        list = list.AddAttributes(attribute);
                    }

                    continue;
                }

                if (excludes.Length > 0 && Array.Exists(excludes, a => a == name)) continue;
                list = list.AddAttributes(attribute);
            }

            if (list.Attributes.Count == 0) continue;
            attrs = attrs.Add(list);
        }

        return attrs;
    }
    
    public static ParameterListSyntax WithoutAttributeLists(this ParameterListSyntax syntax)
    {
        if (syntax.Parameters.Count == 0) return syntax;

        List<ParameterSyntax> list = new();
        foreach(var parameter in syntax.Parameters)
        {
            list.Add(parameter.WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>()));
        }

        return syntax.WithParameters(SyntaxFactory.SeparatedList(list));
    }
    
    public static bool IsModifier(this MemberDeclarationSyntax syntax, SyntaxKind modifier)
    {
        return syntax.Modifiers.Any(x => x.IsKind(modifier));
    }

    public static string[]? GetArray(this TypedConstant constant)
    {
        if (constant.Kind != TypedConstantKind.Array) throw new InvalidCastException();
        if (constant.IsNull || constant.Values.Length == 0) return null;
        return [.. constant.Values.Select(x => x.ToCSharpString().Replace("\"", ""))];
    }
}
