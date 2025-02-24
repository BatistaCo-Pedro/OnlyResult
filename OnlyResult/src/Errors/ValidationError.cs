namespace OnlyResult.Errors;

[Serializable]
public class ValidationError : Error
{
    private readonly Type _validatedType;

    [JsonPropertyName("validatedTypeName")]
    public string ValidatedTypeName => _validatedType.Name;

    public ValidationError(Type validatedType, string message)
        : base(message)
    {
        _validatedType = validatedType;
    }

    public ValidationError(Type validatedType, string message, (string Key, object Value) metadata)
        : base(message, metadata)
    {
        _validatedType = validatedType;
    }

    [JsonConstructor]
    public ValidationError(
        Type validatedType,
        string message,
        ImmutableDictionary<string, string> metadata
    )
        : base(message, metadata)
    {
        _validatedType = validatedType;
    }

    /// <inheritdoc />
    public override string ToString() => JsonSerializer.Serialize(this);
}
