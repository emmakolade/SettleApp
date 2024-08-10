using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;


namespace Settle_App.Repositories
{
    public interface IJWTRepository
    {
        JwtToken GenerateAccessToken(IdentityUser identityUser, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class JwtToken
    {
        public string Token { get; set; }
        public long Expiry { get; set; }
    }
}
