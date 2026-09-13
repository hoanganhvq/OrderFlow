using System.Text.Json.Serialization;
using BuildingBlocks.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application;
using MyStoreProject.Services.Ordering.Infrastructure;
using MyStoreProject.Services.Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5000") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    await context.Database.MigrateAsync();
}
app.UseRouting();
app.UseCors("Frontend");
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();