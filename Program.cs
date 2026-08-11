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
// Skills
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Skills;
using SmartRecruitmentMatchingPlatform.API.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Services.Skills;

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
// Employer & Vacancy
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers;
using SmartRecruitmentMatchingPlatform.API.Services.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Services.Vacancies;

// ======================================
// Applications
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications;
using SmartRecruitmentMatchingPlatform.API.Services.Applications;

// ======================================
// Contact Requests
// ======================================
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Repositories.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Services.ContactRequests;

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


// ======================================
// Swagger
// ======================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Smart Recruitment Matching Platform API",
            Version = "v1"
        });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. " +
                "Example: \"Authorization: Bearer {token}\"",

            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

    c.AddSecurityRequirement(
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
// Job Seeker Repositories & Services
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

builder.Services.AddScoped<
    IJobSeekerProfileService,
    JobSeekerProfileService>();

builder.Services.AddScoped<
    ICVService,
    CVService>();


// ======================================
// Job Seeker Experience
// ======================================
builder.Services.AddScoped<
    IExperienceRepository,
    ExperienceRepository>();

builder.Services.AddScoped<
    IExperienceService,
    ExperienceService>();


// ======================================
// Job Seeker Education
// ======================================
builder.Services.AddScoped<
    IEducationRepository,
    EducationRepository>();

builder.Services.AddScoped<
    IEducationService,
    EducationService>();


// ======================================
// Skills
// ======================================
builder.Services.AddScoped<
    ISkillRepository,
    SkillRepository>();

builder.Services.AddScoped<
    ISkillService,
    SkillService>();

builder.Services.AddScoped<
    IJobSeekerSkillRepository,
    JobSeekerSkillRepository>();

builder.Services.AddScoped<
    IJobSeekerSkillService,
    JobSeekerSkillService>();


// ======================================
// Employer & Vacancy Repositories & Services
// ======================================
builder.Services.AddScoped<
    IEmployerRepository,
    EmployerRepository>();

builder.Services.AddScoped<
    IEmployerService,
    EmployerService>();

builder.Services.AddScoped<
    IVacancyRepository,
    VacancyRepository>();

builder.Services.AddScoped<
    IVacancyService,
    VacancyService>();


// ======================================
// Application Repositories & Services
// ======================================
builder.Services.AddScoped<
    IApplicationRepository,
    ApplicationRepository>();

builder.Services.AddScoped<
    IApplicationService,
    ApplicationService>();


// ======================================
// Contact Request Repositories & Services
// ======================================
builder.Services.AddScoped<
    IContactRequestRepository,
    ContactRequestRepository>();

builder.Services.AddScoped<
    IContactRequestService,
    ContactRequestService>();


// ======================================
// AutoMapper & FluentValidation
// ======================================
builder.Services.AddAutoMapper(
    typeof(JobSeekerMappingProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateJobSeekerProfileValidator>();


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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Recruitment Matching Platform API v1");
    c.RoutePrefix = "swagger";
});

app.UseDefaultFiles();
app.UseStaticFiles();

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