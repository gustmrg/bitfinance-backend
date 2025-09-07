using BitFinance.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Data);

        return result.Error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(CreateProblemDetails(result.Error, 404)),
            ErrorType.Validation => new BadRequestObjectResult(CreateProblemDetails(result.Error, 400)),
            ErrorType.Conflict => new ConflictObjectResult(CreateProblemDetails(result.Error, 409)),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(CreateProblemDetails(result.Error, 401)),
            ErrorType.Forbidden => new ObjectResult(CreateProblemDetails(result.Error, 403)) { StatusCode = 403 },
            _ => new ObjectResult(CreateProblemDetails(result.Error, 500)) { StatusCode = 500 }
        };
    }
    
    public static IActionResult ToCreatedResult<T>(
        this Result<T> result, 
        string actionName, 
        string controllerName, 
        object routeValues)
    {
        if (result.IsSuccess)
            return new CreatedAtActionResult(actionName, controllerName, routeValues, result.Data);

        return result.ToActionResult();
    }
    
    private static ProblemDetails CreateProblemDetails(Error error, int statusCode)
    {
        return new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = statusCode,
            Extensions = error.Metadata?.ToDictionary(kvp => (string)kvp.Key, kvp => kvp.Value)
        };
    }
}