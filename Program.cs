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

using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;
using SmartRecruitmentMatchingPlatform.API.Matching.Engine;
using SmartRecruitmentMatchingPlatform.API.Matching.Filtering;
using SmartRecruitmentMatchingPlatform.API.Matching.Ranking;
using SmartRecruitmentMatchingPlatform.API.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Services.Matching;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================
// Controllers
// ======================================
builder.Services.AddControllers();

// ======================================
// Database - TEMPORARY
// Real SQL Server options will be added
// after the shared database is finalized.
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
// Authentication Repositories
// ======================================
builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IRefreshTokenRepository,
    RefreshTokenRepository>();

// ======================================
// Authentication Services
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
// Keep the exact PasswordHasher registration
// used by the Authentication branch here.

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
    IValidator<ChangePasswordRequestDto>,
    ChangePasswordValidator>();

// ======================================
// Matching Module
// ======================================
builder.Services.AddScoped<
    IMatchingRepository,
    MatchingRepository>();

builder.Services.AddScoped<
    IMatchingService,
    MatchingService>();

builder.Services.AddScoped<MatchingEngine>();
builder.Services.AddScoped<CandidateRanker>();
builder.Services.AddScoped<JobFilter>();

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

// IMPORTANT:
// Authentication must come before Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();