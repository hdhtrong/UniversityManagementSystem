using MassTransit;
using Serilog;
using NotificationService.API.Consumers;
using NotificationService.API.Extentions;
using NotificationService.API.Config;

var builder = WebApplication.CreateBuilder(args);

// Load EmailSettings từ appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

// CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

// RabbitMQ + MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // ✅ Lấy connection string từ config
        var rabbitConn = builder.Configuration["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost:5672/";

        cfg.Host(new Uri(rabbitConn));

        // 👇 Ràng buộc queue riêng cho Notification
        cfg.ReceiveEndpoint("notifications.email", e =>
        {
            e.ConfigureConsumer<SendEmailConsumer>(context);
        });
    });
});

// Swagger (optional, để test API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/health", () => Results.Ok("NotificationService is healthy!"));

// Consul Registration
await app.RegisterWithConsulAsync(builder.Configuration);

Log.Information("NotificationService is running! Writting log to Elasticsearch...");

app.Run();
