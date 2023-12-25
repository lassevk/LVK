using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable InconsistentNaming

namespace LVK;

/// <summary>
/// This class provides assert and assume methods to help the IDE provide more meaningful
/// analysis results when it is not able to assert certain conditions.
/// </summary>
[Obsolete("Use Guard.Ensure and Guard.Assume instead")]
public static class Assertions
{
    /// <summary>
    /// This method will throw an <see cref="AssertionFailedException"/> if the given
    /// <paramref name="expression"/> is <c>false</c>.
    /// </summary>
    /// <param name="expression">
    /// The expression to assert is <c>true</c>.
    /// </param>
    /// <param name="callerArgumentExpression">
    /// Compiler provides the full expression used for <paramref name="expression"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler provides the name of the member the call to assert is in.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler provides the full path to and name of the file the call to assert is in.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler provides the line number of the line of source code that contains the call to assert.
    /// </param>
    /// <exception cref="AssertionFailedException">
    /// <paramref name="expression"/> is <c>false</c>.
    /// </exception>
    [Obsolete("Use Guard.Ensure instead")]
    public static void assert(
        [DoesNotReturnIf(false)] bool expression, [CallerArgumentExpression(nameof(expression))] string? callerArgumentExpression = null, [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int? callerLineNumber = 0)
    {
        if (!expression)
            throw new AssertionFailedException($"Assertion '{expression} failed in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }

    /// <summary>
    /// This method will throw an <see cref="AssertionFailedException"/> if the given
    /// <paramref name="expression"/> is <c>false</c>.
    /// </summary>
    /// <remarks>
    /// Note that calls to this method will only be compiled into the assembly for
    /// DEBUG builds. If you need a similar construct for RELEASE builds, use
    /// <see cref="assert"/>.
    /// </remarks>
    /// <param name="expression">
    /// The expression to assume is <c>true</c>.
    /// </param>
    /// <param name="callerArgumentExpression">
    /// Compiler provides the full expression used for <paramref name="expression"/>.
    /// </param>
    /// <param name="callerMemberName">
    /// Compiler provides the name of the member the call to assume is in.
    /// </param>
    /// <param name="callerFilePath">
    /// Compiler provides the full path to and name of the file the call to assume is in.
    /// </param>
    /// <param name="callerLineNumber">
    /// Compiler provides the line number of the line of source code that contains the call to assume.
    /// </param>
    /// <exception cref="AssertionFailedException">
    /// <paramref name="expression"/> is <c>false</c>.
    /// </exception>
    [Conditional("DEBUG")]
    [Obsolete("Use Guard.Assume instead")]
    public static void assume(
        [DoesNotReturnIf(false)] bool expression, [CallerArgumentExpression(nameof(expression))] string? callerArgumentExpression = null, [CallerMemberName] string? callerMemberName = null,
        [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int? callerLineNumber = 0)
    {
        if (!expression)
            throw new AssertionFailedException($"Assertion '{expression} failed in {callerMemberName} at {callerFilePath}#{callerLineNumber}");
    }
}