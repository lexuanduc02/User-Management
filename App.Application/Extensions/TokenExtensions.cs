using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Application.Extensions
{
    public static class TokenExtensions
    {
        public static string GenerateAccessToken(List<Claim> claims, string issuer, string audience, string key, string expireIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDesciptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddSeconds(Convert.ToDouble(expireIn)),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDesciptior);

            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
