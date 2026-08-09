using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;
using SmartRecruitmentMatchingPlatform.API.Matching.Engine;
using SmartRecruitmentMatchingPlatform.API.Matching.Filtering;
using SmartRecruitmentMatchingPlatform.API.Matching.Ranking;
using SmartRecruitmentMatchingPlatform.API.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Services.Matching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==============================
// Matching Module
// ==============================
builder.Services.AddScoped<IMatchingRepository, MatchingRepository>();
builder.Services.AddScoped<IMatchingService, MatchingService>();

builder.Services.AddScoped<MatchingEngine>();
builder.Services.AddScoped<CandidateRanker>();
builder.Services.AddScoped<JobFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();