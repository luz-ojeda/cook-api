using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var isDevelopment = builder.Environment.IsDevelopment();
var conn = builder.Configuration.GetConnectionString(isDevelopment ? "Development" : "");

builder.Services.AddDbContext<CookApi.Data.CookApiDbContext>(options =>
    options.UseNpgsql(conn));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// For testing
public partial class Program { }