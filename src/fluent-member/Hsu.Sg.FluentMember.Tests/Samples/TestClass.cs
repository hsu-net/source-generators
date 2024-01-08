using Hsu.Sg.FluentMember;

namespace HsuSgFluentMemberTests.Samples;

[FluentMember]
public partial class TestPropertyClass
{
    public static int StaticId { get; protected set; }
    public int Id { get; init; }
    public string Name { get; protected set; }
    public byte Age { get; private set; }
    public string Address => string.Empty;
    [FluentMemberGen(Prefix = "Sub")] 
    public Action<object> OnResult { get; set; }
    
    static TestPropertyClass()
    {
        new TestPropertyClass().SubOnResult(e => { });
    }
}

[FluentMember(Internal = true,Private = true,Prefix = "With")]
public partial class TestFieldClass
{
    public const int ConstId=0;
    public static int StaticId;
    public readonly int Id;
    [FluentMemberGen(Prefix = "Add",Modifier = Accessibility.Protected)] 
    internal string Name;
    private byte _age;
    private string address;
    [FluentMemberGen(Prefix = "Add",Modifier = Accessibility.Internal)] 
    protected event Action<object> onResult;

    static TestFieldClass()
    {
        new TestFieldClass().AddName(string.Empty);
        new TestFieldClass().AddOnResult(e => { });
    }
}

[FluentMember(Private = true)]
public partial class TestEventClass
{
    /// <inheritdoc cref="OnEvent" />
    /// <remarks></remarks>
    public event EventHandler OnEvent;
    [FluentMemberGen(Identifier = "UseOnEnterWithArgs")] 
    public event EventHandler<EventArgs> OnEnterWithArgs;
    [FluentMemberGen(Prefix = "With")] 
    protected event EventHandler OnEventProtected;

    [FluentMemberGen(Modifier = Accessibility.Public)] 
    private event EventHandler onEventPrivate;

    static TestEventClass()
    {
        new TestEventClass().WithOnEventProtected((s, e) => { });
        new TestEventClass().UseOnEnterWithArgs((s, e) => { });
        new TestEventClass().WithOnEventPrivate((s, e) => { });
    }
}

[FluentMember(Private = true)]
public partial class GenericClass<T> where T:struct
{
    [FluentMemberGen(Modifier = Accessibility.Public)] 
    private event EventHandler onEventPrivate;
}
