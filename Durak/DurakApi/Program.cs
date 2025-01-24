using Serilog;
using DotNetEnv;
using DurakApi.Db;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;

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
    
    var connectionString = Env.GetString("connection_string");
    
    // Add services to the container.
    builder.Services.AddSerilog();
    builder.Services.AddDbContext<ApplicationDbContext>(
        opts => opts.UseSqlite(connectionString));
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSignalR();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        // DbInitializer.Initialize(context);
    }

    app.UseHttpsRedirection();

    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

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

