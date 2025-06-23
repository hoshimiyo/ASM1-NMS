using AutoMapper;
using BLL.Interfaces;
using BLL.Services;
using BLL.Utils;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Program).Assembly); // Or Assembly containing your profiles
        });
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            })
            .AddOData(option => option.Select().Filter().Count().OrderBy().Expand()
            .SetMaxTop(100)
            .EnableQueryFeatures()
            .AddRouteComponents("odata", GetEdmModel(),
                services => services.AddSingleton<ODataUriResolver>(
                    sp => new UnqualifiedODataUriResolver() { EnableCaseInsensitive = true })));


        builder.Services.AddDbContext<NewsContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



        // Initialize TokenService with configuration
        TokenService.Initialize(builder.Configuration);

        // JWT token authentication for API endpoints (optional)
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            var secretKey = builder.Configuration["Jwt:Secret"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero,
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers["Authorization"].ToString();
                    Console.WriteLine("Token Received: " + authHeader);
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("JWT auth failed: " + context.Exception.ToString());
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT token successfully validated.");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine("JWT challenge triggered.");
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Staff", policy => policy.RequireRole("1"));
            options.AddPolicy("Lecturer", policy => policy.RequireRole("2"));
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
        builder.Services.AddScoped<JwtUtils>();
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
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NMS-Odata API",
                Version = "v1",
                Description = "API documentation for the NMS",
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = "support@example.com",
                    Url = new Uri("https://example.com")
                }
            });

            // Enable JWT Authentication in Swagger UI
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer <TOKEN>'",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                                });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .WithOrigins("https://localhost:7016")
                    .AllowAnyMethod()
                    .AllowAnyHeader() // This is critical for 'Authorization' header
                    .AllowCredentials()
                    .WithExposedHeaders("Authorization");
            });
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");


        app.UseHttpsRedirection();

        // Use session before authentication
        app.UseSession();  // Make sure session middleware is before authentication
        app.Use(async (context, next) =>
        {
            var token = context.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }
            await next();
        });


        app.UseAuthentication(); // Use authentication after session
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    private static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<Category>("Categories");
        var articles = builder.EntitySet<NewsArticle>("NewsArticles");
        builder.EntitySet<NewsTag>("NewsTags");
        builder.EntitySet<SystemAccount>("SystemAccounts");
        builder.EntitySet<Tag>("Tags");
        return builder.GetEdmModel();
    }
}