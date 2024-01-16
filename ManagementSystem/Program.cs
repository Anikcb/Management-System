using ManagementSystem;
using ManagementSystem.Models;
using ManagementSystem.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StudentStoreDatabaseSettings>(
    builder.Configuration.GetSection("StudentDatabase"));
builder.Services.AddSingleton<StudentService>();

builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddSingleton<RedisService>();

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
