using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var connString = builder.Configuration.GetConnectionString("QuizDatabase");

builder.Services.AddDbContextPool<QuizContext>(options =>
    options.UseNpgsql(connString,
            npgsql => npgsql.EnableRetryOnFailure())
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

app.Run();
