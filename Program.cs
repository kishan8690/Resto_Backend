using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Resto_Backend.Data;
using Resto_Backend.Middleware;

//using Resto_Backend.Middleware;
using Resto_Backend.Utils;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.


builder.Services.AddControllers().
    AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ChefReposetory>();
builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
};
    });

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<AuthMiddleware>();
app.MapControllers();

app.Run();
