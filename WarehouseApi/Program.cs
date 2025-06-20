using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Database context using SQL Server
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication service
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication setup
var jwtConfig = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer           = true,
            ValidIssuer              = jwtConfig["Issuer"],
            ValidateAudience         = true,
            ValidAudience            = jwtConfig["Audience"],
            ValidateLifetime         = true
        };
    });

// Add controllers
builder.Services.AddControllers();

// Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Warehouse API",
        Version     = "v1",
        Description = "Warehouse management API endpoints"
    });
});

var app = builder.Build();

// Seed initial users and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    // Seed users if none exist
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User
            {
                Username     = "test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("test"),
                Role         = "Employee"
            }
        );
        context.SaveChanges();
    }
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
