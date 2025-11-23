namespace Ultimately.Reasons;

using System;
using System.Collections.Generic;

/// <summary>
/// Helper class for building reason strings.
/// </summary>
public sealed class ReasonStringBuilder
{
    private  string _reasonType = string.Empty;
    private readonly List<string> _infoStrings = [];

    /// <summary>
    /// Sets the reason type based on the provided type's name.
    /// </summary>
    /// <param name="type">The type to set as the reason type.</param>
    public ReasonStringBuilder WithReasonType(Type type)
    {
        _reasonType = type.Name;

        return this;
    }

    /// <summary>
    /// Adds an informational label-value pair to the reason string.
    /// </summary>
    /// <param name="label">The label for the information.</param>
    /// <param name="value">The value for the information.</param>
    public ReasonStringBuilder WithInfo(string label, string value)
    {
        var infoString = ToLabelValueStringOrEmpty(label, value);

        if (!string.IsNullOrEmpty(infoString))
        {
            _infoStrings.Add(infoString);
        }

        return this;
    }

    /// <summary>
    /// Appends an informational label and value pair to the builder without surrounding the value in quotes.
    /// </summary>
    /// <param name="label">The label to associate with the value. Cannot be null.</param>
    /// <param name="value">The value to append. If null or empty, the pair is not added.</param>
    public ReasonStringBuilder WithInfoNoQuotes(string label, string value)
    {
        var infoString = ToLabelValueStringOrEmptyNoQuotes(label, value);

        if (!string.IsNullOrEmpty(infoString))
        {
            _infoStrings.Add(infoString);
        }

        return this;
    }

    /// <summary>
    /// Builds and returns a string representation of the current reason type and its associated information.
    /// </summary>
    /// <returns>A string that combines the reason type and its related information. If no information is present, only the
    /// reason type is returned.</returns>
    internal string Build()
    {
        var reasonInfoText = _infoStrings.Count != 0
            ? ReasonInfosToString(_infoStrings)
            : string.Empty;

        return $"{_reasonType}{(_reasonType != "" ? "=" : "")}{reasonInfoText}";
    }

    private static string ReasonInfosToString(IEnumerable<string> infoStrings)
    {
        return string.Join(", ", infoStrings);
    }

    private static string ToLabelValueStringOrEmpty(string label, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return $"{(label != "" ? $"{label}=" : "")}'{value}'";
    }

    private static string ToLabelValueStringOrEmptyNoQuotes(string label, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return $"{(label != "" ? $"{label}=" : "")}{value}";
    }
}
