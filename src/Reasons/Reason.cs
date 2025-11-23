namespace Ultimately.Reasons;

using System.Collections.Generic;

// https://github.com/altmann/FluentResults
/// <summary>
/// Base class for representing reasons for success or failure.
/// </summary>
public abstract class Reason
{
    /// <summary>
    /// Gets the message associated with the current object.
    /// </summary>
    public string Message { get; protected set; }

    /// <summary>
    /// Gets the collection of key-value pairs that store additional metadata associated with the object.
    /// </summary>
    public Dictionary<string, object> Metadata { get; protected set; } = [];

    /// <summary>
    /// Gets a <see cref="ReasonStringBuilder"/> for this reason.
    /// </summary>
    protected internal virtual ReasonStringBuilder GetReasonStringBuilder()
    {
        return new ReasonStringBuilder().WithReasonType(GetType())
                                        .WithInfo("", Message)
                                        .WithInfo(nameof(Metadata), string.Join("; ", Metadata));
    }

    /// <summary>
    /// Returns a string representation of the reason.
    /// </summary>
    public override string ToString()
    {
        return GetReasonStringBuilder().Build();
    }
}
