using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Vasileva_crosspl.Models;
using Vasileva_crosspl.Services;
using Vasileva_crosspl.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=maintenance.db"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.SigningKey,
            ValidateIssuerSigningKey = true,
            // Важно для работы Roles в [Authorize(Roles = "admin")]
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

builder.Services.AddControllers();
builder.Services.AddScoped<Service>();


// Разрешаем запросы с фронтенда (Angular по умолчанию работает на порту 4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Обязательно для JWT токенов!
    });
});

var app = builder.Build();

// Включаем эту политику сразу после builder.Build()
app.UseCors("AllowAngular");

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

}

app.UseAuthentication(); // Сначала аутентификация
app.UseAuthorization();  // Потом авторизация
app.MapControllers();

app.Run();