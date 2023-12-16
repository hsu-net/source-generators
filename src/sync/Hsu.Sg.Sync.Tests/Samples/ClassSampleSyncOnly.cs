using System.ComponentModel;
using Hsu.Sg.Sync;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HsuSgSyncTests.Samples;

[Sync(Only = true,Suffix = "Sync2")]
public partial class ClassSampleSyncOnly
{
    public static void StaticWithout()
    {
        Console.WriteLine("Static without returns");
    }

    public void ClassWithout()
    {
        Console.WriteLine("Class without returns");
    }

#if NET8_0 || NET461

    [System.ComponentModel.Description("StaticValueTaskAsync")]
    [DisplayName("StaticValueTaskAsync")]
    [SyncGen(Identifier = "StaticValueTaskAsync1")]
    public static ValueTask StaticValueTaskAsync()
    {
        return new ValueTask();
    }

    [SyncGen(Ignore = true)]
    [DisplayName("StaticValueTaskIgnoreAsync")]
    [System.ComponentModel.Description("StaticValueTaskIgnoreAsync")]
    public static ValueTask StaticValueTaskIgnoreAsync()
    {
        return new ValueTask();
    }

    [SyncGen]
    public static ValueTask<bool> StaticValueTaskTrueAsync()
    {
        return new ValueTask<bool>(true);
    }

    [SyncGen]
    public static ValueTask<Dictionary<string, object>> StaticValueTaskDictionaryAsync()
    {
        return new ValueTask<Dictionary<string, object>>(new Dictionary<string, object>());
    }

#endif

    [SyncGen(Ignore = true)]
    [DisplayName("StaticTaskIgnoreAsync")]
    [System.ComponentModel.Description("StaticTaskIgnoreAsync")]
    public static Task StaticTaskIgnoreAsync()
    {
        return Task.CompletedTask;
    }

    [SyncGen(Ignore = false)]
    [DisplayName("StaticTaskAsync")]
    [System.ComponentModel.Description("StaticTaskAsync")]
    public static Task StaticTaskAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// StaticTaskAsync summary description
    /// </summary>
    /// <param name="index">index</param>
    /// <param name="name"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [SyncGen(Ignore = true)]
    public static Task StaticTaskAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    public partial struct MyStruct
    {
        [SyncGen(Identifier = "AwaitRename")]
        public Task AwaitAsync(int index, string name, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }

    [SyncGen(Ignore = true)]
    public Task AwaitAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    [SyncGen(Ignore = false)]
    [DisplayName("StaticTaskSuffixByType")]
    [System.ComponentModel.Description("StaticTaskSuffixByType")]
    public static Task StaticTaskSuffixByType()
    {
        return Task.CompletedTask;
    }

    [SyncGen(Ignore = false,Suffix = "Before")]
    [DisplayName("StaticTaskSuffixByMethod")]
    [System.ComponentModel.Description("StaticTaskSuffixByMethod")]
    public static Task StaticTaskSuffixByMethod()
    {
        return Task.CompletedTask;
    }
}