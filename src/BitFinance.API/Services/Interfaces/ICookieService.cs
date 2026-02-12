namespace BitFinance.API.Services.Interfaces;

public interface ICookieService
{
    void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expires);

    string? GetRefreshTokenFromCookie(HttpRequest request);

    void RemoveRefreshTokenCookie(HttpResponse response);
}
