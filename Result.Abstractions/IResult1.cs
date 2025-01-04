namespace Result.Abstractions;

/// <summary>
/// Interface for a result.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result was successful.
    /// </summary>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a collection of errors associated with the result.
    /// </summary>
    /// <returns>
    /// An <see cref="ImmutableArray"/> of <see cref="IError"/> representing the errors.
    /// </returns>
    ImmutableList<IError> Errors { get; }

    /// <summary>
    /// Checks if the result contains an error of the specific type.
    /// </summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns><c>true</c> if an error of the specified type is present, otherwise <c>false</c>.</returns>
    bool HasError<TError>()
        where TError : IError;

    /// <summary>
    /// Checks if the result contains an error of the specific type.
    /// </summary>
    /// <param name="errorType">The type of error to check for.</param>
    /// <returns><c>true</c> if an error of the specified type is present; otherwise, <c>false</c>.</returns>
    bool HasError(Type errorType);

    /// <summary>
    /// Throws an exception if the result is failed.
    /// </summary>
    void ThrowIfFailed();
}