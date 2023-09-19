using ExceptionMiddleware;
using Microsoft.EntityFrameworkCore;
using PatternRepository.Models.DB;
using PatternRepository.Repository.UserRepository;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationContext>();

builder.Services.AddDbContext<ApplicationContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("LocalTestConn"));
});

#region Serilog Settings
builder.Host.UseSerilog((hostBuilder, loggerConfiguration) =>
{
    loggerConfiguration
        .WriteTo.File(builder.Configuration["LoggingLocalPath"]!,
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
            formatProvider: CultureInfo.InvariantCulture)
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
        .MinimumLevel.Information() //Is Correct? A lot of info in Console
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationIdHeader("X-custom-correlationid"); //Serilog with propagation Header
});
#endregion

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var logger = app.Services.GetRequiredService<Serilog.ILogger>();

app.ConfigureCustomExceptionHandler(logger);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
