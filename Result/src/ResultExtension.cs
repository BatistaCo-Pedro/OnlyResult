namespace Result.src;

/// <summary>
/// Partial class of <see cref="src.Result"/> for extensions.
/// </summary>
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
public sealed partial class Result
{
    /// <inheritdoc />
    public static src.Result Try(
        Action action,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            action();
            return Ok();
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }

    /// <inheritdoc />
    public static async Task<src.Result> TryAsync(
        Func<Task> action,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            await action();
            return Ok();
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }

    /// <inheritdoc />
    public static async Task<src.Result> TryAsync(
        Func<ValueTask> action,
        Func<Exception, IError>? exceptionHandler = null
    )
    {
        try
        {
            await action();
            return Ok();
        }
        catch (Exception ex)
        {
            return Fail(exceptionHandler?.Invoke(ex) ?? new Error(ex.Message));
        }
    }

    /// <inheritdoc />
    public static src.Result MergeResults(params src.Result[] results)
    {
        if (results.Length == 0 || results.All(x => x.IsSuccess))
        {
            return Ok();
        }

        return Fail(results.SelectMany(x => x.Errors));
    }
}
