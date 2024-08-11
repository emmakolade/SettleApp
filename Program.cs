using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Settle_App.Data;
using Settle_App.Helpers;
using Settle_App.Models.Domain;
using Settle_App.Repositories;
using Settle_App.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Nz walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Autorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement

    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },
                Scheme ="Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

});
// Add Identity services with custom user class
builder.Services.AddIdentity<SettleAppUser, IdentityRole>()
    .AddEntityFrameworkStores<SettleAppDBContext>()
    .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add DbContext
// builder.Services.AddDbContext<SettleAuthDBContext>(authDBContextOptions =>authDBContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnectionString")));
builder.Services.AddDbContext<SettleAppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SettleAppDBConnectionString")));


// Add Scoped Repository
builder.Services.AddScoped<IJWTRepository, JWTRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<InterswitchService>();
builder.Services.AddScoped<InterswitchAuthService>();
// Add Singleton Helpers
builder.Services.AddSingleton<StringHelper>();


// var cfgSection = builder.Configuration.GetSection("SecretSection");
// var cfgSection = builder.Configuration.GetConnectionString("SecretSection");
// builder.Services.Configure<MySecretSection>(cfgSection);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

static async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = ["SettleAppAdmin", "SettleAppUser"];

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

