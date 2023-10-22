using DatabaseManagement;
using DatabaseManagement.Entities;
using DatabaseManagement.Interfaces;
using DatabaseManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server;

public static class IoC
{
    private static readonly IServiceProvider Provider;
    
    static IoC()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);
        
        var configuration = builder.Build();
        var services = new ServiceCollection();

        services.AddDbContext<DatabaseManagementContext>(s
            => s.UseNpgsql(configuration.GetConnectionString("postgresql")));

        Provider = services.BuildServiceProvider();
        
        foreach (var service in services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            Provider.GetRequiredService(service.ServiceType);
    }

    public static T Resolve<T>() where T : notnull
        => Provider.GetRequiredService<T>();

    private static IServiceCollection AddRepositoriesToServiceCollection(this IServiceCollection services)
    {
        return services.AddScoped<IRepository<Card>, Repository<Card>>()
            .AddScoped<IRepository<Product>, Repository<Product>>()
            .AddScoped<IRepository<Transaction>, Repository<Transaction>>()
            .AddScoped<IRepository<User>, Repository<User>>();
    }
}