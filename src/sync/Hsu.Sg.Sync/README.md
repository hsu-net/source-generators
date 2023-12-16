# Hsu.Sg.Sync

[![Nito.AsyncEx](https://img.shields.io/badge/Nito-AsyncEx-yellow.svg)](https://github.com/StephenCleary/AsyncEx)

Generate a synchronous method from an asynchronous method.

## Package Version

| Name | Source | Stable | Preview |
|---|---|---|---|
| Hsu.Sg.Sync | Nuget | [![NuGet](https://img.shields.io/nuget/v/Hsu.Sg.Sync?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.Sync) | [![NuGet](https://img.shields.io/nuget/vpre/Hsu.Sg.Sync?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.Sync) |
| Hsu.Sg.Sync | MyGet | [![MyGet](https://img.shields.io/myget/godsharp/v/Hsu.Sg.Sync?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.Sync) | [![MyGet](https://img.shields.io/myget/godsharp/vpre/Hsu.Sg.Sync?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.Sync) |

## Usages

### Install

- Hsu.Sg.Sync
- Nito.AsyncEx.Context
- System.Threading.Tasks.Extensions

You can install the packages from nuget.

```csharp
<PackageReference Include="Hsu.Sg.Sync" Version="2023.412.14.1">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

>If you want to use `ValueTask` in .net framework, you should install the package `System.Threading.Tasks.Extensions`.*

### Attributes

Add the `Sync` attribute and `partial` keyword to the object that has asynchronous method.

```csharp
[Sync]
public partial class ClassSamplePartial
{
    public Task PartialTaskAsync()
    {
        return Task.CompletedTask;
    }
}
```

If you want to specify an asynchronous method to generate a synchronous method, you just need to set `[Sync(Only = true)]` and add `[SyncGen]` attribute to the method.

```csharp
[Sync(Only = true)]
public partial class ClassSampleSyncOnly
{    
    [SyncGen]
    public static ValueTask<Dictionary<string, object>> StaticValueTaskDictionaryAsync()
    {
        return new ValueTask<Dictionary<string, object>>(new Dictionary<string, object>());
    }
}
```

If you want to ignore generate a synchronous method, you just need to add `[SyncGen(Ignore=true)]` attribute to the method.

```csharp
[Sync(Only = true)]
public partial class ClassSampleSyncOnly
{    
    [SyncGen(Ignore = true)]
    public Task AwaitAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}
```

If you want to specify an identifier with generated a synchronous method, you just need to add `[SyncGen(Identifier = "AwaitRename")]` attribute to the method.

```csharp
[Sync(Only = true)]
public partial class ClassSampleSyncOnly
{    
    [SyncGen(Identifier = "AwaitRename")]
    public Task AwaitAsync(int index, string name, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}
```

If you just want to generate a `abstract` or `interface` synchronous method, you need to add `[Sync(Definable =true)]` attribute to the `abstract class` or `interface`.

```csharp
[Sync(Definable =true)]
public abstract partial class DefinableClass
{
    public abstract Task AwaitAsync(int index, string name, CancellationToken token);
}

[Sync(Definable =true)]
public abstract partial record DefinableRecord
{
    public abstract Task AwaitAsync(int index, string name, CancellationToken token);
}
```

## References

- [Nito.AsyncEx](https://github.com/StephenCleary/AsyncEx)

## License

[MIT](../../../LICENSE)