using BLL.Interfaces;
using BLL.Services;
using DAL.Data;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using DAL.Interfaces;
using DAL.Repositories;
using BLL.Utils;

var builder = WebApplication.CreateBuilder(args);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly); // Or Assembly containing your profiles
});
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<NewsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Initialize TokenService with configuration
TokenService.Initialize(builder.Configuration);

// JWT token authentication for API endpoints (optional)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
})
.AddJwtBearer(options =>
{
    var secretKey = builder.Configuration["Jwt:Secret"];
    options.TokenValidationParameters.RoleClaimType = "role";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = "role",
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Staff", policy => policy.RequireRole("1", "3"));
    options.AddPolicy("Lecturer", policy => policy.RequireRole("2", "1", "3"));
    options.AddPolicy("Admin", policy => policy.RequireRole("3"));
});



// Register services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<INewsTagService, NewsTagService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<UserUtils>();
builder.Services.AddScoped<PasswordUtils>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddDistributedMemoryCache();

// Add session services
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;  // Ensure that the cookie is accessible via HTTP only (increased security)
    options.Cookie.IsEssential = true;  // Make the cookie essential for the application
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use session before authentication
app.UseSession();  // Make sure session middleware is before authentication

// Middleware to add the JWT token to the request header (for APIs)
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["JwtToken"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseAuthentication(); // Use authentication after session
app.UseAuthorization();

app.MapControllers();

app.Run();
