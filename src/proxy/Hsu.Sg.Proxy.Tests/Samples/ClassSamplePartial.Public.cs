// ReSharper disable MemberCanBePrivate.Global

using System.ComponentModel;
using System.Threading.Tasks;

namespace HsuSgProxyTests.Samples;

public partial class ClassSamplePartial
{
    public int IntProp { get; set; }
    
    public int IntPropReadOnly { get; }
    
    [System.ComponentModel.Description("PublicTaskAsync")]
    [DisplayName("PublicTaskAsync")]
    public Task PublicTaskAsync()
    {
        return Task.CompletedTask;
    }
    
    [System.ComponentModel.Description("StaticPublicTaskAsync")]
    [DisplayName("StaticPublicTaskAsync")]
    public static Task StaticPublicTaskAsync()
    {
        return Task.CompletedTask;
    }
}
