namespace OnlyResult.Abstractions;

/// <summary>
/// Defines an actionable result.
/// </summary>
public interface IActionableResult<TValue, TResult> : IResult<TValue>
    where TResult : IActionableResult<TValue, TResult>
{
    /// <summary>Creates a success result with the given value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a success result with the specified value.</returns>
    static abstract TResult Ok(TValue value);

    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result.</returns>
    static abstract TResult Fail();

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result with the specified error message.</returns>
    static abstract TResult Fail(string errorMessage);

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result with the specified error message and metadata.</returns>
    static abstract TResult Fail(string errorMessage, (string Key, object Value) metadata);

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result with the specified error message and metadata.</returns>
    static abstract TResult Fail(string errorMessage, IDictionary<string, object> metadata);

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result with the specified error.</returns>
    static abstract TResult Fail(IError error);

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult" /> representing a failed result with the specified errors.</returns>
    static abstract TResult Fail(IEnumerable<IError> errors);

    /// <summary>
    /// Wraps an action in a try-catch block and returns a result.
    /// </summary>
    /// <param name="action">The action to wrap.</param>
    /// <param name="exceptionHandler">A custom exception handler to handle the caught exceptions.</param>
    /// <returns>A result of type <see cref="TValue"/> representing the result from the action.</returns>
    static abstract TResult Try(
        Func<TValue> action,
        Func<Exception, IError>? exceptionHandler = null
    );

    /// <summary>
    /// Wraps an asynchronous action in a try-catch block and returns a result.
    /// </summary>
    /// <param name="action">The action to wrap.</param>
    /// <param name="exceptionHandler">A custom exception handler to handle the caught exceptions.</param>
    /// <returns>A result of type <see cref="TValue"/> representing the result from the action.</returns>
    static abstract Task<TResult> TryAsync(
        Func<Task<TValue>> action,
        Func<Exception, IError>? exceptionHandler = null
    );

    /// <summary>
    /// Wraps an asynchronous action in a try-catch block and returns a result.
    /// </summary>
    /// <param name="action">The action to wrap.</param>
    /// <param name="exceptionHandler">A custom exception handler to handle the caught exceptions.</param>
    /// <returns>A result of type <see cref="TValue"/> representing the result from the action.</returns>
    static abstract Task<TResult> TryAsync(
        Func<ValueTask<TValue>> action,
        Func<Exception, IError>? exceptionHandler = null
    );

    /// <summary>
    /// Matches a success and failure function for the result.
    /// </summary>
    /// <param name="onSuccess">The function to be called on success.</param>
    /// <param name="onFailure">The function to be called on failure.</param>
    /// <remarks>
    /// This method does not return a value.
    /// </remarks>
    void Match(Action<TValue> onSuccess, Action<IEnumerable<IError>> onFailure);

    /// <summary>
    /// Matches a success and failure function for the result.
    /// </summary>
    /// <param name="onSuccess">The function to be called on success.</param>
    /// <param name="onFailure">The function to be called on failure.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <typeparam name="TValue">The type of the value contained within the result.</typeparam>
    /// <returns>An object of type <see cref="T"/>.</returns>
    T Match<T>(Func<TValue, T> onSuccess, Func<IEnumerable<IError>, T> onFailure);

    /// <summary>
    /// Matches a success function for the result.
    /// </summary>
    /// <param name="onSuccess">The function to be called on success.</param>
    /// <returns>A <see cref="IResult{T}"/> object representing the result of the operation.</returns>
    /// <remarks>
    /// This method uses a default failure function that returns a failed result with the errors in the result.
    /// </remarks>
    T Match<T>(Func<TValue, T> onSuccess);
}
