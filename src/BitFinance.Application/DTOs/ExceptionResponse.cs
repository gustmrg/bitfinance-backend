using System.Net;

namespace BitFinance.Application.DTOs;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);