using Microsoft.EntityFrameworkCore;
using ShortUrlPJ.Config;
using ShortUrlPJ.Contexts;
using ShortUrlPJ.Filters;
using ShortUrlPJ.Services;
using ShortUrlPJ.Services.Interfaces;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ShortUrlDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")
                            ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseWrapperFilter>();
});
builder.Services.AddSwaggerGen();
CorsConfig.AddCorsPolicies(builder.Services, builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseCors(CorsConfig.DefaultPolicy);
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ShortUrlDbContext>();
        db.Database.Migrate();
        Console.WriteLine("Success");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error:{ex.Message}");
    }
}
app.Run();
