// ReSharper disable MemberCanBePrivate.Global

using Hsu.Sg.Sync;

namespace HsuSgProxyTests.Samples;

public partial class ClassSamplePartial
{
    internal int IntPropInternal { get; set; }
    
    [SyncGen]
    [Ignore]
    internal Task InternalTaskAsync()
    {
        return Task.CompletedTask;
    }
}
