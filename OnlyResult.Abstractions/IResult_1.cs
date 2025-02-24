using OnlyResult.Abstractions;

namespace OnlyResult.Abstractions;

/// <summary>
/// Marker interface for results.
/// </summary>
/// <typeparam name="TError"></typeparam>
public interface IResult<TError> where TError : IError
{
    public ImmutableList<TError> Errors { get; }
    
    /// <summary>
    /// Determines whether the result is a success.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Determines whether the result is a failure.
    /// </summary>
    public bool IsFailure { get; }
}

/// <summary>
/// Interface for a typed result.
/// </summary>
/// <typeparam name="TError"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IResult<TResult, TError> : IResult<TError> where TError : class, IError where TResult : IResult<TResult, TError>
{
    static abstract TResult Fail(string errorMessage); 
    static abstract TResult Fail(TError error);
    static abstract TResult Fail(IEnumerable<TError> errors);
    T Match<T>(Func<T> onSuccess, Func<IEnumerable<TError>, T> onFailure);
    TResult Match(Func<TResult> onSuccess);
    void Match(Action onSuccess, Action<IEnumerable<TError>> onFailure);
    void ThrowIfFailed();
}

/// <summary>
/// Interface for a typed result.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="TError"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IResult<TValue, TResult, TError> : IResult<TResult, TError> where TError : class, IError where TResult : IResult<TValue, TResult, TError>
{
    public TValue Value { get; }
    
    public TValue? ValueOrDefault { get; }
    
    /// <summary>
    /// Creates a success result with the specified value.
    /// </summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <see cref="TResult"/> representing a success result with the specified value.</returns>
    public static abstract TResult Ok(TValue value);

    T Match<T>(Func<TValue, T> onSuccess, Func<IEnumerable<TError>, T> onFailure);
    TResult Match(Func<TValue, TResult> onSuccess);
}