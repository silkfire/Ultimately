namespace Ultimately;

using Reasons;

using System;
using System.Threading.Tasks;

/// <summary>
/// Provides a set of methods for creating optional values.
/// </summary>
public static class Optional
{
    /// <summary>
    /// Creates an optional with a successful outcome.
    /// </summary>
    /// <returns>An optional with a successful outcome.</returns>
    public static Option Some() => Some(Success.Default);

    /// <summary>
    /// Creates an optional with a successful outcome.
    /// </summary>
    /// <param name="success">A success object with data describing the successful outcome.</param>
    /// <returns>An optional with a successful outcome.</returns>
    public static Option Some(Success success) => new(true, success, null);

    /// <summary>
    /// Creates an optional with a successful outcome by using the success object of the specified optional with a value.
    /// <para>The provided optional cannot be empty.</para>
    /// </summary>
    /// <param name="some">An optional with a value whose success object to copy.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option Some<TResult>(Option<TResult> some)
    {
        if (!some.HasValue)
        {
            throw new ArgumentException("Source optional must have a value");
        }

        return new Option(true, some.Success, null);
    }

    /// <summary>
    /// Creates an optional with an unsuccessful outcome.
    /// </summary>
    /// <param name="message">A description of the error that caused the unsuccessful outcome.</param>
    /// <returns>An optional with an unsuccessful outcome.</returns>
    public static Option None(string message) => None(Error.Create(message));

    /// <summary>
    /// Creates an optional with an unsuccessful outcome.
    /// </summary>
    /// <param name="message">A description of the error that caused the unsuccessful outcome.</param>
    /// <param name="exception">An exception instance to attach to the error property of the unsuccessful optional.</param>
    /// <returns>An optional with an unsuccessful outcome.</returns>
    public static Option None(string message, Exception exception) => None(Error.Create(message).CausedBy(exception));

    /// <summary>
    /// Creates an optional with an unsuccessful outcome from an exception.
    /// </summary>
    /// <param name="exception">An exception instance to create an optional with an unsuccessful outcome from.</param>
    /// <returns>An optional with an unsuccessful outcome.</returns>
    public static Option None(Exception exception) => None(ExceptionalError.Create(exception));

    /// <summary>
    /// Creates an optional with an unsuccessful outcome.
    /// </summary>
    /// <param name="error">An error object with data describing what caused the unsuccessful outcome.</param>
    /// <returns>An optional with an unsuccessful outcome.</returns>
    public static Option None(Error error) => new(false, null, error);

    /// <summary>
    /// Creates an optional with an unsuccessful outcome by using the error object of an empty optional.
    /// </summary>
    /// <param name="none">An empty optional whose error object to copy.</param>
    /// <returns>An optional with an unsuccessful outcome based on the specified empty optional.</returns>
    public static Option None<TValue>(Option<TValue> none)
    {
        if (none.HasValue)
        {
            throw new ArgumentException("Source optional must be empty");
        }

        return new Option(false, null, none.Error);
    }

    /// <summary>
    /// Creates an <see cref="Option"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>If the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="error">An error object to attach to the optional when its outcome is unsuccessful.</param>
    /// <returns>An optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static Option SomeWhen(bool predicate, Error error)
    {
        return predicate ? Some() : None(error);
    }

    /// <summary>
    /// Creates an <see cref="Option"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>If the predicate evaluates to true, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="error">An error object with data describing what caused the unsuccessful outcome.</param>
    /// <returns>An optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static Option NoneWhen(bool predicate, Error error)
    {
        return !predicate ? Some() : None(error);
    }

    /// <summary>
    /// Creates an <see cref="Option"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>If the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="success">A success object with data describing the successful outcome.</param>
    /// <param name="error">An error object to attach to the optional when its outcome is unsuccessful.</param>
    /// <returns>An optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static Option SomeWhen(bool predicate, Success success, Error error)
    {
        return predicate ? Some(success) : None(error);
    }

    /// <summary>
    /// Creates an <see cref="Option"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>If the predicate evaluates to true, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="success">A success object with data describing the successful outcome.</param>
    /// <param name="error">An error object with data describing what caused the unsuccessful outcome.</param>
    /// <returns>An optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static Option NoneWhen(bool predicate, Success success, Error error)
    {
        return !predicate ? Some(success) : None(error);
    }



    /// <summary>
    /// Wraps an existing value in an <see cref="Option{T}"/> instance.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(TValue value) => new(true, value, Success.Default, null);

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{T}"/> instance with a specified success message.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="successMessage">A message describing the reason or origin behind the presence of the optional value.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(TValue value, string successMessage) => new(true, value, Success.Create(successMessage), null);

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{T}"/> instance with a specified success object.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="success">An object with data describing the reason or origin behind the presence of the optional value.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(TValue value, Success success) => new(true, value, success, null);

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{T}"/> instance with the success object of the specified optional.
    /// <para>The outcome of the provided optional must be successful.</para>
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="some">A successful optional whose success object to copy.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue>(TValue value, Option some)
    {
        if (!some.IsSuccessful)
        {
            throw new ArgumentException("Source optional must be successful");
        }

        return new Option<TValue>(true, value, some.Success, null);
    }

