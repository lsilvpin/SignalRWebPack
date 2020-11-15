using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Data;
using System.Data.Common;
using System.Linq;

namespace SignalRWebPack
{
    public class Program
    {
        protected Program() { }
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            PrvSeedDataBase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void PrvSeedDataBase(IHost host)
        {
            IServiceScopeFactory scopedFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope scope = scopedFactory.CreateScope();
            EfContext db = scope.ServiceProvider.GetRequiredService<EfContext>();
            Seed seed = scope.ServiceProvider.GetRequiredService<Seed>();

            try
            {
                if (!db.Users.Any())
                {
                    seed.Execute();
                }                
            }
            catch (DbException dbException)
            {
                ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(dbException, "A database seeding error occurred.");
            }
        }
    }
}
