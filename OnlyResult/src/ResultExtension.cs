namespace OnlyResult;

/// <summary>
/// Partial class of <see cref="OnlyResult.Result"/> for extensions.
/// </summary>
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
public partial class Result
{
    public static Result Try(Action action, Func<Exception, Error>? exceptionHandler = null)
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

    public static async Task<Result> TryAsync(
        Func<Task> action,
        Func<Exception, Error>? exceptionHandler = null
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

    public static async Task<Result> TryAsync(
        Func<ValueTask> action,
        Func<Exception, Error>? exceptionHandler = null
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
    
    public Result MergeWith(params IResult<Error>[] results)
    {
        var allResults = new HashSet<IResult<Error>> { this };
        allResults.UnionWith(results);

        return MergeResults(allResults.ToArray());
    }
    
    public static Result MergeResults(params IResult<Error>[] results)
    {
        if (results.Length == 0 || results.All(x => x.IsSuccess))
        {
            return Ok();
        }

        return Fail(results.SelectMany(x => x.Errors));
    }
}
