namespace HsuSgSyncTests.Samples;

public class IgnoreSample
{
    /// <inheritdoc cref="ClassSampleSyncOnly.StaticTaskAsync(int,string,CancellationToken)"/>
    public static Task StaticTaskAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}

// #if !NETFRAMEWORK && !NETSTANDARD && !(NETCOREAPP && !(NETCOREAPP3_0 || NETCOREAPP3_1))
// public partial interface InterfaceSample
// {
//     /// <inheritdoc cref="AwaitAsync" />
//     void AwaitThan(int index, string name, Cts token) 
//         => SyncHelper.Run(AwaitAsync(index, name, token));
// }
// #endif
