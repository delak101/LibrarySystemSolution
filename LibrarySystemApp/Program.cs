using Mapster;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LibrarySystemApp.Data;
using LibrarySystemApp.Mappings;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.Services.Implementation;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Repositories.Implementation;

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
builder.Services.AddAuthorization();

// Add Authentication Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("token key is missing");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// TODO: Authorization for students and admin
// builder.Services.AddAuthorization(options => 
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "admin"));
//     options.AddPolicy("StudentOnly", policy => policy.RequireClaim("role", "student"));
// });

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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();

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