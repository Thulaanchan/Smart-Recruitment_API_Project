using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using SmartRecruitmentMatchingPlatform.API.Data.Context;

// ======================================
// Authentication
// ======================================
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

// ======================================
// Job Seeker
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Services.JobSeekers;

// ======================================
// Matching
// ======================================
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
// Swagger + JWT Authorization
// ======================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter your JWT access token."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});


// ======================================
// Database
// ======================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});


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
builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();


// ======================================
// Authentication Validators
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
// Job Seeker Repositories
// ======================================
builder.Services.AddScoped<
    IJobSeekerRepository,
    JobSeekerRepository>();

builder.Services.AddScoped<
    IJobSeekerProfileRepository,
    JobSeekerProfileRepository>();

builder.Services.AddScoped<
    ICVRepository,
    CVRepository>();


// ======================================
// Job Seeker Services
// ======================================
builder.Services.AddScoped<
    IJobSeekerProfileService,
    JobSeekerProfileService>();

builder.Services.AddScoped<
    ICVService,
    CVService>();


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
// Build Application
// ======================================
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
// Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();