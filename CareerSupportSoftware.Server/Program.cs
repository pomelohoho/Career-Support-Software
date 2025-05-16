using CareerSupportSoftware.Server.Converters;
using CareerSupportSoftware.Server.Data;
using CareerSupportSoftware.Server.Models;
using CareerSupportSoftware.Server.Services;
using CareerSupportSoftwareServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // Remove problematic EventLog provider
builder.Logging.AddConsole();     // Simple console logging
builder.Logging.AddDebug();       // Debug output


// Add DbContext with proper configuration
builder.Services.AddDbContext<JobDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// Configure Identity
builder.Services.AddIdentity<ApiUser, IdentityRole>()
    .AddEntityFrameworkStores<JobDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

// Add after AddIdentity()
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});


// Configure HTTP client
builder.Services.AddHttpClient("RapidAPI", client =>
{
    client.BaseAddress = new Uri("https://linkedin-job-search-api.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", builder.Configuration["RapidAPI:Key"]);
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "linkedin-job-search-api.p.rapidapi.com");
});

// Service registrations
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<ExcelImportService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database initialization with error handling
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<JobDbContext>();
        context.Database.Migrate();
        await context.Database.ExecuteSqlRawAsync(@"
            UPDATE dbo.JobPostings
            SET    IsVisaSponsor = 1
            WHERE  UPPER(Organization) LIKE 'CAPITAL ONE%';
        ");
        Console.WriteLine("Marked Capital One as visa sponsors.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database");
    }
}


using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    var import = scope.ServiceProvider.GetRequiredService<ExcelImportService>();
    var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();

    if (!db.H1BCompanies.Any())
    {
        var filePath = Path.Combine(env.ContentRootPath,
                                    "SeedData",
                                    "LCA_Disclosure_Data_FY2025_Q1.xlsx");

        await using var fs = File.OpenRead(filePath);
        await import.ImportH1BData(fs);

        Console.WriteLine($"Seeded H-1B sponsors from {Path.GetFileName(filePath)}");
    }
}



using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Student" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Create admin user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

    string adminEmail = "admin@careersupport.com";
    string adminPassword = "AdminPassword123!";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApiUser();
        adminUser.UserName = adminEmail;
        adminUser.Email = adminEmail;

        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();