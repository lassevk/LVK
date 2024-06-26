using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace LVK.Core;

public class Assume
{
    [Conditional("DEBUG")]
    [ContractAnnotation("value:false => halt")]
    public static void That([DoesNotReturnIf(false)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (!value)
            throw new InvalidOperationException($"Assumption failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }
}