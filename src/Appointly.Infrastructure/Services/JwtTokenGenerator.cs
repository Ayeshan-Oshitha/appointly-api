using Appointly.Application.Common.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Appointly.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateAccessToken(Guid userId, string firstName, string lastName, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jshhdgshskdndHHYYnsddfdjf76474734854546kfg"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName , firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName , lastName)
            };

            var token = new JwtSecurityToken(
                issuer: "Appointly",
                audience: "AppointlyUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
