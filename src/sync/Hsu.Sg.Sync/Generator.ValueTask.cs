namespace Hsu.Sg.Sync;

// ReSharper disable InconsistentNaming
#pragma warning disable S2223,S125

public partial class Generator
{
    internal static bool ValueTaskSupported;
    private static bool DefaultImplementationsOfInterfacesSupported;
    private static readonly Dictionary<string, SourceText> ValueTaskTypes = [];

    private static (INamedTypeSymbol? ValTask, IAssemblySymbol Assembly) ValueTaskTransform(Compilation compilation, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DefaultImplementationsOfInterfacesSupported = compilation
            .SupportsRuntimeCapability(RuntimeCapability.DefaultImplementationsOfInterfaces);

        return (compilation.GetTypeByMetadataName($"{TaskNamespace}.{ValueTaskType}"), compilation.Assembly);
    }

    private static void GenerateValueTaskCode(SourceProductionContext ctx, (INamedTypeSymbol? Symbol, IAssemblySymbol Assembly) source)
    {
        ValueTaskSupported = source.Symbol != null;
        if (!ValueTaskSupported) return;

        foreach (var item in ValueTaskTypes) ctx.AddSource(item.Key, item.Value);
    }
}