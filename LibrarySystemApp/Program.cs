using Mapster;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LibrarySystemApp.Data;
using LibrarySystemApp.Mappings;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Repositories;
using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.Services.Implementation;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Repositories.Implementation;
using LibrarySystemApp.Services;
using LibrarySystemApp.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Production: Allow App to be externally accessible on port 5000
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Development: Allow App to be externally accessible on port 5001
builder.WebHost.UseUrls("http://0.0.0.0:5001");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Mapster
MappingConfig.RegisterMappings();

builder.Services.AddDbContext<LibraryContext>(opt => 
{
    // opt.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")); // DefaultConnection or AZURE_SQL_CONNECTIONSTRING
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Authorization Services
// builder.Services.AddAuthorization();

// Add Authentication Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
    options.IncludeErrorDetails = true;
    
    var tokenKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:TokenKey"]!);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        ValidateIssuer = true, // Was false, now validating
        ValidateAudience = true, // Was false, now validating
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Match Issuer in TokenService
        ValidAudience = builder.Configuration["Jwt:Audience"], // Match Audience in TokenService
    };

    // Allow JWT tokens from query string for SignalR WebSocket connections
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy =>
//         policy.RequireRole("admin")); // Ensure role claim is mapped
// });

// Add the controllers service
builder.Services.AddControllers();

// Add SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
});

// Configuring AllowAll Policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Allow requests from any domain
            .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
            .AllowAnyHeader(); // Allow all headers
    });
    
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials() // Required for SignalR
            .SetIsOriginAllowed(_ => true); // Allow any origin for SignalR
    });
});

// Adding Scopes and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<INotificationService, CombinedNotificationService>();
builder.Services.AddScoped<NotificationService>(); // Keep the original as a dependency
builder.Services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();
builder.Services.AddHttpClient(); // For Firebase HTTP requests
// Enable automatic notification reminders for due date notifications
builder.Services.AddHostedService<NotificationBackgroundService>();

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// Allowing Access
app.UseCors("SignalRPolicy");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR Hub
app.MapHub<NotificationHub>("/notificationHub");

app.Run();