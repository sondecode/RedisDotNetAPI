using DemoRedis.Configurations;
using DemoRedis.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var redisConfiguration = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
builder.Services.AddSingleton(redisConfiguration);

if (redisConfiguration.Enabled)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.RedisConnectionString));
    builder.Services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.RedisConnectionString);
    builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
}
    
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
