using System.Text;
using KanaTomo.API.APIAnki;
using KanaTomo.API.APIAuth;
using KanaTomo.API.APITranslation;
using KanaTomo.API.APIUser;
using KanaTomo.Helper;
using KanaTomo.Models.User;
using KanaTomo.Web.Controllers.Auth;
using KanaTomo.Web.Repositories;
using KanaTomo.Web.Repositories.Anki;
using KanaTomo.Web.Repositories.Auth;
using KanaTomo.Web.Repositories.Translation;
using KanaTomo.Web.Repositories.User;
using KanaTomo.Web.Services.Anki;
using KanaTomo.Web.Services.Translation;
using KanaTomo.Web.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5070");
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80);
    });
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KanaTomo API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Register HttpClient
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();


// Register TranslationService
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<ITranslationRepository, TranslationRepository>();

builder.Services.AddHttpClient<IApiTranslationRepository, ApiTranslationRepository>();
builder.Services.AddScoped<IApiTranslationService, ApiTranslationService>();

builder.Services.AddScoped<IApiUserRepository, ApiUserRepository>();
builder.Services.AddScoped<IApiUserService, ApiUserService>();

builder.Services.AddScoped<IApiAuthRepository, ApiAuthRepository>();
builder.Services.AddScoped<IApiAuthService, ApiAuthService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IApiAnkiService, ApiAnkiService>();
builder.Services.AddScoped<IApiAnkiRepository, ApiAnkiRepository>();

builder.Services.AddScoped<IAnkiService, AnkiService>();
builder.Services.AddScoped<IAnkiRepository, AnkiRepository>();


// Add configuration
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    EnvReader.Load(".env");
}
builder.Configuration.AddEnvironmentVariables();

// Add database context
var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured in environment variables.");
}
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 1, 0)));
});

// Add JWT authentication
var jwtSecret = Environment.GetEnvironmentVariable("jwtSecret");
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JWT Secret is not configured in environment variables.");
}

var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanaTomo API v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();