using Hsu.Sg.Sync;
using Cts = System.Threading.CancellationToken;
namespace HsuSgSyncTests.Samples;

[Sync(Definable =true)]
public partial interface IDefinableInterface
{
    Task AwaitAsync(int index, string name, Cts token);
}