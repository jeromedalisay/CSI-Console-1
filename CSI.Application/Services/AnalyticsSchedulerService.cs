using CSI.Application.DTOs;
using CSI.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.Application.Services
{
    public class AnalyticsSchedulerService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public AnalyticsSchedulerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private void ConfigureLogger()
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"log_{DateTime.Now:yyyyMMddHHmmss}.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath)
                .CreateLogger();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ConfigureLogger();
            // Run the task at 6 am every day
            var now = DateTime.Now;
            var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
            Log.Information("Start Task: {0}", now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (now > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            var dueTime = scheduledTime - now;
            //_timer = new Timer(DoWork, null, dueTime, TimeSpan.FromDays(1));
            Task.Run(() => DoWork(null));
            Log.Information("Running DoWork");
            return Task.CompletedTask;
        }


        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var analyticsService = scope.ServiceProvider.GetRequiredService<IAnalyticsService>();
                DateTime prevDate = new DateTime(2023, 12, 15);
                var salesParam = new AnalyticsParamsDto()
                {
                    dates = new List<DateTime> { prevDate, prevDate },
                    memCode = new List<string> {"9999011855", "90999011855", "900999011855", "9999011931", "9999011955",
                                                "9999011915", "9999011914", "9999011926", "9999011838", "9999011929",
                                                "9999011935", "9999011860"},
                    storeId = new List<int> { 221, 201, 210 },
                };
                Log.Information("Adding parameters: {0}", salesParam);
                Log.Information("Param Dates: {0}", salesParam.dates.ToList());
                Log.Information("Param MemCodes: {0}", salesParam.memCode.ToList());
                Log.Information("Param StoreId: {0}", salesParam.storeId.ToList());
                Log.Information("Running SalesAnalytics...");

                // Wrap the asynchronous work in a Task
                var task = Task.Run(() => analyticsService.SalesAnalytics(salesParam).Wait());

                // Wait for the task to complete
                task.Wait();
                StopAsync(default).Wait();
            }

            // Auto close the application
            Environment.Exit(0);
            Thread.Sleep(100);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            _timer?.Dispose();
            Log.Information("Completed Task: {0}", now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
