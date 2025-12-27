using BitFinance.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        Func<T, object> mapper)
    {
        if (result.IsSuccess)
            return new OkObjectResult(mapper(result.Value));

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToCreatedResult<T>(
        this Result<T> result,
        string actionName,
        object routeValues,
        Func<T, object>? mapper = null)
    {
        if (result.IsSuccess)
        {
            var value = mapper is not null ? mapper(result.Value) : result.Value;
            return new CreatedAtActionResult(actionName, null, routeValues, value);
        }

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToNoContentResult(this Result result)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToNoContentResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return ToProblemDetails(result.Errors);
    }

    public static IActionResult ToFileResult<T>(
        this Result<T> result,
        Func<T, (Stream Stream, string ContentType, string FileName)> mapper)
    {
        if (result.IsSuccess)
        {
            var (stream, contentType, fileName) = mapper(result.Value);
            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };
        }

        return ToProblemDetails(result.Errors);
    }

    private static IActionResult ToProblemDetails(IReadOnlyList<Error> errors)
    {
        if (errors.Count == 0)
            return new ObjectResult("An unexpected error occurred")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(firstError.Type),
            Detail = firstError.Message,
            Extensions =
            {
                ["errors"] = errors.Select(e => new
                {
                    e.Code,
                    e.Message,
                    e.Metadata
                }).ToList()
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private static string GetTitle(ErrorType type) => type switch
    {
        ErrorType.Validation => "Validation Error",
        ErrorType.NotFound => "Resource Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        _ => "Server Error"
    };
}
