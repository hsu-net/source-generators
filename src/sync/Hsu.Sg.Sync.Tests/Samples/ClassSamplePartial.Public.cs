// ReSharper disable MemberCanBePrivate.Global

using System.ComponentModel;

namespace HsuSgSyncTests.Samples;

public partial class ClassSamplePartial
{
    [System.ComponentModel.Description("PublicTaskAsync")]
    [DisplayName("PublicTaskAsync")]
    public Task PublicTaskAsync()
    {
        return Task.CompletedTask;
    }
    
    [System.ComponentModel.Description("PublicTaskSuffix")]
    [DisplayName("PublicTaskSuffix")]
    public Task PublicTaskSuffix()
    {
        return Task.CompletedTask;
    }
}
