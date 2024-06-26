using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace LVK.Core;

public static class Guard
{
    [ContractAnnotation("value:null => halt")]
    public static void NotNull(object value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value is null)
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    [ContractAnnotation("value:null => halt")]
    public static void NotNullOrEmpty(string value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or empty string, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    [ContractAnnotation("value:null => halt")]
    public static void NotNullOrEmpty(ReadOnlySpan<char> value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value.Length == 0)
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or empty string, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    [ContractAnnotation("value:null => halt")]
    public static void NotNullOrWhiteSpace(string value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or consisting only of whitespace, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    [ContractAnnotation("value:null => halt")]
    public static void NotNullOrWhiteSpace(ReadOnlySpan<char> value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value.IsWhiteSpace())
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or consisting only of whitespace, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    public static void InRange<T>(T value, T lowestValue, T highestValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(lowestValue) < 0 || value.CompareTo(highestValue) > 0)
        {
            throw new ArgumentOutOfRangeException(valueExpression,
                $"{valueExpression} must be in the range {lowestValue}..{highestValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
        }
    }

    public static void GreaterThan<T>(T value, T lowerValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(lowerValue) <= 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than or equal to {lowerValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    public static void GreaterThanOrEqualTo<T>(T value, T lowestValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(lowestValue) < 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than {lowestValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    public static void LessThan<T>(T value, T higherValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(higherValue) >= 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than or equal to {higherValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    public static void LessThanOrEqualTo<T>(T value, T highestValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(highestValue) > 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than {highestValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    public static void DifferentInstances<T>(
        T instance1, T instance2, [CallerArgumentExpression(nameof(instance1))] string? instance1Expression = null, [CallerArgumentExpression(nameof(instance2))] string? instance2Expression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : class
    {
        if (ReferenceEquals(instance1, instance2))
            throw new ArgumentException($"{instance1Expression} and {instance2Expression} should not refer to the same instance in {callerMemberName} at {callerFilePath}#{callerLineNumber}", instance1Expression);
    }

    public static void Against([DoesNotReturnIf(true)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value)
            throw new InvalidOperationException($"Guard failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    [ContractAnnotation("value:false => halt")]
    public static void Assert([DoesNotReturnIf(false)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (!value)
            throw new InvalidOperationException($"Guard failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }
}