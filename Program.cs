using FluentValidation;
using Microsoft.EntityFrameworkCore;

using SmartRecruitmentMatchingPlatform.API.Data.Context;

using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;

using SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Services.JobSeekers;

using SmartRecruitmentMatchingPlatform.API.Mappings.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Validators.JobSeekers;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Job Seeker Repositories
builder.Services.AddScoped<
    IJobSeekerRepository,
    JobSeekerRepository>();

builder.Services.AddScoped<
    IJobSeekerProfileRepository,
    JobSeekerProfileRepository>();

builder.Services.AddScoped<
    ICVRepository,
    CVRepository>();

// Job Seeker Services
builder.Services.AddScoped<
    IJobSeekerProfileService,
    JobSeekerProfileService>();

builder.Services.AddScoped<
    ICVService,
    CVService>();

// AutoMapper
builder.Services.AddAutoMapper(
    typeof(JobSeekerMappingProfile).Assembly);

// FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<
    CreateJobSeekerProfileValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();