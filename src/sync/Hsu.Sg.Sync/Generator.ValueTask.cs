using System.Collections.Generic;

namespace Hsu.Sg.Sync;

// ReSharper disable InconsistentNaming
#pragma warning disable S2223,S125

public partial class Generator
{
    internal static bool ValueTaskSupported;
    private static bool DefaultImplementationsOfInterfacesSupported;
    private static readonly Dictionary<string, SourceText> ValueTaskTypes = new();

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

        var keys = new List<string> { "SyncHelper.Task.g.cs" };
        if (ValueTaskSupported) keys.Add("SyncHelper.ValueTask.g.cs");
        var sources = ValueTaskTypes
            .Where(x => keys.Exists(a => x.Key.EndsWith(a)))
            .ToDictionary(x => x.Key, x => x.Value);
        
        foreach (var item in sources)
        {
            ctx.AddSource(item.Key, item.Value);
        }
    }
}