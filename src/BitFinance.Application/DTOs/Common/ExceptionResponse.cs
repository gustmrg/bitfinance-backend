using System.Net;

namespace BitFinance.Application.DTOs.Common;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
