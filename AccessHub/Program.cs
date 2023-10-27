using System.Text;
using DatabaseManagement;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<ProductRepository>();
        builder.Services.AddScoped<TransactionRepository>();
        builder.Services.AddScoped<CardRepository>();

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthorization();
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

        app.MapControllers();

        app.Run();
    }
}