using TaskFlow.Api.Services;
using Microsoft.EntityFrameworkCore; // El motor de la base de datos
using TaskFlow.Api.Data;
using Scalar.AspNetCore;             // Donde guardaste tu ApplicationDbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Registramos el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // Le decimos que use SQL Server y que busque la "dirección" (ConnectionString)
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference(); // Esto creará una página visual
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
