namespace Result;

/// <summary>
/// Partial class of <see cref="Result.Result"/> for extensions.
/// </summary>
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
public sealed partial class Result
{
    /// <inheritdoc />
    public static Result Try(Action action, Func<Exception, IError>? exceptionHandler = null)
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
    public static async Task<Result> TryAsync(
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
    public static async Task<Result> TryAsync(
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
    public static Result MergeResults(params Result[] results)
    {
        if (results.Length == 0 || results.All(x => x.IsSuccess))
        {
            return Ok();
        }

        return Fail(results.SelectMany(x => x.Errors));
    }
}
