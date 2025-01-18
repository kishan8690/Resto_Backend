using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Resto_Backend.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resto_Backend.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;
        public AuthMiddleware(RequestDelegate next, IConfiguration configuration,UserRepository userRepository)
        {
            _next = next;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["AuthToken"] ?? context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorize request - No token provided");
                return;
            }
            try
            {
                var tokenHandle = new JwtSecurityTokenHandler();
                var jwtSettings = _configuration.GetSection("Jwt");
                var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                var principal = tokenHandle.ValidateToken(token, validationParameters, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Invalid Access Token - No User ID found");
                        return;
                    }
                    var User = _userRepository.SelectUserByPk(Convert.ToInt32(userId));
                    if (User == null)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("No User Found");
                    }
                    context.Items["User"] = User;
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("User Name" + User.UserName);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync($"Unauthorized: {ex.Message}");
                return;
            }
            await _next(context);
        }
    }
}
