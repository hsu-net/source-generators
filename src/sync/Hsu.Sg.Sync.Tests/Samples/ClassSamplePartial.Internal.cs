// ReSharper disable MemberCanBePrivate.Global

using System.ComponentModel;

namespace HsuSgSyncTests.Samples;

public partial class ClassSamplePartial
{
    [System.ComponentModel.Description("InternalTaskAsync")]
    [DisplayName("InternalTaskAsync")]
    internal async Task InternalTaskAsync()
    {
        await Task.CompletedTask;
    }
}
