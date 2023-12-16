using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Hsu.Sg.Proxy;
using Hsu.Sg.Sync;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HsuSgProxyTests.Samples;

[Proxy(Only = true,Static = true,Abstract = true,Suffix = "Builder")]
[Sync(Attribute = true,AttributeExcludes = new []{"DisplayName","Description"})]
public partial class ClassSampleSyncOnly
{
    [ProxyGen(Ignore = false)]
    public DayOfWeek Week { get; set; }

    [ProxyGen(Ignore = false)] private DayOfWeek PrivateWeek;

    [ProxyGen(Ignore = false)] public event Action EventAction;
    [ProxyGen(Ignore = false)] public event EventHandler Event;
    
    public static void StaticWithout()
    {
        Console.WriteLine("Static without returns");
    }

    public void ClassWithout()
    {
        Console.WriteLine("Class without returns");
    }

    [ProxyGen(Ignore = true)]
    [DisplayName("StaticTaskIgnoreAsync")]
    [System.ComponentModel.Description("StaticTaskIgnoreAsync")]
    public static Task StaticTaskIgnoreAsync()
    {
        return Task.CompletedTask;
    }

    [ProxyGen(Ignore = false)]
    [SyncGen(Ignore = false)]
    [DisplayName("StaticTaskIgnoreAsync")]
    [System.ComponentModel.Description("StaticTaskIgnoreAsync")]
    public static Task StaticTaskAsync()
    {
        return Task.CompletedTask;
    }

    [ProxyGen(Ignore = false)]
    [DisplayName("StaticTaskAsync")]
    [System.ComponentModel.Description("StaticTaskAsync")]
    public static Task StaticTaskAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    public partial struct MyStruct
    {
        [ProxyGen]
        public Task AwaitAsync(int index, string name, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }

    [ProxyGen(Ignore = true)]
    public Task AwaitAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}