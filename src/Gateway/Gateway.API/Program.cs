using Gateway.API.Extentions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration

    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// CORS setting.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                            policy.WithOrigins(allowedOrigins!) // Angular, Flutter
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials(); // nếu Angular dùng withCredentials
                      });
});
// Enable Gateway basic Authentication if in Production
var enableAuth = builder.Environment.EnvironmentName == "Production";
if (enableAuth)
{
    builder.Services.AddJwtAuthentication(builder.Configuration);
}
// Load ocelot.json
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration)
                .AddConsul(); // use consul as service discovery

// Add SeriLog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Enable basic Authentication
if (enableAuth)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/", () => "Gateway is running!");

await app.UseOcelot();

Log.Information("Gateway is running! Writting log to Elasticsearch...");
app.Run();
