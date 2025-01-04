namespace Result.src;

/// <summary>
/// Partial class of <see cref="src.Result.Result.Result{TValue}"/> for extensions.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
public sealed partial class Result<TValue>
{
    /// <inheritdoc />
    public static Result<TValue> Try(
        Func<TValue> func,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            return Ok(func());
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }

    /// <inheritdoc />
    public static async Task<Result<TValue>> TryAsync(
        Func<Task<TValue>> func,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            return Ok(await func());
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }

    /// <inheritdoc />
    public static async Task<Result<TValue>> TryAsync(
        Func<ValueTask<TValue>> func,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            return Ok(await func());
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }
}
