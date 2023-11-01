using DevInsightForge.Application;
using DevInsightForge.Infrastructure;
using DevInsightForge.WebAPI;
using DevInsightForge.WebAPI.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog to container
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebAPIServices(builder.Configuration);

// Initialize app from builder
var app = builder.Build();

// Configure App Pipelines
app.UseAppExceptionHandler();
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run application
app.Run();
