using DatabaseManagement;
using DatabaseManagement.Interfaces;
using DatabaseManagement.Repositories;
using CSA.Entities;
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
        services.AddRepositoriesToServiceCollection();

        Provider = services.BuildServiceProvider();
        
        foreach (var service in services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            Provider.GetRequiredService(service.ServiceType);
    }

    public static T Resolve<T>() where T : notnull
        => Provider.GetRequiredService<T>();

    private static IServiceCollection AddRepositoriesToServiceCollection(this IServiceCollection services)
    {
        return services.AddScoped<IRepository<Card>, CardRepository>()
            .AddScoped<IRepository<Product>, ProductRepository>()
            .AddScoped<IRepository<Transaction>, TransactionRepository>()
            .AddScoped<IRepository<User>, UserRepository>();
    }
}