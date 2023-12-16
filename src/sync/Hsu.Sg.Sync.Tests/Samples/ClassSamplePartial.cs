using Hsu.Sg.Sync;
// ReSharper disable MemberCanBePrivate.Global

namespace HsuSgSyncTests.Samples;

[Sync]
public partial class ClassSamplePartial
{
    [Ignore]
    [System.ComponentModel.Description("PrivateTaskAsync")]
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