    /// <summary>
    /// Wraps an existing value in an <see cref="Option{T}"/> instance with the success object of the specified optional.
    /// <para>The provided optional cannot be empty.</para>
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <param name="some">An optional with a value whose success object to copy.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<TValue> Some<TValue, TResult>(TValue value, Option<TResult> some)
    {
        if (!some.HasValue)
        {
            throw new ArgumentException("Source optional must have a value");
        }

        return new Option<TValue>(true, value, some.Success, null);
    }

    /// <summary>
    /// Creates an empty <see cref="Option{TValue}"/> instance with a specified error message.
    /// </summary>
    /// <param name="message">A description of why the optional is missing its value.</param>
    /// <returns>An empty optional.</returns>
    public static Option<TValue> None<TValue>(string message) => None<TValue>(Error.Create(message));

    /// <summary>
    /// Creates an empty <see cref="Option{TValue}"/> instance with a specified error message and exception.
    /// </summary>
    /// <param name="message">A description of why the optional is missing its value.</param>
    /// <param name="exception">An exception instance to attach to error property of the empty optional.</param>
    /// <returns>An empty optional.</returns>
    public static Option<TValue> None<TValue>(string message, Exception exception) => None<TValue>(Error.Create(message).CausedBy(exception));

    /// <summary>
    /// Creates an empty <see cref="Option{TValue}"/> instance with the specified exception.
    /// </summary>
    /// <param name="exception">An exception instance to create an empty optional from.</param>
    /// <returns>An empty optional.</returns>
    public static Option<TValue> None<TValue>(Exception exception) => None<TValue>(ExceptionalError.Create(exception));

    /// <summary>
    /// Creates an empty <see cref="Option{TValue}"/> instance with a specified error object.
    /// </summary>
    /// <param name="error">An error object with data describing why the optional is missing its value.</param>
    /// <returns>An empty optional.</returns>
    public static Option<TValue> None<TValue>(Error error) => new(false, default, null, error);

    /// <summary>
    /// Creates an empty optional with the error object of an optional with an unsuccessful outcome.
    /// </summary>
    /// <param name="none">An empty optional whose error object to copy.</param>
    /// <returns>An empty optional based on the specified optional with an unsuccessful outcome.</returns>
    public static Option<TValue> None<TValue>(Option none)
    {
        if (none.IsSuccessful)
        {
            throw new ArgumentException("Source optional must be unsuccessful");
        }

        return new Option<TValue>(false, default, null, none.Error);
    }

    /// <summary>
    /// Creates a new empty optional with the error object of the specified empty optional.
    /// </summary>
    /// <param name="none">An empty optional whose error object to copy.</param>
    /// <returns>An empty optional based on the specified empty optional.</returns>
    public static Option<TResult> None<TValue, TResult>(Option<TValue> none)
    {
        if (none.HasValue)
        {
            throw new ArgumentException("Source optional must be empty");
        }

        return new Option<TResult>(false, default, null, none.Error);
    }


    /// <summary>
    /// Creates a <see cref="LazyOption"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>When resolved, if the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="error">An error object to attach to the resolved optional in case its outcome is unsuccessful.</param>
    /// <returns>A deferred optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static LazyOption Lazy(Func<bool> predicate, Error error)
    {
        return Lazy(predicate, Success.Default, error);
    }

    /// <summary>
    /// Creates a <see cref="LazyOption"/> instance whose outcome depends on the satisfaction of the given predicate.
    /// <para>When resolved, if the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicate">The predicate to satisfy.</param>
    /// <param name="success">A success object with data describing the successful outcome.</param>
    /// <param name="error">An error object to attach to the resolved optional in case its outcome is unsuccessful.</param>
    /// <returns>A deferred optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static LazyOption Lazy(Func<bool> predicate, Success success, Error error)
    {
        return new LazyOption(predicate, success, error);
    }

    /// <summary>
    /// Creates a <see cref="LazyOption"/> instance whose outcome depends on the satisfaction of the given predicate task.
    /// <para>When resolved, if the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicateTask">The predicate to satisfy.</param>
    /// <param name="error">An error object to attach to the resolved optional in case its outcome is unsuccessful.</param>
    /// <returns>A deferred optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static LazyOptionAsync LazyAsync(Func<Task<bool>> predicateTask, Error error)
    {
        return new LazyOptionAsync(predicateTask, Success.Default, error);
    }

    /// <summary>
    /// Creates a <see cref="LazyOption"/> instance whose outcome depends on the satisfaction of the given predicate task.
    /// <para>When resolved, if the predicate evaluates to false, an empty optional is returned with the specified error object.</para>
    /// </summary>
    /// <param name="predicateTask">The predicate to satisfy.</param>
    /// <param name="success">A success object with data describing the successful outcome.</param>
    /// <param name="error">An error object to attach to the resolved optional in case its outcome is unsuccessful.</param>
    /// <returns>A deferred optional whose outcome depends on the satisfaction of the provided predicate.</returns>
    public static LazyOptionAsync LazyAsync(Func<Task<bool>> predicateTask, Success success, Error error)
    {
        return new LazyOptionAsync(predicateTask, success, error);
    }
}
