using System.Reflection;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using VeloSpace.Context;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
// {
//     options.ConnectionString = builder.Configuration["AzureMonitor:ConnectionString"]; 
// });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VeloSpace API",
        Version = "v1",
        Description = "API RESTful da solução VeloSpace."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Digite o token JWT no formato: Bearer {seu token}"
//     });
//
//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             Array.Empty<string>()
//         }
//     });
});


builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: connectionString, 
        name: "oracle-database", 
        failureStatus: HealthStatus.Degraded,
        tags: new[]{"db", "oracle", "sql"},
        timeout: TimeSpan.FromSeconds(10)
    )
    .AddCheck(
        "self",
        () => HealthCheckResult.Healthy("Application is running"),
        tags: new[] { "api", "self" }
    );

builder.Services.AddHealthChecksUI(options =>
    {
        options.SetEvaluationTimeInSeconds(15);
        options.MaximumHistoryEntriesPerEndpoint(50);
        options.SetApiMaxActiveRequests(1);
        options.AddHealthCheckEndpoint("Health Check General", "/health");
        options.AddHealthCheckEndpoint("Health Check Application", "/health/application");
        options.AddHealthCheckEndpoint("Health Check Database", "/health/database");
    })
    .AddInMemoryStorage();

// ==============================
// DbContext
// ==============================
builder.Services.AddDbContext<VeloSpaceContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===============
// Authentication
// ===============
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
//             )
//         };
//     });

// ==============================
// CORS configurado
// ==============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// builder.Services.AddAuthorization();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/application", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("self"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/database", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VeloSpace API v1");
    c.RoutePrefix = "swagger";
});

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (ex != null || httpContext.Response.StatusCode >= 500)
            return Serilog.Events.LogEventLevel.Error;

        if (httpContext.Response.StatusCode >= 400)
            return Serilog.Events.LogEventLevel.Warning;

        return Serilog.Events.LogEventLevel.Information;
    };
});

// app.UseAuthentication();
// app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();