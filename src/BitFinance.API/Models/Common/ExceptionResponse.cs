using System.Net;

namespace BitFinance.API.Models.Common;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);