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

public abstract class ClassSamplePartialProxy2 : IAsync
{
    /// <inheritdoc cref="ClassSamplePartial.IntPropInternal" />
    /// <remarks></remarks>
    internal virtual int IntPropInternal { get => _proxy.IntPropInternal; set => _proxy.IntPropInternal = value; }

    /// <inheritdoc cref="ClassSamplePartial.InternalTaskAsync()" />
    /// <remarks></remarks>
    internal virtual Task InternalTaskAsync() 
        => _proxy.InternalTaskAsync();

    /// <inheritdoc cref="ClassSamplePartial.IntProp" />
    /// <remarks></remarks>
    public virtual int IntProp { get => _proxy.IntProp; set => _proxy.IntProp = value; }
    /// <inheritdoc cref="ClassSamplePartial.IntPropReadOnly" />
    /// <remarks></remarks>
    public virtual int IntPropReadOnly => _proxy.IntPropReadOnly;

    /// <inheritdoc cref="ClassSamplePartial.PublicTaskAsync()" />
    /// <remarks></remarks>
    public virtual Task PublicTaskAsync() 
        => _proxy.PublicTaskAsync();

    protected readonly ClassSamplePartial _proxy;
    public ClassSamplePartialProxy2(ClassSamplePartial proxy)
    {
        _proxy = proxy;
    }
}