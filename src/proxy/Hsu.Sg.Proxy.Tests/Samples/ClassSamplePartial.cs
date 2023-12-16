using System.Threading.Tasks;
using Hsu.Sg.Proxy;
using Hsu.Sg.Sync;
// ReSharper disable MemberCanBePrivate.Global

namespace HsuSgProxyTests.Samples;

public interface IAsync {}

[Proxy(Interfaces = new []{nameof(IAsync)})]
[Sync(Attribute = true,AttributeIncludes = new []{"Proxy"})]
public partial class ClassSamplePartial
{
    private int IntPropPrivate { get; set; }
    
    [SyncGen]
    [Ignore]
    private Task PrivateTaskAsync()
    {
        return Task.CompletedTask;
    }
}

public partial class ClassSamplePartial
{
    private Task PartialTaskAsync()
    {
        return Task.CompletedTask;
    }
}