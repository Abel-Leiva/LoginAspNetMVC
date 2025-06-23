using Microsoft.AspNetCore.Identity;

public static class PasswordHelper
{
    public static string Hashear(string password)
    {
        var hasher = new PasswordHasher<object>();
        return hasher.HashPassword(null, password);
    }

    public static bool Verificar(string hash, string password)
    {
        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(null, hash, password);
        return result == PasswordVerificationResult.Success;
    }
}
