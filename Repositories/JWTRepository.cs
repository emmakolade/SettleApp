using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Settle_App.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Settle_App.Repositories
{
    public class JWTRepository : IJWTRepository
    {
        private readonly IConfiguration configuration;

        public JWTRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public JwtToken GenerateAccessToken(IdentityUser identityUser, string role)
        {
            //claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, identityUser.Email),
                new(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenExpiry = DateTime.Now.AddMinutes(15);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: tokenExpiry,
                signingCredentials: credentials
                );
            var accesstoken = new JwtSecurityTokenHandler().WriteToken(token);
            var expiryTimestamp = new DateTimeOffset(tokenExpiry).ToUnixTimeSeconds();
            return new JwtToken
            {
                Token = accesstoken,
                Expiry = expiryTimestamp

            };

        

        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng =RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateLifetime = false // we don't care about the token's expiration date here
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
