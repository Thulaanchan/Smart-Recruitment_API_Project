using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.Configurations;
using SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Interfaces.Services.Auth;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;
using SmartRecruitmentMatchingPlatform.Models.Entities.Users;
using SmartRecruitmentMatchingPlatform.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Services.Auth;
using SmartRecruitmentMatchingPlatform.Services.Users;
using SmartRecruitmentMatchingPlatform.Validators.Auth;

using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ======================================
// Controllers
// ======================================
builder.Services.AddControllers();


// ======================================
// Database - TEMPORARY
// Actual SQL Server configuration will
// be added after merging all modules.
// ======================================
builder.Services.AddDbContext<ApplicationDbContext>();


// ======================================
// JWT Settings
// ======================================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(
        JwtSettings.SectionName));

var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "JWT settings are missing.");

if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException(
        "JWT Key is missing.");
}


// ======================================
// JWT Authentication
// ======================================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key)),

                ClockSkew = TimeSpan.Zero
            };
    });


// ======================================
// Authorization
// ======================================
builder.Services.AddAuthorization();


// ======================================
// Repositories
// ======================================
builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IRefreshTokenRepository,
    RefreshTokenRepository>();


// ======================================
// Services
// ======================================
builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IJwtService,
    JwtService>();

builder.Services.AddScoped<
    IUserService,
    UserService>();


// ======================================
// Password Hashing
// ======================================
builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();


// ======================================
// FluentValidation
// ======================================
builder.Services.AddScoped<
    IValidator<RegisterRequestDto>,
    RegisterValidator>();

builder.Services.AddScoped<
    IValidator<LoginRequestDto>,
    LoginValidator>();

builder.Services.AddScoped<
    IValidator<ChangePasswordDto>,
    ChangePasswordValidator>();


// ======================================
// Swagger
// ======================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// ======================================
// HTTP Request Pipeline
// ======================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Authentication MUST be before Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();