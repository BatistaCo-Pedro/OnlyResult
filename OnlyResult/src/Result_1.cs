namespace OnlyResult;

/// <summary>
/// Default implementation of <see cref="IResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
/// <remarks>
/// Structs don't return null, instead they return their default value.
/// There might be a need to handle classes and structs differently.
/// </remarks>
[Serializable]
public partial class Result<TValue> : Result, IResult<TValue, Result<TValue>, Error>
{
    /// <summary>
    /// The value of the result or null.
    /// </summary>
    [JsonPropertyName("valueOrDefault")]
    public TValue? ValueOrDefault { get; protected init; }

    /// <inheritdoc />
    [JsonIgnore]
    public TValue Value
    {
        get
        {
            ThrowIfFailed();
            return ValueOrDefault!;
        }
        init
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            ValueOrDefault = value;
        }
    }

    protected Result() { }

    /// <summary>
    /// Initializes a new instance of the<see cref="Result{TValue}"/> class with the specified error.
    /// </summary>
    /// <param name="error">The error to initialize with.</param>
    protected Result(Error error)
        : base(error) { }

    protected Result(TValue value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the<see cref="Result{TValue}"/> class with the specified errors.
    /// </summary>
    /// <param name="errors">The errors to initialize with.</param>
    protected Result(ImmutableList<Error> errors)
        : base(errors) { }

    [JsonConstructor]
    protected Result(ImmutableList<Error> errors, TValue? value)
        : base(errors)
    {
        ValueOrDefault = value;
    }

    /// <inheritdoc />
    public static Result<TValue> Ok(TValue value) => new(value);

    public static new Result<TValue> Fail(Error error) => new(error);

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    public new static Result<TValue> Fail(string errorMessage) => Fail(new Error(errorMessage));

    public static new Result<TValue> Fail(IEnumerable<Error> errors) =>
        new(errors.ToImmutableList());

    public Result<TValueToKeep> MergeWith<TValueToKeep>(params Result<TValue>[] results)
    {
        var allResults = new HashSet<Result<TValue>> { this };
        allResults.UnionWith(results);

        return MergeResults<TValueToKeep>(allResults.ToArray());
    }

    public static Result<TValueToKeep> MergeResults<TValueToKeep>(params Result<TValue>[] results)
    {
        TValueToKeep? valueToKeep = default;
        foreach (var result in results)
        {
            if (result.ValueOrDefault is TValueToKeep value)
            {
                valueToKeep = value;
            }

            if (result.IsFailure)
            {
                break;
            }
        }

        return valueToKeep?.Equals(default(TValueToKeep)) is true or null
            ? Result<TValueToKeep>.Fail(results.SelectMany(x => x.Errors))
            : Result<TValueToKeep>.Ok(valueToKeep);
    }

    /// <inheritdoc />
    public Result<TValue> Match(Func<Result<TValue>> onSuccess) => IsSuccess ? onSuccess() : this;

    /// <inheritdoc />
    public T Match<T>(Func<TValue, T> onSuccess, Func<IEnumerable<Error>, T> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Errors);

    /// <inheritdoc />
    public Result<TValue> Match(Func<TValue, Result<TValue>> onSuccess) =>
        IsSuccess ? onSuccess(Value) : this;

    /// <summary>
    /// Implicitly convert an error to a failed result.
    /// </summary>
    /// <param name="value">The value to convert and include in the result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> with the value.</returns>
    public static implicit operator Result<TValue>(TValue value)
    {
        if (value is Result<TValue> result)
            return result;

        return Ok(value);
    }

    /// <summary>
    /// Implicitly convert an error to a failed result.
    /// </summary>
    /// <param name="error">The error to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> with the error.</returns>
    public static implicit operator Result<TValue>(Error? error) => Fail(error ?? Error.Empty);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> with the errors.</returns>
    public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> with the errors.</returns>
    public static implicit operator Result<TValue>(HashSet<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> with the errors.</returns>
    public static implicit operator Result<TValue>(ImmutableList<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a result to its value.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The value of the result.</returns>
    public static implicit operator TValue(Result<TValue> result) => result.Value;

    /// <summary>
    /// Implicitly convert a result to its error list.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The error list of the result.</returns>
    public static implicit operator ImmutableList<Error>(Result<TValue> result) => result.Errors;

    /// <summary>
    /// Deconstruct Result.
    /// </summary>
    /// <param name="isSuccess">Bool defining if the result is successful.</param>
    /// <param name="value">The value of the result in case of success or the default of the value.</param>
    /// <param name="errors">The errors from the result - empty in case of success.</param>
    public void Deconstruct(out bool isSuccess, out TValue? value, out ImmutableList<Error> errors)
    {
        isSuccess = IsSuccess;
        value = ValueOrDefault;
        errors = Errors;
    }

    /// <inheritdoc />
    public override string ToString() => JsonSerializer.Serialize(this);
}
