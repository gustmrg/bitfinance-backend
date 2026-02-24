using System.Security.Cryptography;
using System.Text;

namespace BitFinance.Business.Helpers;

public static class TokenHasher
{
    public static string GenerateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant();
    }

    public static string HashToken(string token)
    {
        return Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}