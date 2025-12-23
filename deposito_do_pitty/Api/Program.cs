using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Application.Services;
using deposito_do_pitty.Application.Validators;
using deposito_do_pitty.Domain.Interfaces;
using deposito_do_pitty.Infrastructure.Repositories;
using DepositoDoPitty.Application.Interfaces;
using DepositoDoPitty.Application.Services;
using DepositoDoPitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using DepositoDoPitty.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection ausente nas configurações.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IAccountsPayableRepository, AccountsPayableRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IAccountsPayableService, AccountsPayableService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("Jwt:Key ausente nas configurações.");
if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new InvalidOperationException("Jwt:Issuer ausente nas configurações.");
if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("Jwt:Audience ausente nas configurações.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cole APENAS o token (sem a palavra Bearer)"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


app.UseRouting();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();


var swaggerEnabled = app.Configuration.GetValue<bool>("Swagger:Enabled");

if (app.Environment.IsDevelopment() || swaggerEnabled)
{
 
    app.MapSwagger().AllowAnonymous();

   
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Deposito do Pitty API")
               .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
               .AddDocument("v1");
    })
    .AllowAnonymous();

    
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();