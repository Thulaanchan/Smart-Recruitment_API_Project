using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using SmartRecruitmentMatchingPlatform.API.Data.Context;

// ======================================
// Job Seeker
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Mappings.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Validators.JobSeekers;

// ======================================
// Authentication
// ======================================
using SmartRecruitmentMatchingPlatform.Configurations;
using SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Interfaces.Services.Auth;
using SmartRecruitmentMatchingPlatform.Models.Entities.Users;
using SmartRecruitmentMatchingPlatform.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Services.Auth;
using SmartRecruitmentMatchingPlatform.Services.Users;

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

// ======================================
// Admin + Notifications
// ======================================
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Repositories.Implementations;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.Implementations;

using System.Text;


var builder = WebApplication.CreateBuilder(args);


// ======================================
// Controllers
// ======================================
builder.Services.AddControllers();


// ======================================
// Swagger
// ======================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
// Authentication
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
// AutoMapper
// ======================================
builder.Services.AddAutoMapper(
    typeof(JobSeekerMappingProfile).Assembly);


// ======================================
// FluentValidation
// ======================================
builder.Services.AddValidatorsFromAssemblyContaining<
    CreateJobSeekerProfileValidator>();



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
// Admin Module
// ======================================
builder.Services.AddScoped<
    IAdminRepository,
    AdminRepository>();

builder.Services.AddScoped<
    IAdminService,
    AdminService>();


// ======================================
// Notification Module
// ======================================
builder.Services.AddScoped<
    INotificationRepository,
    NotificationRepository>();

builder.Services.AddScoped<
    INotificationService,
    NotificationService>();


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


// ======================================
// Authentication / Authorization
// ======================================
app.UseAuthentication();

app.UseAuthorization();


// ======================================
// Controllers
// ======================================
app.MapControllers();


app.Run();