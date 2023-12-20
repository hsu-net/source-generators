namespace Hsu.Sg.Proxy;

internal record Metadata
{
    /// <summary>
    /// The static methods are generated.
    /// </summary>
    public bool Static { get; set; }

    /// <summary>
    ///     The public methods are generated.
    /// </summary>
    public bool Public { get; set; } = true;

    /// <summary>
    ///     The internal methods are generated.
    /// </summary>
    public bool Internal { get; set; } = true;

    /// <summary>
    ///     Only [ProxyGen] methods are generated.
    /// </summary>
    public bool Only { get; set; }

    /// <summary>
    /// To generate abstract proxy class.
    /// </summary>
    public bool Abstract { get; set; }

    /// <summary>
    /// To generate sealed proxy class.
    /// </summary>
    public bool Sealed { get; set; }

    /// <summary>
    /// To generate proxy member with virtual keyword.
    /// </summary>
    public bool Virtual { get; set; } = true;

    /// <summary>
    /// The specific object name of proxy object.
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// The suffix of proxy object name.
    /// </summary>
    /// <remarks>default is `Proxy`</remarks>
    public string Suffix { get; set; } = string.Empty;

    /// <summary>
    /// The name of proxy object inherits.
    /// </summary>
    public string[]? Interfaces { get; set; }

    /// <summary>
    /// Whether generate attributes.
    /// </summary>
    public bool Attribute { get; set; }

    /// <summary>
    /// To generate with attributes
    /// </summary>
    public string[]? AttributeIncludes { get; set; }

    /// <summary>
    /// To generate without attributes
    /// </summary>
    public string[]? AttributeExcludes { get; set; }

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder
            .Append($"{nameof(Only)} = {Only}, ")
            .Append($"{nameof(Abstract)} = {Abstract}, ")
            .Append($"{nameof(Virtual)} = {Virtual}, ")
            .Append($"{nameof(Public)} = {Public}, ")
            .Append($"{nameof(Internal)} = {Internal}, ")
            .Append($"{nameof(Static)} = {Static}, ")
            .Append($"{nameof(Sealed)} = {Sealed}, ");

        if (!string.IsNullOrWhiteSpace(Identifier))
        {
            builder.Append($"{nameof(Identifier)} = {Identifier}, ");
        }

        if (!string.IsNullOrWhiteSpace(Suffix))
        {
            builder.Append($"{nameof(Suffix)} = {Suffix}, ");
        }

        if (Interfaces?.Length > 0)
        {
            builder.Append($"{nameof(Interfaces)} = [{string.Join(",", Interfaces)}], ");
        }

        if (AttributeIncludes?.Length > 0)
        {
            builder.Append($"{nameof(AttributeIncludes)} = [{string.Join(",", AttributeIncludes)}], ");
        }

        if (AttributeExcludes?.Length > 0)
        {
            builder.Append($"{nameof(AttributeExcludes)} = [{string.Join(",", AttributeExcludes)}], ");
        }

        builder.Append($"{nameof(Attribute)} = {Attribute}");
        return true;
    }

    public static bool TryGet(TypeDeclarationSyntax syntax, SemanticModel semanticModel, out Metadata? metadata)
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
                    case nameof(Static):
                        attribute.Static = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Public):
                        attribute.Public = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Internal):
                        attribute.Internal = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Sealed):
                        attribute.Sealed = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Virtual):
                        attribute.Virtual = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Only):
                        attribute.Only = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Abstract):
                        attribute.Abstract = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(Identifier):
                        attribute.Identifier = item.Value.ToCSharpString().Replace("\"", "");
                        break;
                    case nameof(Suffix):
                        attribute.Suffix = item.Value.ToCSharpString().Replace("\"", "");
                        break;
                    case nameof(Interfaces):
                        attribute.Interfaces = item.Value.GetArray();
                        break;
                    case nameof(Attribute):
                        attribute.Attribute = bool.Parse(item.Value.ToCSharpString());
                        break;
                    case nameof(AttributeIncludes):
                        attribute.AttributeIncludes = item.Value.GetArray();
                        break;
                    case nameof(AttributeExcludes):
                        attribute.AttributeExcludes = item.Value.GetArray();
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
    ///   Ignore method.
    /// </summary>
    public bool Ignore { get; set; }

    public static bool TryGet(MemberDeclarationSyntax syntax, SemanticModel semanticModel, out GenMetadata? metadata)
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
                }
            }
        }

        metadata = attribute;
        return true;
    }
}
