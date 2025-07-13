using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DinamicCharts.Application.Helpers.TokenHelper
{
    public class TokenHelper : ITokenHelper
    {
        public string GenerateToken(string username, string password)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f2077ae1-6c74-4bf5-b845-f22581afae45"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = new JwtSecurityToken(
                issuer: "dinamicChart",
                audience: "dinamicChartUser",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
