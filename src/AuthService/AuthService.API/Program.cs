using AuthService.Domain.Entities;
using AuthService.Infrastructure;
using AuthService.API.Extentions;
using AuthService.API.Security.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Serilog;
using MassTransit;
using SharedKernel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// CORS setting.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

// MassTransit + RabbitMQ
var rabbitConnStr = builder.Configuration["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost:5672/";
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(rabbitConnStr));
    });
});
builder.Services.AddSingleton(new RabbitMqHealthChecker(rabbitConnStr));

// Add authentication and Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

// add Identity and database for Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
    })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddSingleton<IAuthorizationHandler, CredentialRequirementPolicyAuthorizationHandler>();
builder.Services.AddDatabase(builder.Configuration, builder.Environment);
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configure API versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
}).AddMvc().AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV"; // v1, v1.1, v2
    opt.SubstituteApiVersionInUrl = true;
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.AssumeDefaultVersionWhenUnspecified = true;
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Configure AutoMapper
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));

// Add SeriLog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }
    });
    // Allow anonymous access to Swagger in dev mode
    app.Use(async (context, next) =>
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/swagger"))
        {
            context.User = new System.Security.Claims.ClaimsPrincipal();
        }
        await next();
    });
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("AuthService is healthy!"));

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.MapControllers();

// Config Consul
await app.RegisterWithConsulAsync(builder.Configuration);

Log.Information("AuthService is running! Writting log to Elasticsearch...");

app.Run();