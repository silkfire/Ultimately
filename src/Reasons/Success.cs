namespace Ultimately.Reasons;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a successful outcome with associated reasons.
/// </summary>
public class Success : Reason
{
    /// <summary>
    /// Gets a default success instance with an empty message.
    /// </summary>
    public static Success Default => new("");

    /// <summary>
    /// Gets the list of reasons that led to this success.
    /// </summary>
    public List<Success> Reasons { get; }

    private Success()
    {
        Reasons = [];
    }

    private Success(string message) : this()
    {
        Message = message;
    }

    private Success(string message, Success enabledBy) : this(message)
    {
        Reasons.Add(enabledBy);
    }

    /// <summary>
    /// Creates a new <see cref="Success"/> instance with the specified message.
    /// </summary>
    /// <param name="message">The success message.</param>
    public static Success Create(string message)
    {
        return new Success(message);
    }

    /// <summary>
    /// Creates a new <see cref="Success"/> instance with the specified message and the <see cref="Success"/> instance that this instance is enabled by.
    /// </summary>
    /// <param name="message">The message that describes the success.</param>
    /// <param name="enabledBy">A <see cref="Success"/> instance that this success is based on.</param>
    public static Success Create(string message, Success enabledBy)
    {
        return new Success(message, enabledBy);
    }

    /// <summary>
    /// Adds the success reason that enabled this success.
    /// </summary>
    /// <param name="message">The message of the success that enabled this success.</param>
    public Success EnabledBy(string message)
    {
        Reasons.Add(new Success(message));

        return this;
    }

    /// <summary>
    /// Adds the <see cref="Success"/> instance that enabled this success.
    /// </summary>
    /// <param name="success">The <see cref="Success"/> instance that enabled this success.</param>
    public Success EnabledBy(Success success)
    {
        Reasons.Add(success);

        return this;
    }

    /// <summary>
    /// Adds the success reason from the specified optional value to the current object if the value is present.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the optional parameter.</typeparam>
    /// <param name="some">An optional value whose associated success reason will be added if present. Must have a value.</param>
    /// <returns>The current <see cref="Success"/> instance with the success reason added if the optional value is present.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="some"/> does not have a value.</exception>
    public Success EnabledBy<TValue>(Option<TValue> some)
    {
        if (!some.HasValue)
        {
            throw new InvalidOperationException("The optional value cannot empty in order to access its success object");
        }

        some.MatchSome((_, s) => Reasons.Add(s));

        return this;
    }

    /// <summary>
    /// Adds the value from a successful optional result to the current success reasons and returns this instance.
    /// </summary>
    /// <param name="some">The optional value to evaluate. Must represent a successful result.</param>
    /// <returns>This <see cref="Success"/> instance, with the value from <paramref name="some"/> added to its reasons if
    /// successful.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="some"/> does not represent a successful result.</exception>
    public Success EnabledBy(Option some)
    {
        if (!some.IsSuccessful)
        {
            throw new InvalidOperationException("The optional value cannot unsuccessful in order to access its success object");
        }

        some.MatchSome(s => Reasons.Add(s));

        return this;
    }

    /// <summary>
    /// Adds the specified metadata to the <see cref="Success"/> instance and returns the updated instance.
    /// </summary>
    /// <param name="name">The name of the metadata.</param>
    /// <param name="value">The value of the metadata.</param>
    public Success WithMetadata(string name, object value)
    {
        Metadata.Add(name, value);

        return this;
    }

    /// <summary>
    /// Adds the specified metadata entries to this <see cref="Success"/> instance and returns the updated instance.
    /// </summary>
    /// <param name="metadata">A dictionary containing metadata key-value pairs to add.</param>
    public Success WithMetadata(Dictionary<string, object> metadata)
    {
        foreach (var metadataItem in metadata)
        {
            Metadata.Add(metadataItem.Key, metadataItem.Value);
        }

        return this;
    }

    /// <inheritdoc/>
    protected internal override ReasonStringBuilder GetReasonStringBuilder()
    {
        return new ReasonStringBuilder()
               .WithInfo("", Message)
               .WithInfoNoQuotes(nameof(Metadata), string.Join("; ", Metadata.Select(kvp => $"'{kvp.Key}: {kvp.Value}'")))
               .WithInfoNoQuotes("Anteceded by", $"{string.Join(" ⁎ ", Reasons)}");
    }
}
