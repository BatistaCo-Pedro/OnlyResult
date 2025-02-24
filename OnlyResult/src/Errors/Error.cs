namespace OnlyResult.Errors;

/// <summary>
/// Default implementation of the <see cref="IError"/> interface.
/// </summary>
[Serializable]
public class Error : IError
{
    /// <summary>
    /// Represents an empty error.
    /// </summary>
    public static Error Empty { get; } = new();

    /// <inheritdoc />
    [JsonPropertyName("message")]
    public string Message { get; }

    /// <inheritdoc />
    [JsonPropertyName("metadata")]
    public ImmutableDictionary<string, string> Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IError"/> class.
    /// </summary>
    private Error()
        : this(string.Empty) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/>
    /// class with the specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public Error(string message)
    {
        Message = message;
        Metadata = ImmutableDictionary<string, string>.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/>
    /// class with the specified error message and metadata.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, (string Key, object Value) metadata)
    {
        Message = message;
        var dictionary = new Dictionary<string, string> { { metadata.Key, metadata.Value.ToString() ?? string.Empty } };
        Metadata = dictionary.ToImmutableDictionary();
    }
    
    [JsonConstructor]
    public Error(string message, ImmutableDictionary<string, string> metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <inheritdoc />
    public override string ToString() => JsonSerializer.Serialize(this);
}