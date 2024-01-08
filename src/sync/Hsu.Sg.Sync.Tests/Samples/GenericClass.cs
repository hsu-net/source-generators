using Hsu.Sg.Sync;

namespace HsuSgSyncTests.Samples;

[Sync]
public partial class GenericClass<T> where T:struct
{
    public ValueTask StaticValueTaskAsync()
    {
        return new ValueTask();
    }
}
