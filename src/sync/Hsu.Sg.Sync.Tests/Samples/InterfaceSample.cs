using Hsu.Sg.Sync;
using Cts = System.Threading.CancellationToken;

namespace HsuSgSyncTests.Samples;

[Sync]
public partial interface InterfaceSample
{
    Task AwaitAsync(int index, string name, Cts token);
}

[Sync(Only = true)]
public partial interface IOperator2
{
    Task Await2Async(int index, string name, Cts token);

    [SyncGen]
    Task OnlyAsync(int index, string name, Cts token);
}