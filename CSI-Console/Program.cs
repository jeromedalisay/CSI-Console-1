using CSI.Application.Services;
using CSI.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using CSI.Application.Interfaces;
using Serilog;

public partial class Program
{
    static void Main(string[] args)
    {
        // Configure TLS version
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddScoped(provider => new AppConfig(configuration))
            .AddScoped<IAnalyticsService, AnalyticsService>()
            .AddScoped<AnalyticsSchedulerService>()
            .AddDbContext<AppDBContext>((provider, options) =>
            {
                var appConfig = provider.GetRequiredService<AppConfig>();
                options.UseSqlServer(appConfig.ConnectionString);
            })
            .BuildServiceProvider();

        try
        {
            var analyticsSchedulerService = serviceProvider.GetRequiredService<AnalyticsSchedulerService>();
            analyticsSchedulerService.StartAsync(default).Wait();
            Console.ReadKey();
            analyticsSchedulerService.StopAsync(default).Wait();
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred: {0}", ex);
        }
    }
}