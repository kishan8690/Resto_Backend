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

            public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
            {
                _next = next;
                _configuration = configuration;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var token = context.Request.Cookies["AuthToken"]; // Get the token from the cookie

                if (string.IsNullOrEmpty(token))
                {
                    // If no token is found, continue with the request
                    await _next(context);
                    return;
                }

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                Console.WriteLine("Middleware halo");
                    // Extract user information (like UserName) from the token claims
                    var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        // Set the UserName in the cookie
                        context.Response.Cookies.Append("UserName", userName, new CookieOptions
                        {
                            HttpOnly = true,  // Prevents JavaScript from accessing the cookie
                            Secure = true,    // Ensures the cookie is sent only over HTTPS
                            SameSite = SameSiteMode.Strict, // Prevents CSRF attacks
                            Expires = DateTime.UtcNow.AddHours(1) // Set cookie expiration
                        });

                        // Set the user name in HttpContext, so it's accessible in controllers
                        context.Items["UserName"] = userName;
                    Console.WriteLine(context.Items["UserName"]);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors related to token parsing or validation
                    Console.WriteLine($"Token validation failed: {ex.Message}");
                }

                // Continue processing the request
                await _next(context);
            }
        
    }
}

