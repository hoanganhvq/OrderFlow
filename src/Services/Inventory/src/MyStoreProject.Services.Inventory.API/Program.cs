using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Inventory.Application;
using MyStoreProject.Services.Inventory.Infrastructure;
using MyStoreProject.Services.Inventory.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    await context.Database.MigrateAsync();
}

app.UseRouting();
app.UseCors("Frontend");
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();