namespace Ultimately.Reasons;

using System;

/// <summary>
/// Represents an error that is associated with a specific exception instance.
/// </summary>
public sealed class ExceptionalError : Error
{
    /// <summary>
    /// Gets the exception associated with this error.
    /// </summary>
    public Exception Exception { get; }

    private ExceptionalError(string message, Exception exception) : base(message)
    {
        Exception = exception;
    }

    /// <summary>
    /// Creates a new <see cref="ExceptionalError"/> instance that wraps the specified exception.
    /// </summary>
    /// <param name="exception">The exception to wrap.</param>
    public static ExceptionalError Create(Exception exception)
    {
        return new ExceptionalError(exception.Message, exception);
    }

    /// <inheritdoc/>
    protected internal override ReasonStringBuilder GetReasonStringBuilder()
    {
        return new ReasonStringBuilder().WithInfoNoQuotes("", $"{Exception.GetType().Name}: '{Message}'");
    }
}
