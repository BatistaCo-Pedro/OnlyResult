namespace OnlyResult.Abstractions;

/// <summary>
/// Defines an error with a message and associated metadata.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the metadata associated with the error.
    /// </summary>
    /// <remarks>The metadata is represented as a dictionary of key-value pairs.</remarks>
    public ImmutableDictionary<string, string> Metadata { get; }
}
