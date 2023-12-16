// ReSharper disable MemberCanBePrivate.Global
namespace Hsu.Sg.FluentMember;

internal static class Constants
{
    // Configuration
    public const string FluentMemberPrefix = "build_property.GlobalFluentMemberPrefix";
    
    // Action
    public const string SystemNamespace = "System";

    public const string ActionType = "Action";
    public const string GenericActionType = "Action`1";
    public const string FuncType = "Func";
    public const string GenericFuncType = "Func`1";
    
    // Names
    public const string GenSuffix = "fm";
    public const string Namespace = "Hsu.Sg.FluentMember";
    public const string AttributeName = "FluentMemberAttribute";
    public const string FullAttributeName = $"{Namespace}.{AttributeName}";
    public const string GenName = "FluentMemberGen";
    public const string GenAttributeName = "FluentMemberGenAttribute";
    public const string FullGenAttributeName = $"{Namespace}.{GenAttributeName}";
    
    public static readonly DiagnosticDescriptor PartialDiagnosticDescriptor = new(
        "FM100",
        "Type should be within partial modifier",
        "Type should be within partial modifier",
        "Design",
        DiagnosticSeverity.Error,
        true);
}