# Hsu.Sg.FluentMember

Generate a fluent method for fields and properties from a `struct` or `class`.

## Package Version

| Name | Source | Stable | Preview |
|---|---|---|---|
| Hsu.Sg.FluentMember| Nuget | [![NuGet](https://img.shields.io/nuget/v/Hsu.Sg.FluentMember?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.FluentMember) | [![NuGet](https://img.shields.io/nuget/vpre/Hsu.Sg.FluentMember?style=flat-square)](https://www.nuget.org/packages/Hsu.Sg.FluentMember) |
| Hsu.Sg.FluentMember| MyGet | [![MyGet](https://img.shields.io/myget/godsharp/v/Hsu.Sg.FluentMember?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.FluentMember) | [![MyGet](https://img.shields.io/myget/godsharp/vpre/Hsu.Sg.FluentMember?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.Sg.FluentMember) |

## Usages

### Install

- Hsu.Sg.FluentMember

You can install the package from nuget.

```csharp
<PackageReference Include="Hsu.Sg.FluentMember" Version="2023.412.12">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

### Attributes

Add the `FluentMember` attribute and `partial` keyword to the `class` or `struct`.

```csharp
[FluentMember]
public partial class ClassSamplePartial
{
}
```

If you want to specify fluent members to generate, you just need to set `[FluentMember(Only = true)]` and add `[FluentMemberGen]` attribute to the members.

```csharp
[FluentMember(Only = true)]
public partial class ClassSampleSyncOnly
{
    [FluentMemberGen(Ignore = true)]
    public DayOfWeek Week { get; set; }

    [FluentMemberGen(Ignore = true)] 
    private DayOfWeek PrivateWeek;

    [FluentMemberGen(Ignore = true)] 
    public event Action EventAction;
}
```

If you want to ignore generate fluent members, you just need to add `[FluentMemberGen(Ignore=true)]` attribute to the members.

```csharp
[FluentMember]
public partial class ClassSampleSyncOnly
{
    [FluentMemberGen(Ignore = true)] 
    public event Action EventAction;
}
```

If you want to specify an identifier with generated a proxy member, you just need to add `[FluentMemberGen(Identifier = "UseOnEnterWithArgs")]` attribute to that member.

```csharp
[FluentMember(Private = true)]
public partial class TestEventClass
{
    [FluentMemberGen(Identifier = "UseOnEnterWithArgs")] 
    public event EventHandler<EventArgs> OnEnterWithArgs;
}
```

If you want to specify an prefix with generated a fluent object, you just need to add `[FluentMember(Prefix = "With")]` attribute to that object or add `[FluentMemberGen(Prefix = "Add")]` attribute to that member.

```csharp
[FluentMember(Internal = true,Private = true,Prefix = "With")]
public partial class TestFieldClass
{
    [FluentMemberGen(Prefix = "Add",Modifier = Accessibility.Internal)] 
    protected event Action<object> onResult;
}
```

If you want to specify an accessible with generated a fluent object, you just need to add `[FluentMemberGen(Modifier = Accessibility.Internal)]` attribute to that member.

```csharp
[FluentMember]
public partial class TestFieldClass
{
    [FluentMemberGen(Modifier = Accessibility.Internal)]
    private event Action<object> onResult;
}
```

## References

- [Nito.AsyncEx](https://github.com/StephenCleary/AsyncEx)

## License

[MIT](../../../LICENSE)