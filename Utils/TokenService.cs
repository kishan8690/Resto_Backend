using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resto_Backend.Utils
{

    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username)
        {
            // Retrieve JWT Settings from Configuration
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            // Validate Key Length (256 bits minimum for HS256)
            if (key.Length < 32)
            {
                throw new ArgumentException("Key size must be at least 256 bits (32 bytes) for HS256.");
            }

            // Create Signing Credentials
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            // Define Claims
            var claims = new[]
            {
                new Claim("UserName", username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique Identifier for Token
        };

            // Generate Token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            // Return Token as String
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
