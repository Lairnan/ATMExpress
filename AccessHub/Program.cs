using System.Text;
using AccessHub.BehaviorsFiles;
using CSA.Entities;
using DatabaseManagement;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AccessHub;

internal static class Program
{
    public static ConfigurationManager Configuration { get; private set; } = null!;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Configuration = builder.Configuration;
        var connectionString = Configuration.GetSection("ConnectionString");
        var postgresql = connectionString.GetSection("postgresql").Value;

// Add services to the container.

        builder.Services.AddDbContext<DatabaseManagementContext>(s => s.UseNpgsql(postgresql));
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
        builder.Services.AddScoped<IRepository<Transaction>, TransactionRepository>();
        builder.Services.AddScoped<IRepository<Card>, CardRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = Configuration["AppSettings:issuer"],
                    ValidateAudience = false,
                    ValidAudience = Configuration["AppSettings:audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:secret"]!)),
                    ValidateIssuerSigningKey = true,
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddControllers(o =>
        {
            o.Filters.Add<AllowAnonymousFilter>();
            o.Filters.Add<UserTokenAuthorizationFilter>();
        });

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHeaderMiddleware();

        app.MapControllers();

        app.Run();
    }
}