using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using BdTracker.Back.Data;
using BdTracker.Back.Middlewares;
using BdTracker.Back.Services;
using BdTracker.Back.Services.Interfaces;
using BdTracker.Back.Settings;
using BdTracker.Back.Validators;
using BdTracker.Shared.Entities;

using Serilog;
using Microsoft.OpenApi.Models;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    // Read from your appsettings.json.
    .AddJsonFile("appsettings.json")
    // Read from your secrets.
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithExceptionDetails()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("Logs/log.txt")
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

var connString = builder.Configuration.GetConnectionString("ConnectionName").Replace("~", builder.Environment.ContentRootPath);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connString);
});

//builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // wtf is this for?
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<CompanyOwnerValidator>();
builder.Services.AddTransient<RegisterOwnerRequestValidator>();
builder.Services.AddTransient<AddUserRequestValidator>();
builder.Services.AddTransient<UpdateUserRequestValidator>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.SectionName));
builder.Services.AddSingleton<IAuthSettings>(x => x.GetRequiredService<IOptions<AuthSettings>>().Value);

builder.Services.Configure<SecretAccessSettings>(configuration.GetSection(SecretAccessSettings.SectionName));
builder.Services.AddSingleton<ISecretAccessSettings>(x => x.GetRequiredService<IOptions<SecretAccessSettings>>().Value);

builder.Services.Configure<IConnectionStringSettings>(configuration.GetSection(ConnectionStringSettings.SectionName));
builder.Services.AddSingleton<IConnectionStringSettings>(x => x.GetRequiredService<IOptions<ConnectionStringSettings>>().Value);

var app = builder.Build();

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHealthChecks("/health");
app.UseMiddleware<ExceptionMiddleware>();
app.Run();