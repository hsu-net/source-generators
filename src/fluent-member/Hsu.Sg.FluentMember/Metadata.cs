// ReSharper disable InconsistentNaming

namespace Hsu.Sg.FluentMember;

internal record Metadata
{
    /// <summary>
    ///     The public member are generated.
    /// </summary>
    public bool Public { get; set; } = true;

    /// <summary>
    ///     The internal member are generated.
    /// </summary>
    public bool Internal { get; set; }

    /// <summary>
    ///     The private member are generated.
    /// </summary>
    public bool Private { get; set; }

    /// <summary>
    ///     Only [FluentMemberGen] member are generated.
    /// </summary>
    public bool Only { get; set; }

    /// <summary>
    /// The prefix of member name.
    /// </summary>
    /// <remarks>default is `With`</remarks>
    public string Prefix { get; set; } = string.Empty;
    
    public static bool TryGet(TypeDeclarationSyntax syntax,SemanticModel semanticModel,out Metadata? metadata)
    {
        metadata = null;
        var data = syntax.GetAttributeData(semanticModel, FullAttributeName);
        if (data == null) return false;

        Metadata attribute = new();
        if (data.NamedArguments.Length > 0)
        {
            foreach(var item in data.NamedArguments)
            {
                switch (item.Key)
                {
                    case nameof(Public):
                        attribute.Public = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Internal):
                        attribute.Internal = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Private):
                        attribute.Private = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Only):
                        attribute.Only = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Prefix):
                        attribute.Prefix = item.Value.ToCSharpString().Replace("\"","");
                        break;
                }
            }
        }

        metadata = attribute;
        return true;
    }
}

internal record GenMetadata
{
    /// <summary>
    ///   Ignore member.
    /// </summary>
    public bool Ignore { get; set; }

    /// <summary>
    /// The specific name of member.
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// The prefix of member name.
    /// </summary>
    /// <remarks>default is `With`</remarks>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// The modifier of member
    /// </summary>
    /// <remarks>default is <see cref="InnerAccessibility.Inherit"/></remarks>
    public InnerAccessibility Modifier { get; set; } = InnerAccessibility.Inherit;

    public static bool TryGet<T>(T syntax, SemanticModel semanticModel, out GenMetadata? metadata) where T : MemberDeclarationSyntax
    {
        metadata = null;
        var data = syntax.GetAttributeData(semanticModel, FullGenAttributeName);
        if (data == null) return false;

        GenMetadata attribute = new();
        if (data.NamedArguments.Length > 0)
        {
            foreach(var item in data.NamedArguments)
            {
                switch (item.Key)
                {
                    case nameof(Ignore):
                        attribute.Ignore = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Identifier):
                        attribute.Identifier = item.Value.ToCSharpString().Replace("\"", "");
                        break;
                    case nameof(Prefix):
                        attribute.Prefix = item.Value.ToCSharpString().Replace("\"", "");
                        break;
                    case nameof(Modifier):
                        var str = item.Value.ToCSharpString().Replace("\"", "");
                        attribute.Modifier = Enum.TryParse<InnerAccessibility>(str.Split('.').Last(), true, out var v) ? v : default;
                        break;
                }
            }
        }

        metadata = attribute;
        return true;
    }
}

/// <summary>
/// The accessibility for fluent member set method.
/// </summary>
internal enum InnerAccessibility
{
    /// <summary>
    /// Inherit from the member.
    /// </summary>
    Inherit,
    /// <summary>
    /// Is public access.
    /// </summary>
    Public,
    /// <summary>
    /// Is internal access.
    /// </summary>
    Internal,
    /// <summary>
    /// Is protected access.
    /// </summary>
    Protected,
    /// <summary>
    /// Is private access.
    /// </summary>
    Private
}
