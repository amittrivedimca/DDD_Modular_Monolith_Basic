
using Scalar.AspNetCore;
using Catalog.Application;
using Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Catalog Module
builder.Services.AddCatalogApplication();
builder.Services.AddCatalogInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Run with url http://localhost:{port}/scalar/
    app.MapScalarApiReference(options =>
    {
        options.Title = "Shopping Cart System v1";
    });
}

app.UseAuthorization();


app.MapControllers();

app.Run();
