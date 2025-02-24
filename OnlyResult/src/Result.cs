namespace OnlyResult;

/// <summary>
/// Default implementation of <see cref="IResult{TResult, TError}"/>.
/// </summary>
[Serializable]
public partial class Result : IResult<Result, Error>
{
    /// <summary>
    /// Pre allocated instance of <see cref="Result"/> representing a successful result.
    /// </summary>
    private static readonly Result OkResult = new();

    /// <inheritdoc />
    [JsonPropertyName("errors")]
    public ImmutableList<Error> Errors { get; protected init; }

    /// <inheritdoc />
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess => Errors.Count == 0;
    
    /// <inheritdoc />
    [JsonIgnore]
    public bool IsFailure => Errors.Count > 0;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified errors.
    /// Used for serialization.
    /// </summary>
    /// <param name="errors">The errors to set.</param>
    [JsonConstructor]
    protected Result(ImmutableList<Error> errors)
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    protected Result()
    {
        Errors = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified error.
    /// </summary>
    /// <param name="error">The error to initialize with.</param>
    protected Result(Error error)
    {
        Errors = [error];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Result Ok() => OkResult;

    /// <inheritdoc />
    public static Result Fail(Error error) => new(error);

    /// <inheritdoc />
    public static Result Fail(string errorMessage) => Fail(new Error(errorMessage));

    /// <inheritdoc />
    public static Result Fail(IEnumerable<Error> errors) => new(errors.ToImmutableList());
    
    /// <inheritdoc />
    public void Match(Action onSuccess, Action<IEnumerable<Error>> onFailure)
    {
        if (IsSuccess)
        {
            onSuccess();
            return;
        }
        onFailure(Errors);
    }

    /// <inheritdoc />
    public T Match<T>(Func<T> onSuccess, Func<IEnumerable<Error>, T> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Errors);

    /// <inheritdoc />
    public Result Match(Func<Result> onSuccess) => IsSuccess ? onSuccess() : this;
    
    public async Task<T> MatchAsync<T>(Func<Task<T>> onSuccess, Func<IEnumerable<Error>, Task<T>> onFailure) =>
        IsSuccess ? await onSuccess() : await onFailure(Errors);
    
    public async Task<Result> MatchAsync(Func<Task<Result>> onSuccess) =>
        IsSuccess ? await onSuccess() : this;

    /// <inheritdoc />
    public virtual void ThrowIfFailed()
    {
        if (!IsSuccess)
            throw new ResultFailedException<Error>(this);
    }

    /// <summary>
    /// Implicitly convert an error to a failed result.
    /// </summary>
    /// <param name="error">The error to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result"/> with the error.</returns>
    public static implicit operator Result(Error? error) => Fail(error ?? Error.Empty);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result"/> with the errors.</returns>
    public static implicit operator Result(List<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result"/> with the errors.</returns>
    public static implicit operator Result(HashSet<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a list of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert and include in the result.</param>
    /// <returns>A failed <see cref="Result"/> with the errors.</returns>
    public static implicit operator Result(ImmutableList<Error> errors) => Fail(errors);

    /// <summary>
    /// Implicitly convert a result to its error list.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The error list of the result.</returns>
    public static implicit operator ImmutableList<Error>(Result result) => result.Errors;

    /// <summary>
    /// Deconstruct Result.
    /// </summary>
    /// <param name="isSuccess">Bool defining if the result is successful.</param>
    /// <param name="errors">The errors from the result - empty in case of success.</param>
    public void Deconstruct(out bool isSuccess, out ImmutableList<Error> errors)
    {
        isSuccess = IsSuccess;
        errors = Errors;
    }

    /// <inheritdoc />
    public override string ToString() => JsonSerializer.Serialize(this);
}
