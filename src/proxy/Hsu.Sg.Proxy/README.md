# Hsu.Sg.Proxy

Generate a proxy object from a `struct` or `class` or `interface`.

## Package Version

| Name | Source | Stable | Preview |
|---|---|---|---|
| Hsu.Sg.Proxy | Nuget | [![NuGet](https://img.shields.io/nuget/v/Hsu.Sg.Proxy?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.Proxy) | [![NuGet](https://img.shields.io/nuget/vpre/Hsu.Sg.Proxy?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.Proxy) |
| Hsu.Sg.Proxy | MyGet | [![MyGet](https://img.shields.io/myget/godsharp/v/Hsu.Sg.Proxy?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.Proxy) | [![MyGet](https://img.shields.io/myget/godsharp/vpre/Hsu.Sg.Proxy?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.Proxy) |

## Usages

### Install

- Hsu.Sg.Proxy

You can install the package from nuget.

```csharp
<PackageReference Include="Hsu.Sg.Proxy" Version="2023.412.12">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

### Attributes

Add the `Proxy` attribute and `partial` keyword to the `class` or `struct` or `interface`.

```csharp
[Proxy]
public partial class ClassSamplePartial
{
}
```

If you want to specify proxy members to generate, you just need to set `[Proxy(Only = true)]` and add `[ProxyGen]` attribute to the members.

```csharp
[Proxy(Only = true,Static = true,Abstract = true,Suffix = "Builder")]
public partial class ClassSampleSyncOnly
{
    [ProxyGen(Ignore = false)]
    public DayOfWeek Week { get; set; }

    [ProxyGen(Ignore = false)] private DayOfWeek PrivateWeek;

    [ProxyGen(Ignore = false)] public event Action EventAction;
    [ProxyGen(Ignore = false)] public event EventHandler Event;
   
    [ProxyGen(Ignore = false)]
    public static Task StaticTaskAsync()
    {
        return Task.CompletedTask;
    }
}
```

If you want to ignore generate proxy members, you just need to add `[ProxyGen(Ignore=true)]` attribute to the members.

```csharp
[Proxy]
public partial class ClassSampleSyncOnly
{
    
    [ProxyGen(Ignore = true)] 
    public event Action EventAction;
    
    [ProxyGen(Ignore = true)]
    public static Task StaticTaskIgnoreAsync()
    {
        return Task.CompletedTask;
    }
}
```

If you want to specify an identifier with generated a proxy object, you just need to add `[Proxy(Identifier = "InterfaceIdentity")]` attribute to that object.

```csharp
[Proxy(Sealed = true, Identifier = "InterfaceIdentity")]
public partial interface InterfaceSample
{
    Task AwaitAsync(int index, string name, Cts token);
}
```

If you want to specify an suffix with generated a proxy object, you just need to add `[Proxy(Suffix = "Builder")]` attribute to that object.

```csharp
[Proxy(Only = true,Static = true,Abstract = true,Suffix = "Builder")]
public partial class ClassSampleSyncOnly
{
}
```

If you want to specify `interface` with generated a proxy object, you just need to add `[Proxy(Interfaces = new []{nameof(IAsync)})]` attribute to that object.

```csharp
public interface IAsync {}

[Proxy(Interfaces = new []{nameof(IAsync)})]
public partial class ClassSamplePartial
{
}
```

## References

- [Nito.AsyncEx](https://github.com/StephenCleary/AsyncEx)

## License

[MIT](../../../LICENSE)