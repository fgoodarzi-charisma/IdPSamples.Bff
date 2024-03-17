using Api;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://auth-stage.charisma.digital";
        options.Audience = "smpl__weather_api";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://auth-stage.charisma.digital",
            ValidAudience = "smpl__weather_api",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(5),
        };
    });

builder.Services.AddAuthentication("token")
    .AddOAuth2Introspection("token", options =>
    {
        options.Authority = "https://auth-stage.charisma.digital";
        options.ClientId = "smpl__weather_api";
        options.ClientSecret = "smpl__weather_api_secret";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiCaller", policy =>
    {
        policy.RequireClaim("scope", "smpl__weather");
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/todos")
    .ToDoGroup()
    .RequireAuthorization("ApiCaller");

app.Run();

