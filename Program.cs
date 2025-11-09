using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortfolioAdmin.Api.Data;
using PortfolioAdmin.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Health Checks
builder.Services.AddHealthChecks();

// Database
builder.Services.AddSingleton<DatabaseContext>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IPortfolioContentRepository, PortfolioContentRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// JWT Authentication
// Prefer explicit environment variable (JWT_SECRET_KEY) when deployed on platforms like Render.
// Fall back to configuration file value (appsettings) or a built-in development key.
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
             ?? builder.Configuration["Jwt:SecretKey"]
             ?? "your-super-secret-jwt-key-for-portfolio-admin-system-2024-with-extra-security-padding";

// Validate key length early so we surface a clear error instead of the cryptic IDX10720 later.
var keyBytes = Encoding.UTF8.GetBytes(jwtKey ?? string.Empty);
if (keyBytes.Length < 32)
{
    // 32 bytes == 256 bits required by HmacSha256
    Console.Error.WriteLine($"FATAL: JWT secret key is too short ({keyBytes.Length} bytes). It must be at least 32 bytes (256 bits).\nSet environment variable JWT_SECRET_KEY with a sufficiently long secret.");
    throw new InvalidOperationException("JWT secret too short. Set JWT_SECRET_KEY to at least 32 characters (256 bits).");
}

var key = keyBytes;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "PortfolioAdmin.Api",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "PortfolioAdmin.Client",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// CORS - Allow frontend domains
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allow all necessary domains for both development and production
        policy.WithOrigins(
                // Production domains
                "https://admin-kapil.netlify.app",        // Your admin dashboard
                "https://kapilkaushal.netlify.app",       // Your portfolio website
                "https://portfolio-kapil.netlify.app",    // Alternative portfolio URL
                "https://kapil-admin.netlify.app",        // Alternative admin URL
                
                // Development domains
                "http://localhost:3000",                  // Next.js admin dashboard
                "https://localhost:3000",
                "http://localhost:5173",                  // Vite/React portfolio
                "https://localhost:5173",
                "http://localhost:3001",                  // Alternative ports
                "http://localhost:5174",
                
                // Render preview domains (if you use them)
                "https://admin-kapil.onrender.com",
                "https://portfolio-kapil.onrender.com"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Portfolio Admin API", 
        Version = "v1",
        Description = "API for managing portfolio content including projects, experience, and skills"
    });
    
    // Add JWT Authentication support in Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await databaseContext.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio Admin API v1");
        c.RoutePrefix = "swagger"; // Swagger UI will be available at /swagger
    });
}
else
{
    // Enable Swagger in production for API documentation
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio Admin API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Portfolio Admin API Documentation";
    });
}

// Only use HTTPS redirection in development - Render handles HTTPS
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowFrontend");

// Enable serving static files from uploads directory
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoints (BEFORE MapControllers)
app.MapHealthChecks("/health");
app.MapHealthChecks("/api/health");

// Add a root endpoint
app.MapGet("/", () => Results.Redirect("/swagger"));

// Add CORS test endpoint
app.MapGet("/api/cors-test", () => new { 
    message = "CORS is working!", 
    timestamp = DateTime.UtcNow,
    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
}).WithOpenApi();

app.MapControllers();

app.Run();

// Make Program class accessible for testing
public partial class Program { }
