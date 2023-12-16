// ReSharper disable InconsistentNaming

namespace Hsu.Sg.Sync;

internal record Metadata
{
    /// <summary>
    ///     Only <c>interface</c> or <c>abstract</c> async methods are generated.
    /// </summary>
    public bool Definable { get; set; }
    
    /// <summary>
    ///     The public async methods are generated.
    /// </summary>
    public bool Public { get; set; } = true;

    /// <summary>
    ///     The internal async methods are generated.
    /// </summary>
    public bool Internal { get; set; } = true;

    /// <summary>
    ///     The private async methods are generated.
    /// </summary>
    public bool Private { get; set; } = true;

    /// <summary>
    ///     Only [SyncGen] async methods are generated.
    /// </summary>
    public bool Only { get; set; }

    /// <summary>
    /// The suffix of sync method name when not end with Async.
    /// </summary>
    /// <remarks>default is `Sync`</remarks>
    public string Suffix { get; set; } = string.Empty;

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
            .Append($"{nameof(Definable)} = {Definable}, ")
            .Append($"{nameof(Public)} = {Public}, ")
            .Append($"{nameof(Internal)} = {Internal}, ")
            .Append($"{nameof(Private)} = {Private}, ");

        if (!string.IsNullOrWhiteSpace(Suffix))
        {
            builder.Append($"{nameof(Suffix)} = {Suffix}, ");
        }

        if (AttributeIncludes?.Length > 0)
        {
            builder.Append($"{nameof(AttributeIncludes)} = [{string.Join(",", AttributeIncludes)}], ");
        }

        if (AttributeExcludes?.Length > 0)
        {
            builder.Append($"{nameof(AttributeExcludes)} = [{string.Join(",", AttributeExcludes)}], ");
        }

        builder.Append($"{nameof(Attribute)} = {Attribute} ");
        return true;
    }
    
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
                    case nameof(Definable):
                        attribute.Definable = bool.Parse(item.Value.ToCSharpString());
                        break;
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
                    case nameof(Suffix):
                        attribute.Suffix = item.Value.ToCSharpString().Replace("\"","");
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

    /// <summary>
    /// The specific name of sync method.
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// The suffix of sync method name when not end with Async.
    /// </summary>
    /// <remarks>default is `Sync`</remarks>
    public string Suffix { get; set; } = string.Empty;

    public static bool TryGet(MethodDeclarationSyntax syntax, SemanticModel semanticModel, out GenMetadata? metadata)
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
                    case nameof(Suffix):
                        attribute.Suffix = item.Value.ToCSharpString().Replace("\"", "");
                        break;
                }
            }
        }

        metadata = attribute;
        return true;
    }
}