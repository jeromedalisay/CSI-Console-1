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
                DateTime prevDate = DateTime.Today.AddDays(-1);
                //DateTime prevDate = new DateTime(2024, 4, 4);
                var salesParam = new AnalyticsParamsDto()
                {
                    dates = new List<DateTime> { prevDate, prevDate },
                    memCode = new List<string> {
                        "9999011537", "9999011542", "9999011546", "9999011547", "9999011548",
                        "9999011549", "9999011550", "9999011552", "9999011553", "9999011554",
                        "9999011559", "9999011563", "9999011565", "9999011571", "9999011572",
                        "9999011574", "9999011578", "9999011579", "9999011580", "9999011581",
                        "9999011582", "9999011593", "9999011595", "9999011596", "9999011599",
                        "9999011600", "9999011601", "9999011604", "9999011611", "9999011617",
                        "9999011620", "9999011621", "9999011626", "9999011627", "9999011631",
                        "9999011632", "9999011633", "9999011634", "9999011637", "9999011638",
                        "9999011639", "9999011640", "9999011641", "9999011642", "9999011644",
                        "9999011646", "9999011647", "9999011649", "9999011650", "9999011655",
                        "9999011656", "9999011657", "9999011659", "9999011661", "9999011662",
                        "9999011663", "9999011665", "9999011667", "9999011671", "9999011672",
                        "9999011673", "9999011675", "9999011676", "9999011677", "9999011678",
                        "9999011688", "9999011696", "9999011697", "9999011698", "9999011700",
                        "9999011702", "9999011707", "9999011710", "9999011714", "9999011724",
                        "9999011735", "9999011740", "9999011747", "9999011749", "9999011750",
                        "9999011751", "9999011753", "9999011773", "9999011774", "9999011776",
                        "9999011785", "9999011789", "9999011792", "9999011793", "9999011794",
                        "9999011795", "9999011796", "9999011797", "9999011799", "9999011800",
                        "9999011823", "9999011826", "9999011827", "9999011828", "9999011829",
                        "9999011838", "9999011841", "9999011850", "9999011851", "9999011852",
                        "9999011853", "9999011854", "9999011855", "9999011856", "9999011857",
                        "9999011860", "9999011877", "9999011886", "9999011887", "9999011889",
                        "9999011894", "9999011898", "9999011900", "9999011903", "9999011904",
                        "9999011907", "9999011910", "9999011914", "9999011915", "9999011918",
                        "9999011919", "9999011925", "9999011926", "9999011929", "9999011931",
                        "9999011933", "9999011935", "9999011936", "9999011944", "9999011945",
                        "9999011949", "9999011950", "9999011951", "9999011953", "9999011955",
                        "9999011956", "9999011957", "9999011959", "9999011960", "9999011967",
                        "9999011968", "9999011971", "9999011972", "9999011978", "9999011983",
                        "9999011984", "9999011986", "9999011988", "9999011989", "9999011990",
                        "9999011996", "9999011999", "9999012000", "9999012001", "9999012002",
                        "9999012003", "9999012005", "9999012006", "9999012008", "9999012009",
                        "9999012010", "9999012011", "9999012012", "9999012013", "9999012014",
                        "9999012015", "9999012017", "9999012018", "9999012019", "9999012020",
                        "9999012021", "9999012022", "9999012023", "9999012024", "9999012025",
                        "9999012026", "9999012027", "9999012028", "9999012029", "9999012030",
                        "9999012031", "9999012032", "9999012039", "9999012040", "9999012041",
                        "9999012042", "9999012043", "9999012044", "9999012045", "9999012046",
                        "9999012047"},
                storeId = new List<int> { 201, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 227, 228, 226, 229, 230, 231, 232 },
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
