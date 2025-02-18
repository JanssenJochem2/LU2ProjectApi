using projectapi.Webapi.Interfaces;
using projectapi.Webapi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["SqlConnectionString"];

// Add services to the container.
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(o => new Object2DRepository(connectionString));
//Hoe word dit gefixt?

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();