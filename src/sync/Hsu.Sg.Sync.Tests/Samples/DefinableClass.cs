using Hsu.Sg.Sync;

namespace HsuSgSyncTests.Samples;

[Sync(Definable =true)]
public abstract partial class DefinableClass
{
    public abstract Task AwaitAsync(int index, string name, CancellationToken token);
}

[Sync(Definable =true)]
public abstract partial record DefinableRecord
{
    public abstract Task AwaitAsync(int index, string name, CancellationToken token);
}