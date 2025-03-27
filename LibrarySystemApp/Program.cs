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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Allow App to be externally accessible on port 5000
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Mapster
MappingConfig.RegisterMappings();

// Add the controllers service
builder.Services.AddControllers();

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
});

builder.Services.AddAuthorization();
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy =>
//         policy.RequireRole("admin")); // Ensure role claim is mapped
// });

// Add the controllers service
builder.Services.AddControllers();

// Configuring AllowAll Policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Allow requests from any domain
            .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
            .AllowAnyHeader(); // Allow all headers
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

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// Allowing Access
app.UseCors("AllowAll");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();