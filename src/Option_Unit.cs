namespace Ultimately;

using Reasons;

using System;
using System.Collections.Generic;

/// <summary>
/// An optional that represents either a successful or an unsuccessful outcome.
/// </summary>
public readonly struct Option
{
    /// <summary>
    /// Checks whether the optional's outcome is successful.
    /// </summary>
    public bool IsSuccessful { get; }

    internal Success Success { get; }

    internal Error Error { get; }

    internal Option(bool isSuccessful, Success success, Error error)
    {
        IsSuccessful = isSuccessful;
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Returns an enumerator for the optional.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<Success> GetEnumerator()
    {
        if (IsSuccessful)
        {
            yield return Success;
        }
    }

    /// <summary>
    /// Returns an alternative optional if this optional's outcome is unsuccessful.
    /// </summary>
    /// <param name="alternativeOption">The alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option Else(Option alternativeOption) => IsSuccessful ? this : alternativeOption;

    /// <summary>
    /// Creates an alternative optional if this optional's outcome is unsuccessful.
    /// </summary>
    /// <param name="alternativeOptionFactory">A factory function to create an alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option Else(Func<Option> alternativeOptionFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeOptionFactory);

        return IsSuccessful ? this : alternativeOptionFactory();
    }

    /// <summary>
    /// Evaluates a specified function, based on whether the optional's outcome is successful or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the outcome is successful.</param>
    /// <param name="none">The function to evaluate if the outcome is unsuccessful.</param>
    /// <returns>The result of the evaluated function.</returns>
    public TResult Match<TResult>(Func<TResult> some, Func<Error, TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);

        ArgumentNullException.ThrowIfNull(none);

        return IsSuccessful ? some() : none(Error);
    }

    /// <summary>
    /// Evaluates a specified function, based on whether the optional's outcome is successful or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the outcome is successful.</param>
    /// <param name="none">The function to evaluate if the outcome is unsuccessful.</param>
    /// <returns>The result of the evaluated function.</returns>
    public TResult Match<TResult>(Func<Success, TResult> some, Func<Error, TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);

        ArgumentNullException.ThrowIfNull(none);

        return IsSuccessful ? some(Success) : none(Error);
    }

    /// <summary>
    /// Evaluates a specified action, based on whether the optional's outcome is successful or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the optional's outcome is succesful.</param>
    /// <param name="none">The function to evaluate if the optional's outcome is unsuccesful.</param>
    public void Match(Action some, Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(some);

        ArgumentNullException.ThrowIfNull(none);

        if (IsSuccessful)
        {
            some();
        }
        else
        {
            none(Error);
        }
    }

    /// <summary>
    /// Evaluates a specified action, based on whether the optional's outcome is successful or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the optional's outcome is succesful.</param>
    /// <param name="none">The function to evaluate if the optional's outcome is unsuccesful.</param>
    public void Match(Action<Success> some, Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(some);

        ArgumentNullException.ThrowIfNull(none);

        if (IsSuccessful)
        {
            some(Success);
        }
        else
        {
            none(Error);
        }
    }

    /// <summary>
    /// Evaluates a specified action if the optional's outcome is succesful.
    /// </summary>
    /// <param name="some">The action to evaluate if the optional's outcome is succesful.</param>
    public void MatchSome(Action some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (IsSuccessful)
        {
            some();
        }
    }

    /// <summary>
    /// Evaluates a specified action if the optional's outcome is succesful.
    /// </summary>
    /// <param name="some">The action to evaluate if the optional's outcome is succesful.</param>
    public void MatchSome(Action<Success> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (IsSuccessful)
        {
            some(Success);
        }
    }

    /// <summary>
    /// Evaluates a specified action if outcome is unsuccesful.
    /// </summary>
    /// <param name="none">The action to evaluate if outcome is unsuccesful.</param>
    public void MatchNone(Action<Error> none)
    {
        ArgumentNullException.ThrowIfNull(none);

        if (!IsSuccessful)
        {
            none(Error);
        }
    }

    /// <summary>
    /// Transforms the optional into another optional. The result is flattened, and if either optional's outcome is unsuccessful, an unsuccessful optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional's outcome is unsuccessful, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option FlatMap(Func<Option> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option result;

        if (IsSuccessful)
        {
            result = mapping();

            if (!result.IsSuccessful)
            {
                if (subsequentError != null)
                {
                    result = Optional.None(subsequentError.CausedBy(result));
                }
            }
        }
        else
        {
            result = Optional.None(subsequentError != null ? subsequentError.CausedBy(this) : Error);
        }

        return result;
    }

    /// <summary>
    /// Transforms the optional into another optional. The result is flattened, and if either optional's outcome is unsuccessful, an unsuccessful optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional's outcome is unsuccessful, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public Option FlatMap(Func<Success, Option> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option result;

        if (IsSuccessful)
        {
            result = mapping(Success);

            if (!result.IsSuccessful)
            {
                if (subsequentError != null)
                {
                    result = Optional.None(subsequentError.CausedBy(result));
                }
            }
        }
        else
        {
            result = Optional.None(subsequentError != null ? subsequentError.CausedBy(this) : Error);
        }

        return result;
    }

    /// <summary>
    /// If the current optional's outcome is successful, sets its success reason as the direct reason to the specified subsequent success reason.
    /// </summary>
    /// <param name="success">The successful optional's success reason will be set as the cause of the specified success reason.</param>
    public Option FlatMapSome(Success success)
    {
        ArgumentNullException.ThrowIfNull(success);

        if (!IsSuccessful)
        {
            return Optional.Some(success.EnabledBy(this));
        }

        return this;
    }

    /// <summary>
    /// If the current optional's outcome is unsuccessful, sets its error as the direct reason to the specified subsequent error.
    /// </summary>
    /// <param name="subsequentError">The error of the unsuccessful optional will be set as the cause of the specified error.</param>
    public Option FlatMapNone(Error subsequentError)
    {
        ArgumentNullException.ThrowIfNull(subsequentError);

        if (!IsSuccessful)
        {
            return Optional.None(subsequentError.CausedBy(this));
        }

        return this;
    }

    /// <summary>
    /// Returns a string that represents the current optional.
    /// </summary>
    /// <returns>A string that represents the current optional.</returns>
    public override string ToString()
    {
        if (IsSuccessful)
        {
            return $"Some({(Success.Message != "" || Success.Metadata.Count > 0 ? Success.ToString() : "")})";
        }

        return $"None{(Error != null ? $"(Error={Error})" : "")}";
    }
}
