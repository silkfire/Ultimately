namespace Ultimately;

using Reasons;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Wraps an optional value that may or may not exist depending on a predetermined set of business rules.
/// </summary>
/// <typeparam name="TValue">The type of the value to be wrapped.</typeparam>
[Serializable]
public readonly struct Option<TValue>
{
    /// <summary>
    /// Indicates whether or not the optional contains a value.
    /// </summary>
    public bool HasValue { get; }

    internal TValue Value { get; }

    internal Success Success { get; }

    internal Error Error { get; }

    internal Option(bool hasValue, TValue value, Success success, Error error)
    {
        HasValue = hasValue;
        Value = value;
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Converts the current optional into an enumerable with one or zero elements.
    /// </summary>
    /// <returns>A corresponding enumerable.</returns>
    public IEnumerable<TValue> ToEnumerable()
    {
        if (HasValue)
        {
            yield return Value;
        }
    }

    /// <summary>
    /// Returns an enumerator for the optional.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<(TValue Value, Success Success)> GetEnumerator()
    {
        if (HasValue)
        {
            yield return (Value, Success);
        }
    }

    /// <summary>
    /// Determines if the current optional contains a specified value.
    /// </summary>
    /// <param name="value">The value to locate.</param>
    /// <returns>A boolean indicating whether or not the value was found.</returns>
    public bool Contains(TValue value)
    {
        if (HasValue)
        {
            if (Value == null)
            {
                return value == null;
            }

            return Value.Equals(value);
        }

        return false;
    }

    /// <summary>
    /// Determines if the current optional contains a value satisfying a specified predicate.
    /// </summary>
    /// <param name="predicate">A predicate to test the optional value against.</param>
    /// <returns>A boolean indicating whether or not the predicate was satisfied.</returns>
    public bool Exists(Predicate<TValue> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return HasValue && predicate(Value);
    }

    /// <summary>
    /// Returns the existing value if present, or otherwise an alternative value.
    /// </summary>
    /// <param name="alternative">The alternative value.</param>
    /// <returns>The existing or alternative value.</returns>
    public TValue ValueOr(TValue alternative) => HasValue ? Value : alternative;

    /// <summary>
    /// Returns the existing value if present, or otherwise an alternative value.
    /// </summary>
    /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
    /// <returns>The existing or alternative value.</returns>
    public TValue ValueOr(Func<TValue> alternativeFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeFactory);

        return HasValue ? Value : alternativeFactory();
    }

    /// <summary>
    /// Uses an alternative value if no existing value is present.
    /// </summary>
    /// <param name="alternative">The alternative value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<TValue> Or(TValue alternative) => HasValue ? this : Optional.Some(alternative);

    /// <summary>
    /// Uses an alternative value if no existing value is present and attaches the specified success object.
    /// </summary>
    /// <param name="alternative">The alternative value.</param>
    /// <param name="success">An object with data describing the reason or origin behind the presence of the alternative optional value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<TValue> Or(TValue alternative, Success success) => HasValue ? this : Optional.Some(alternative, success);

    /// <summary>
    /// Uses an alternative value if no existing value is present.
    /// </summary>
    /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<TValue> Or(Func<TValue> alternativeFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeFactory);

        return HasValue ? this : Optional.Some(alternativeFactory());
    }

    /// <summary>
    /// Uses an alternative value if no existing value is present and attaches the specified success object.
    /// </summary>
    /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
    /// <param name="success">An object with data describing the reason or origin behind the presence of the alternative optional value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<TValue> Or(Func<TValue> alternativeFactory, Success success)
    {
        ArgumentNullException.ThrowIfNull(alternativeFactory);

        return HasValue ? this : Optional.Some(alternativeFactory(), success);
    }

    /// <summary>
    /// Uses an alternative optional, if no existing value is present.
    /// </summary>
    /// <param name="alternativeOption">The alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option<TValue> Else(Option<TValue> alternativeOption) => HasValue ? this : alternativeOption;

    /// <summary>
    /// Uses an alternative optional, if no existing value is present.
    /// </summary>
    /// <param name="alternativeOptionFactory">A factory function to create an alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option<TValue> Else(Func<Option<TValue>> alternativeOptionFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeOptionFactory);

        return HasValue ? this : alternativeOptionFactory();
    }

    /// <summary>
    /// Evaluates a specified function, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the value is present.</param>
    /// <param name="none">The function to evaluate if the value is missing.</param>
    /// <returns>The result of the evaluated function.</returns>
    public TResult Match<TResult>(Func<TValue, TResult> some, Func<Error, TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        return HasValue ? some(Value) : none(Error);
    }

    /// <summary>
    /// Evaluates a specified function, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the value is present.</param>
    /// <param name="none">The function to evaluate if the value is missing.</param>
    /// <returns>The result of the evaluated function.</returns>
    public TResult Match<TResult>(Func<TValue, Success, TResult> some, Func<Error, TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        return HasValue ? some(Value, Success) : none(Error);
    }

    /// <summary>
    /// Evaluates a specified action, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    /// <param name="none">The action to evaluate if the value is missing.</param>
    public void Match(Action<TValue> some, Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        if (HasValue)
        {
            some(Value);
        }
        else
        {
            none(Error);
        }
    }

    /// <summary>
    /// Evaluates a specified action, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    /// <param name="none">The action to evaluate if the value is missing.</param>
    public void Match(Action<TValue, Success> some, Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        if (HasValue)
        {
            some(Value, Success);
        }
        else
        {
            none(Error);
        }
    }

    /// <summary>
    /// Evaluates a specified action if a value is present.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    public void MatchSome(Action<TValue> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (HasValue)
        {
            some(Value);
        }
    }

    /// <summary>
    /// Evaluates a specified action if a value is present.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    public void MatchSome(Action<TValue, Success> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (HasValue)
        {
            some(Value, Success);
        }
    }

    /// <summary>
    /// Evaluates a specified action if no value is present.
    /// </summary>
    /// <param name="none">The action to evaluate if the value is missing.</param>
    public void MatchNone(Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(none);

        if (!HasValue)
        {
            none(Error);
        }
    }

    /// <summary>
    /// Transforms the inner value of an optional. If the instance is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> Map<TResult>(Func<TValue, TResult> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Func<Error, Option<TResult>> errorFunc = Optional.None<TResult>;

        if (subsequentError != null)
        {
            errorFunc = e => Optional.None<TResult>(subsequentError.CausedBy(e));
        }

        return Match(
                     some: v => Optional.Some(mapping(v)),
                     none: errorFunc
                    );
    }

    /// <summary>
    /// Transforms the inner value of an optional. If the instance is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> Map<TResult>(Func<TValue, Success, TResult> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Func<Error, Option<TResult>> errorFunc = Optional.None<TResult>;

        if (subsequentError != null)
        {
            errorFunc = e => Optional.None<TResult>(subsequentError.CausedBy(e));
        }

        return Match(
                     some: (v, s) => Optional.Some(mapping(v, s)),
                     none: errorFunc
                    );
    }

    /// <summary>
    /// Transforms the inner value of an optional into another optional. The result is flattened, and if either is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> FlatMap<TResult>(Func<TValue, Option<TResult>> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option<TResult> result;

        if (HasValue)
        {
            result = mapping(Value);

            result.MatchNone(e =>
            {
                if (subsequentError != null)
                {
                    result = Optional.None<TResult>(subsequentError.CausedBy(e));
                }
            });
        }
        else
        {
            result = Optional.None<TResult>(subsequentError != null ? subsequentError.CausedBy(Error) : Error);
        }

        return result;
    }

    /// <summary>
    /// Transforms the inner value of an optional into another optional. The result is flattened, and if either is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> FlatMap<TResult>(Func<TValue, Success, Option<TResult>> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option<TResult> result;

        if (HasValue)
        {
            result = mapping(Value, Success);

            result.MatchNone(e =>
            {
                if (subsequentError != null)
                {
                    result = Optional.None<TResult>(subsequentError.CausedBy(e));
                }
            });
        }
        else
        {
            result = Optional.None<TResult>(subsequentError != null ? subsequentError.CausedBy(Error) : Error);
        }

        return result;
    }

    /// <summary>
    /// If the current optional has a value, sets its success reason as the direct reason to the specified subsequent success reason.
    /// </summary>
    /// <param name="success">The current optional's success reason will be set as the cause of the specified success reason.</param>
    public Option<TValue> FlatMapSome(Success success)
    {
        ArgumentNullException.ThrowIfNull(success);

        if (!HasValue)
        {
            return Optional.Some(Value, success.EnabledBy(this));
        }

        return this;
    }

    /// <summary>
    /// If the current optional is empty, sets its error as the direct reason to the specified subsequent error.
    /// </summary>
    /// <param name="subsequentError">If the current optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    public Option<TValue> FlatMapNone(Error subsequentError)
    {
        ArgumentNullException.ThrowIfNull(subsequentError);

        if (!HasValue)
        {
            return Optional.None<TValue>(subsequentError.CausedBy(this));
        }

        return this;
    }

    /// <summary>
    /// Empties an optional and attaches an error object if the specified predicate is not satisfied.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="predicateFailure">An error object describing why the predicate failed.<para>Example: "Value must be greater than 10. Was 2."</para></param>
    /// <param name="subsequentError">If the current optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The filtered optional.</returns>
    public Option<TValue> Filter(Predicate<TValue> predicate, Error predicateFailure, Error subsequentError = null)
    {
        return Filter(predicate, _ => predicateFailure, subsequentError);
    }

    /// <summary>
    /// Empties an optional and attaches an error object if the specified predicate is not satisfied.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="predicateFailure">An error object describing why the predicate failed.<para>Example: "Value must be greater than 10. Was 2."</para></param>
    /// <param name="subsequentError">If the current optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The filtered optional.</returns>
    public Option<TValue> Filter(Predicate<TValue> predicate, Func<TValue, Error> predicateFailure, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (var (value, _) in this)
        {
            return predicate(value) ? this : Optional.None<TValue>(predicateFailure(value));
        }

        return Optional.None<TValue>(subsequentError?.CausedBy(Error) ?? Error);
    }

    /// <summary>
    /// Empties an optional and attaches an error object if the value is null.
    /// </summary>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>The filtered optional.</returns>
    public Option<TValue> NotNull(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return HasValue && Value == null ? Optional.None<TValue>(error) : this;
    }


    /// <summary>
    /// Returns a string that represents the current optional.
    /// </summary>
    /// <returns>A string that represents the current optional.</returns>
    public override string ToString()
    {
        if (HasValue)
        {
            string valueString;

            if (Value == null)
            {
                valueString = "null";
            }
            else
            {
                if (Value is ICollection c) valueString = $"Count = {c.Count}";
                else if (typeof(TValue) == typeof(IReadOnlyCollection<>))
                {
                    var valueType = Value.GetType().GetInterfaces().Single(i => i == typeof(IReadOnlyCollection<>));

                    valueString = $"Count = {valueType.GetProperty("Count")!.GetValue(valueType)}";
                }
                else
                {
                    valueString = Value.ToString();
                }
            }

            return $"Some({valueString}{(Success.Message != "" || Success.Metadata.Count > 0 ? $" | {Success}" : "")})";
        }

        return $"None{(Error != null ? $"(Error={Error})" : "")}";
    }
}
