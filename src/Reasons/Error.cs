namespace Ultimately.Reasons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents an error with an associated message, optional metadata, and a chain of underlying causes. Provides
/// methods for constructing error objects, attaching additional context, and formatting error information for display
/// or logging.
/// </summary>
public class Error : Reason
{
    /// <summary>
    /// Gets a default error instance with an empty message.
    /// </summary>
    public static Error Default => new("");

    /// <summary>
    /// Gets the list of errors that describe the reasons for the current failure or invalid state.
    /// </summary>
    public List<Error> Reasons { get; }

    private Error()
    {
        Reasons = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    protected Error(string message) : this()
    {
        Message = message;
    }

    private Error(string message, Error causedBy) : this(message)
    {
        Reasons.Add(causedBy);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public static Error Create(string message)
    {
        return new Error(message);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified message and the <see cref="Error"/> instance that caused this error.
    /// </summary>
    /// <param name="message">The error message that describes the error.</param>
    /// <param name="causedBy">The underlying <see cref="Error"/> that caused this error.</param>
    public static Error Create(string message, Error causedBy)
    {
        return new Error(message, causedBy);
    }

    /// <summary>
    /// Adds the error reason that led to the current error and returns the updated error instance.
    /// </summary>
    /// <param name="message">The message of the error that led to this error.</param>
    public Error CausedBy(string message)
    {
        Reasons.Add(new Error(message));

        return this;
    }

    /// <summary>
    /// Adds the <see cref="Error"/> instance that led to the current error and returns the updated error instance.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> instance that led to this error.</param>
    public Error CausedBy(Error error)
    {
        Reasons.Add(error);

        return this;
    }

    /// <summary>
    /// Adds the error reason from the specified empty optional value that led to the current error and returns the updated error instance.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the optional parameter.</typeparam>
    /// <param name="none">An optional value whose associated error reason will be added if present. Must be empty.</param>
    /// <returns>The current <see cref="Error"/> instance with the error reason added if the optional value is empty.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="none"/> does not have a value.</exception>
    public Error CausedBy<TValue>(Option<TValue> none)
    {
        if (none.HasValue)
        {
            throw new InvalidOperationException("The optional value must be empty in order to access its error object");
        }

        none.MatchNone(e => Reasons.Add(e));

        return this;
    }

    /// <summary>
    /// Adds the error reason from an unsuccessful <see cref="Option"/> to the current <see cref="Error"/> instance.
    /// </summary>
    /// <param name="none">The <see cref="Option"/> instance whose error reason will be added. Must represent an unsuccessful outcome.</param>
    /// <returns>The current <see cref="Error"/> instance with the additional error reason included.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="none"/> represents a successful outcome.</exception>
    public Error CausedBy(Option none)
    {
        if (none.IsSuccessful)
        {
            throw new InvalidOperationException("The optional's outcome must be unsuccessful in order to access its error object");
        }

        none.MatchNone(e => Reasons.Add(e));

        return this;
    }

    /// <summary>
    /// Adds the exception that led to the current error and returns the updated error instance.
    /// </summary>
    /// <param name="exception">The exception that caused or contributed to this error.</param>
    public Error CausedBy(Exception exception)
    {
        Reasons.Add(ExceptionalError.Create(exception));

        return this;
    }

    /// <summary>
    /// Adds the specified metadata to the <see cref="Error"/> instance and returns the updated instance.
    /// </summary>
    /// <param name="name">The name of the metadata entry to add.</param>
    /// <param name="value">The value to associate with the specified metadata name.</param>
    public Error WithMetadata(string name, object value)
    {
        Metadata.Add(name, value);

        return this;
    }

    /// <summary>
    /// Adds the specified metadata entries to this <see cref="Error"/> instance and returns the updated instance.
    /// </summary>
    /// <param name="metadata">A dictionary containing metadata key-value pairs to add.</param>
    public Error WithMetadata(Dictionary<string, object> metadata)
    {
        foreach (var metadataItem in metadata)
        {
            Metadata.Add(metadataItem.Key, metadataItem.Value);
        }

        return this;
    }

    /// <summary>
    /// Adds metadata about the calling method's source location to the current <see cref="Error"/> instance and returns the updated instance.
    /// </summary>
    /// <remarks>This method is intended to be called without specifying arguments, so that the compiler automatically provides the caller's file path, method name, and line number using caller information attributes. This metadata can be useful for debugging or logging purposes.</remarks>
    /// <param name="callingClassFilepath">The full file path of the source code file containing the calling method. This value is automatically supplied by the compiler and should not be set explicitly.</param>
    /// <param name="callingMethod">The name of the calling method. This value is automatically supplied by the compiler and should not be set explicitly.</param>
    /// <param name="callingClassLineNumber">The line number in the source file at which the method is called. This value is automatically supplied by the compiler and should not be set explicitly.</param>
    public Error WithCallerMethodMetadata([CallerFilePath] string callingClassFilepath = "", [CallerMemberName] string callingMethod = "", [CallerLineNumber] int callingClassLineNumber = 0)
    {
        Metadata.Add(callingClassFilepath, $"{callingMethod}:{callingClassLineNumber}");

        return this;
    }

    /// <summary>
    /// Implicitly converts a string message to an <see cref="Error"/> instance.
    /// </summary>
    /// <param name="message">The error message.</param>
    public static implicit operator Error(string message)
    {
        return Create(message);
    }

    /// <summary>
    /// Formats the error object as a one-line string, using the → symbol as a separator and an infinite depth.
    /// </summary>
    public string Print() => Print(0);

    /// <summary>
    /// Formats the error object as a one-line string, using the specified symbol as a separator and an infinite depth.
    /// </summary>
    /// <param name="separator">A string to delimit the indivudual error messages with.</param>
    public string Print(string separator)
    {
        var errorMessageChain = GetErrorMessageChain(this);

        return string.Join(separator, errorMessageChain);
    }

    /// <summary>
    /// Formats the error object as a one-line string, using the → symbol as a separator and using the specified depth value.
    /// </summary>
    /// <param name="depth">The number of levels to traverse in the error chain. Zero means infinite depth.</param>
    public string Print(byte depth) => Print(null as Func<string, string>, depth);

    /// <summary>
    /// Formats the error object as a one-line string using the specified transformation function on each individual message.
    /// </summary>
    /// <param name="transformFunc">A function to transform each message according to.</param>
    /// <param name="depth">The number of levels to traverse in the error chain. Zero means infinite depth.</param>
    /// <param name="separator">A string to delimit the indivudual error messages with.</param>
    public string Print(Func<string, string> transformFunc, byte depth = 0, string separator = " → ")
    {
        transformFunc ??= s => s;

        var errorMessageChain = GetErrorMessageChain(this, depth).Select(transformFunc);

        return string.Join(separator, errorMessageChain);
    }

    /// <summary>
    /// Formats the error object as a one-line string using the specified transformation function on each individual message.
    /// </summary>
    /// <param name="transformFunc">A function to transform each message according to. <para>The second argument specifies the zero-based index of the message in the error chain and the third argument is <see langword="true"/> when the message is the last one in the chain.</para></param>
    /// <param name="depth">The number of levels to traverse in the error chain. Zero means infinite depth.</param>
    /// <param name="separator">A string to delimit the indivudual error messages with.</param>
    public string Print(Func<string, int, bool, string> transformFunc, byte depth = 0, string separator = " → ")
    {
        transformFunc ??= (m, _, _) => m;

        var errorMessageChain = GetErrorMessageChain(this, depth).ToList();

        return string.Join(separator, errorMessageChain.Select((m, i) => transformFunc(m, i, i == errorMessageChain.Count - 1)));
    }

    private static IEnumerable<string> GetErrorMessageChain(Error error, byte depth = 0)
    {
        var currentDepth = 0;

        while (true)
        {
            if (error == null || depth > 0 && currentDepth == depth)
            {
                yield break;
            }

            yield return error.Message;

            error = error.Reasons.FirstOrDefault();

            currentDepth++;
        }
    }

    /// <inheritdoc/>
    protected internal override ReasonStringBuilder GetReasonStringBuilder()
    {
        return new ReasonStringBuilder().WithInfo("", Message)
                                        .WithInfoNoQuotes(nameof(Metadata), string.Join("; ", Metadata.Select(kvp => $"{kvp.Key}: {kvp.Value}")))
                                        .WithInfoNoQuotes("Caused by", $"{string.Join(" ⁎ ", Reasons)}");
    }
}
