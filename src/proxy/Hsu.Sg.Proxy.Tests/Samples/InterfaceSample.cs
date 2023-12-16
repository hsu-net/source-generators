using System.ComponentModel;
using System.Threading.Tasks;
using Hsu.Sg.Proxy;
using Hsu.Sg.Sync;
using Cts = System.Threading.CancellationToken;

namespace HsuSgProxyTests.Samples;

[Proxy(Sealed = true, Identifier = "InterfaceIdentity")]
[Sync]
public partial interface InterfaceSample
{
    [DisplayName("AwaitAsync")]
    [System.ComponentModel.Description("AwaitAsync")]
    Task AwaitAsync(int index, string name, Cts token);
}

[Proxy(Only = true)]
[Sync]
public partial interface IOperator2
{
    [DisplayName("Await2Async")]
    [System.ComponentModel.Description("Await2Async")]
    Task Await2Async(int index, string name, Cts token);

    [ProxyGen]
    [DisplayName("OnlyAsync")]
    [System.ComponentModel.Description("OnlyAsync")]
    Task OnlyAsync(int index, string name, Cts token);
}
