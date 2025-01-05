namespace OnlyResult.Abstractions;

/// <summary>
/// Interface for a typed result.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Gets the value of the result, throwing an exception if the result is failed.
    /// </summary>
    /// <returns>The value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get or set the value of a failed result.</exception>
    public TValue Value { get; }
}