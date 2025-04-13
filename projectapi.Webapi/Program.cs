using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projectapi.Webapi.Interfaces;
using projectapi.Webapi.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["SqlConnectionString"];

builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        //options.SignIn.RequireConfirmedPhoneNumber = true;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 10;
    })
    .AddDapperStores(options =>
    {
        options.ConnectionString = connectionString;
    });

builder.Services
    .AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
    .Configure(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
    });


builder.Services.AddTransient<IObjectRepository, ObjectRepository>(o => new ObjectRepository(connectionString));
//builder.Services.AddScoped<IAuthenticationService, AspNetIdentityAuthenticationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/", () => "Hello world, the API is up");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//authorization endpoint
app.MapGroup("/auth")
    .MapIdentityApi<IdentityUser>();

//logout endpoint
app.MapPost("/auth/logout",
    async (SignInManager<IdentityUser> SignInManager,
    [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await SignInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .RequireAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();