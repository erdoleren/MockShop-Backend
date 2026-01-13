using Microsoft.EntityFrameworkCore;
using MockShop.Application.Interfaces;
using MockShop.BackgroundWorker;
using MockShop.Infrastructure.Persistance;
using MockShop.Infrastructure.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // DB and Repository registrations
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IOrderRepository, OrderRepository>();

        // Bizim i?çi s?n?f?m?z
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
