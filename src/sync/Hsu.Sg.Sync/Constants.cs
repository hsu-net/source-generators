// ReSharper disable MemberCanBePrivate.Global
namespace Hsu.Sg.Sync;

internal static class Constants
{
    // Task
    public const string TaskNamespace = "System.Threading.Tasks";

    public const string TaskType = "Task";
    public const string ValueTaskType = "ValueTask";
    public const string GenericTaskType = "Task`1";
    public const string GenericValueTaskType = "ValueTask`1";

    // Names
    public const string GenSuffix = "sync";
    public const string Namespace = "Hsu.Sg.Sync";
    public const string AttributeName = "SyncAttribute";
    public const string FullAttributeName = $"{Namespace}.{AttributeName}";
    public const string GenName = "SyncGen";
    public const string GenAttributeName = "SyncGenAttribute";
    public const string FullGenAttributeName = $"{Namespace}.{GenAttributeName}";

    public const string InterfaceDefaultImplTrue = "!NETFRAMEWORK && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0_OR_GREATER)";
    public const string InterfaceDefaultImplFalse = "NETFRAMEWORK || !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0_OR_GREATER)";

    public static readonly DiagnosticDescriptor PartialDiagnosticDescriptor = new(
        "HS100",
        "Type should be within partial modifier",
        "Type should be within partial modifier",
        "Design",
        DiagnosticSeverity.Error,
        true);
}