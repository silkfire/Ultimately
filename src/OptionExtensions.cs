namespace Ultimately;

using Reasons;

using System;

/// <summary>
/// Provides extension methods for creating and transforming optional values.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Wraps an existing value in an <see cref="Option{TValue}"/> instance.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(this TValue value) => Optional.Some(value);

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{TValue}"/> instance with a specified success message.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="successMessage">A message describing the reason or origin behind the presence of the optional value.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(this TValue value, string successMessage) => Optional.Some(value, successMessage);

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{TValue}"/> instance with a specified success object.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="success">An object describing the reason or origin behind the presence of the optional value.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(this TValue value, Success success) => Optional.Some(value, success);

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> instance from a specified value.
    /// <para>If the value does not satisfy the given predicate, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="error">An error object describing why the optional is missing its value.</param>
    /// <returns>An optional containing the specified value, if the predicate is satisified.</returns>
    public static Option<TValue> SomeWhen<TValue>(this TValue value, Predicate<TValue> predicate, Error error)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return predicate(value) ? Optional.Some(value) : Optional.None<TValue>(error);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> instance from a specified value and success message.
    /// <para>If the value does not satisfy the given predicate, an empty optional is returned with the specified error message.</para>
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="successMessage">A message describing the reason or origin behind the presence of the optional value.</param>
    /// <param name="errorMessage">A description of why the optional is missing its value if the predicate is not satisfied.</param>
    /// <returns>An optional containing the specified value, if the predicate is satisified.</returns>
    public static Option<TValue> SomeWhen<TValue>(this TValue value, Predicate<TValue> predicate, string successMessage, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return predicate(value) ? Optional.Some(value, successMessage) : Optional.None<TValue>(errorMessage);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> instance from a specified value and success object.
    /// <para>If the value does not satisfy the given predicate, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="success">An object with data describing the reason or origin behind the presence of the optional value.</param>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>An optional containing the specified value, if the predicate is satisified.</returns>
    public static Option<TValue> SomeWhen<TValue>(this TValue value, Predicate<TValue> predicate, Success success, Error error)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return predicate(value) ? Optional.Some(value, success) : Optional.None<TValue>(error);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> instance from a specified value.
    /// <para>If the value satisfies the given predicate, an empty optional is returned with the specified error object attached.</para>
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>An optional containing the specified value, if the predicate is not satisifed.</returns>
    public static Option<TValue> NoneWhen<TValue>(this TValue value, Predicate<TValue> predicate, Error error)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return value.SomeWhen(val => !predicate(val), error);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> instance from a specified value.
    /// <para>If the value is null, an empty optional is returned with the specified error object attached.</para>
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>An optional containing the specified value given that it's not null.</returns>
    public static Option<TValue> SomeNotNull<TValue>(this TValue value, Error error) => value.SomeWhen(val => val != null, error);

    /// <summary>
    /// Converts a <see cref="Nullable{T}"/> to an <see cref="Option{TValue}"/> instance with the specified error message if its value is null.
    /// <para>If its value is null, an empty optional is returned with the specified error object attached.</para>
    /// </summary>
    /// <param name="value">The Nullable&lt;T&gt; instance.</param>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>The <see cref="Option{TValue}"/> instance.</returns>
    public static Option<TValue> ToOption<TValue>(this TValue? value, Error error)
        where TValue : struct
        => value.HasValue ? Optional.Some(value.Value) : Optional.None<TValue>(error);

    /// <summary>
    /// Flattens two nested optionals into one. The resulting optional will be empty if either the inner or outer optional is empty.
    /// </summary>
    /// <param name="nestedOption">The nested optional.</param>
    /// <returns>A flattened optional.</returns>
    public static Option<TValue> Flatten<TValue>(this Option<Option<TValue>> nestedOption) => nestedOption.FlatMap(innerOption => innerOption);

    /// <summary>
    /// Transforms the optional into an optional with a value. The result is flattened, and if the original optional's outcome is unsuccessful, an empty optional is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public static Option<TResult> Map<TResult>(this Option option, Func<TResult> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Func<Error, Option<TResult>> errorFunc = Optional.None<TResult>;

        if (subsequentError != null)
        {
            errorFunc = e => Optional.None<TResult>(subsequentError.CausedBy(e));
        }

        return option.Match(
                            some: s => Optional.Some(mapping(), s),
                            none: errorFunc
                           );
    }

    /// <summary>
    /// Transforms the optional into an optional with a value. The result is flattened, and if the original optional's outcome is unsuccessful, an empty optional is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.<para>Example: "Failed to create object" (child error), Caused by: "Timed out while connecting to service" (antecedent optional with an unsuccessful outcome)</para></param>
    /// <returns>The transformed optional.</returns>
    public static Option<TResult> Map<TResult>(this Option option, Func<Success, TResult> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Func<Error, Option<TResult>> errorFunc = Optional.None<TResult>;

        if (subsequentError != null)
        {
            errorFunc = e => Optional.None<TResult>(subsequentError.CausedBy(e));
        }

        return option.Match(
                            some: s => Optional.Some(mapping(s), s),
                            none: errorFunc
                           );
    }

    /// <summary>
    /// Transforms the optional into an optional with a value. The result is flattened, and if either optional's outcome is unsuccessful, an empty optional is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public static Option<TResult> FlatMap<TResult>(this Option option, Func<Option<TResult>> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option<TResult> result;

        if (option.IsSuccessful)
        {
            result = mapping();

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
            result = Optional.None<TResult>(subsequentError != null ? subsequentError.CausedBy(option.Error) : option.Error);
        }

        return result;
    }

    /// <summary>
    /// Transforms the optional into an optional with a value. The result is flattened, and if either optional's outcome is unsuccessful, an empty optional is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public static Option<TResult> FlatMap<TResult>(this Option option, Func<Success, Option<TResult>> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option<TResult> result;

        if (option.IsSuccessful)
        {
            result = mapping(option.Success);

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
            result = Optional.None<TResult>(subsequentError != null ? subsequentError.CausedBy(option.Error) : option.Error);
        }

        return result;
    }

    /// <summary>
    /// Transforms the value-optional into an optional with an outcome. The result is flattened, and if either the source optional is empty or the resulting optional's outcome is unsuccessful, an optional with an unsuccessful outcome is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public static Option FlatMap<TValue>(this Option<TValue> option, Func<TValue, Option> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option result;

        if (option.HasValue)
        {
            result = mapping(option.Value);

            result.MatchNone(e =>
            {
                if (subsequentError != null)
                {
                    result = Optional.None(subsequentError.CausedBy(e));
                }
            });
        }
        else
        {
            result = Optional.None(subsequentError != null ? subsequentError.CausedBy(option.Error) : option.Error);
        }

        return result;
    }

    /// <summary>
    /// Transforms the value-optional into an optional with an outcome. The result is flattened, and if either the source optional is empty or the resulting optional's outcome is unsuccessful, an optional with an unsuccessful outcome is returned.
    /// </summary>
    /// <param name="option">The optional to transform.</param>
    /// <param name="mapping">The transformation function.</param>
    /// <param name="subsequentError">If the resulting optional is empty, sets its error as the direct reason to the specified subsequent error.</param>
    /// <returns>The transformed optional.</returns>
    public static Option FlatMap<TValue>(this Option<TValue> option, Func<TValue, Success, Option> mapping, Error subsequentError = null)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        Option result;

        if (option.HasValue)
        {
            result = mapping(option.Value, option.Success);

            result.MatchNone(e =>
            {
                if (subsequentError != null)
                {
                    result = Optional.None(subsequentError.CausedBy(e));
                }
            });
        }
        else
        {
            result = Optional.None(subsequentError != null ? subsequentError.CausedBy(option.Error) : option.Error);
        }

        return result;
    }
}
