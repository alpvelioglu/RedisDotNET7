using Microsoft.EntityFrameworkCore;
using RedisDotNET7.Models;
using RedisDotNET7.Redis;
using RedisDotNET7.Repository;
using RedisDotNET7.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseInMemoryDatabase("myDatabase");
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepo>(sp =>
{
    var appDbContext = sp.GetRequiredService<AppDBContext>();
    var productRepository = new ProductRepo(appDbContext);
    var redisService = sp.GetRequiredService<RedisService>();

    return new ProductRepoWithCacheDecorator(productRepository, redisService);
});

builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    dbContext.Database.EnsureCreated();
}

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
