using deposito_do_pitty.Application.Validators;
using DepositoDoPitty.Application.Interfaces;
using DepositoDoPitty.Application.Services;
using DepositoDoPitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using DepositoDoPitty.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ?? Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ?? FluentValidation (versão 11+)
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

// ?? Controllers
builder.Services.AddControllers();

// ?? DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ?? Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// ?? Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
