namespace OnlyResult;

/// <summary>
/// Partial class of <see cref="OnlyResult.Result{TValue}"/> for extensions.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
public partial class Result<TValue>
{
    public static Result<TValue> Try(
        Func<TValue> func,
        Func<Exception, Error>? exceptionHandler = null
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

    public static async Task<Result<TValue>> TryAsync(
        Func<Task<TValue>> func,
        Func<Exception, Error>? exceptionHandler = null
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

    public static async Task<Result<TValue>> TryAsync(
        Func<ValueTask<TValue>> func,
        Func<Exception, Error>? exceptionHandler = null
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
