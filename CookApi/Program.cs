using System.Text.Json.Serialization;
using ApiKeyAuthentication.Middlewares;
using AutoMapper;
using CookApi.DTOs;
using CookApi.Models;
using CookApi.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

var corsPolicyName = "AllowFrontFrontEndCookweb";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
                      policy =>
                      {
                          policy.WithOrigins(isDevelopment ? ["http://localhost:5173", "http://localhost:5174"] : ["https://cook-web-weathered-thunder-7639.fly.dev/"]);
                      });
});

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Services
var conn = builder.Configuration.GetConnectionString(builder.Environment.IsEnvironment("Docker") ? "Docker" : "DefaultConnection");

builder.Services.AddDbContext<CookApi.Data.CookApiDbContext>(options =>
    options.UseNpgsql(conn));
builder.Services.AddScoped<RecipesService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cook API",
        Description = "An ASP.NET Core Web API for managing cooking recipes.",
        Contact = new OpenApiContact
        {
            Name = "Luz Ojeda",
            Email = "luzojeda@proton.me"
        },
    });
});

if (isDevelopment)
{
    builder.Services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.RequestMethod |
                          HttpLoggingFields.RequestPath |
                          HttpLoggingFields.RequestQuery |
                          HttpLoggingFields.RequestBody |
                          HttpLoggingFields.Duration |
                          HttpLoggingFields.ResponseStatusCode |
                          HttpLoggingFields.ResponseBody;
        });
}

// Automapper
var configuration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

if (app.Environment.IsProduction())
{
    app.UseMiddleware<ApiKeyMiddleware>();
}

if (isDevelopment)
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();

// For testing
public partial class Program { }