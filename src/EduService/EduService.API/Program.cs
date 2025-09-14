using EduService.API.Extentions;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using EduService.API.Security.Handlers;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// CORS setting.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // policy.WithOrigins("http://example.com", "https://localhost");
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

// Add authentication and Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();// Add Customized Authorization Policies

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
builder.Services.AddSwaggerGen(c =>
{
    // 👇 Cho phép đọc XML comments (từ <summary> trong code)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
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

app.MapGet("/health", () => Results.Ok("EduService is healthy!"));

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

// Consul Registration
await app.RegisterWithConsulAsync(builder.Configuration);

app.Run();
