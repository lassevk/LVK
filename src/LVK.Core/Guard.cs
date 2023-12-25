using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LVK;

/// <summary>
/// This class provides guard class methods.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Guard against <c>null</c> value.
    /// </summary>
    /// <param name="value">
    /// The value that should not be <c>null</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <c>null</c>.
    /// </exception>
    public static void NotNull(object value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value is null)
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against <c>null</c> or empty string value.
    /// </summary>
    /// <param name="value">
    /// The value that should not be <c>null</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <c>null</c> or empty.
    /// </exception>
    public static void NotNullOrEmpty(string value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or empty string, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against <c>null</c> or empty string value.
    /// </summary>
    /// <param name="value">
    /// The value that should not be <c>null</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <c>null</c> or empty.
    /// </exception>
    public static void NotNullOrEmpty(ReadOnlySpan<char> value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value.Length == 0)
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or empty string, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against <c>null</c> or a string value only consisting of whitespace.
    /// </summary>
    /// <param name="value">
    /// The value that should not be <c>null</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <c>null</c> or only consists of whitespace.
    /// </exception>
    public static void NotNullOrWhiteSpace(string value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or consisting only of whitespace, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against <c>null</c> or a string value only consisting of whitespace.
    /// </summary>
    /// <param name="value">
    /// The value that should not be <c>null</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <c>null</c> or only consists of whitespace.
    /// </exception>
    public static void NotNullOrWhiteSpace(ReadOnlySpan<char> value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value.IsWhiteSpace())
            throw new ArgumentNullException(valueExpression, $"{valueExpression} cannot be null or consisting only of whitespace, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against out of range values by ensuring a value inside a minimum and maximum value range.
    /// </summary>
    /// <param name="value">
    /// The value that should be in range.
    /// </param>
    /// <param name="lowestValue">
    /// The lowest legal value of <paramref name="value"/>.
    /// </param>
    /// <param name="highestValue">
    /// The highest legal value of <paramref name="value"/>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is lower than <paramref name="lowestValue"/>, or greater than <paramref name="highestValue"/>.
    /// </exception>
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

    /// <summary>
    /// Guard against out of range values by ensuring a value that is greater than a lower value.
    /// </summary>
    /// <param name="value">
    /// The value that should be in range.
    /// </param>
    /// <param name="lowerValue">
    /// A value that should be considered lower than <paramref name="value"/>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is lower than or equal to <paramref name="lowerValue"/>.
    /// </exception>
    public static void GreaterThan<T>(T value, T lowerValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(lowerValue) <= 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than or equal to {lowerValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against out of range values by ensuring a value that is greater or equal to a minimum value.
    /// </summary>
    /// <param name="value">
    /// The value that should be in range.
    /// </param>
    /// <param name="lowestValue">
    /// A value that should be considered the lowest legal value for <paramref name="value"/>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is lower than <paramref name="lowestValue"/>.
    /// </exception>
    public static void GreaterThanOrEqualTo<T>(T value, T lowestValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(lowestValue) < 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than {lowestValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against out of range values by ensuring a value that is less than a lower value.
    /// </summary>
    /// <param name="value">
    /// The value that should be in range.
    /// </param>
    /// <param name="higherValue">
    /// A value that should be considered higher than <paramref name="value"/>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is higher than or equal to <paramref name="higherValue"/>.
    /// </exception>
    public static void LessThan<T>(T value, T higherValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(higherValue) >= 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than or equal to {higherValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against out of range values by ensuring a value that is less than or equal to a maximum value.
    /// </summary>
    /// <param name="value">
    /// The value that should be in range.
    /// </param>
    /// <param name="highestValue">
    /// A value that should be considered the highest legal value for <paramref name="value"/>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is higher than <paramref name="highestValue"/>.
    /// </exception>
    public static void LessThanOrEqualTo<T>(T value, T highestValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(highestValue) > 0)
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} should not be lower than {highestValue}, but was {value}, in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }


    /// <summary>
    /// Guard against same instance being passed into two different parameters.
    /// </summary>
    /// <param name="instance1">
    /// The first instance, to be compared against <paramref name="instance2"/>.
    /// </param>
    /// <param name="instance2">
    /// The second instance, to be compared against <paramref name="instance1"/>.
    /// </param>
    /// <param name="instance1Expression">
    /// Compiler will provide the expression used for <paramref name="instance1"/>.
    /// </param>
    /// <param name="instance2Expression">
    /// Compiler will provide the expression used for <paramref name="instance2"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <typeparam name="T">
    /// The type of references being compared.
    /// </typeparam>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="instance1"/> refers to the same instance as <paramref name="instance2"/>.
    /// </exception>
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

    /// <summary>
    /// Guard against arbitrary issues by providing an expression that should not be <c>true</c>.
    /// </summary>
    /// <param name="value">
    /// The expression/value that should not be <c>true</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/> is <c>true</c>.
    /// </exception>
    public static void Against([DoesNotReturnIf(true)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (value)
            throw new InvalidOperationException($"Guard failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against arbitrary issues by providing an expression that should be <c>true</c>. This method
    /// WILL be compiled into RELEASE builds. If you do not want RELEASE build checks, use <see cref="Assume"/> instead.
    /// </summary>
    /// <param name="value">
    /// The expression/value that should be <c>true</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/> is <c>false</c>.
    /// </exception>
    public static void Assert([DoesNotReturnIf(false)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (!value)
            throw new InvalidOperationException($"Guard failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// Guard against arbitrary issues by providing an expression that should be <c>true</c>. This method
    /// will not be compiled into RELEASE builds. If you want RELEASE build checks, use <see cref="Assert"/> instead.
    /// </summary>
    /// <remarks>
    /// The purpose of this method is more to guide the IDE and code analyzers into detecting
    /// unreachable code or always-true/always-false situations, when said IDE or analyzer is not able to
    /// by themselves, and the programmes "knows better".
    /// </remarks>
    /// <param name="value">
    /// The expression/value that should be <c>true</c>.
    /// </param>
    /// <param name="valueExpression">
    /// Compiler will provide the expression used for <paramref name="value"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler will provide the name of the member that contains the call to this method.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler will provide the full path to and name of the source code file that contains the call to this method.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler will provide the line number of the line in the source code file (<paramref name="callerFilePath"/>) that
    /// contains the call to this method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/> is <c>false</c>.
    /// </exception>
    [Conditional("DEBUG")]
    public static void Assume([DoesNotReturnIf(false)] bool value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int? callerLineNumber = null)
    {
        if (!value)
            throw new InvalidOperationException($"Guard failed for '{valueExpression}' in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }
}