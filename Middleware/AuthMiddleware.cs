//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.IdentityModel.Tokens;
//using Resto_Backend.Data;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Resto_Backend.Middleware
//{
//    public class AuthMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly string _secretKey;
//        private readonly string _validIssuer;
//        private readonly string _validAudience;

//        public AuthMiddleware(RequestDelegate next, string secretKey, string validIssuer, string validAudience)
//        {
//            _next = next;
//            _secretKey = secretKey;
//            _validIssuer = validIssuer;
//            _validAudience = validAudience;
//        }

        
//            public async Task InvokeAsync(HttpContext context)
//            {
//                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//                if (token != null)
//                {
//                    try
//                    {
//                        var key = _configuration["Jwt:Key"];
//                        var issuer = _configuration["Jwt:Issuer"];
//                        var audience = _configuration["Jwt:Audience"];

//                        var tokenHandler = new JwtSecurityTokenHandler();
//                        var tokenValidationParameters = new TokenValidationParameters
//                        {
//                            ValidateIssuer = true,
//                            ValidIssuer = issuer,

//                            ValidateAudience = true,
//                            ValidAudience = audience,

//                            ValidateIssuerSigningKey = true,
//                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

//                            ValidateLifetime = true,
//                            ClockSkew = TimeSpan.Zero
//                        };

//                        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

//                        context.User = principal; // Attach the user to the context
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"Token validation failed: {ex.Message}");
//                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                        await context.Response.WriteAsync("Invalid Token");
//                        return;
//                    }
//                }

//                await _next(context);
//            }
//        }
//    }

