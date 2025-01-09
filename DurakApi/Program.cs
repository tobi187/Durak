using DotNetEnv;
using Serilog;
using Serilog.Events;

Env.TraversePath().Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        Env.GetString("log_path"), 
        restrictedToMinimumLevel: LogEventLevel.Information,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

try {
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddSerilog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSignalR();

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
} catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
} finally
{
    Log.CloseAndFlush();
}

