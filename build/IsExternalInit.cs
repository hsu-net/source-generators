using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

#if NETFRAMEWORK || NETSTANDARD || NETCOREAPP || NETCOREAPP2_1 || NETCOREAPP3_1

/// <summary>
///     Reserved to be used by the compiler for tracking metadata.
///     This class should not be used by developers in source code.
/// </summary>
[ExcludeFromCodeCoverage, DebuggerNonUserCode]
internal static class IsExternalInit;

#endif
