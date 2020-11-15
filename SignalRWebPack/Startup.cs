using AspDotNetCore.RestFull.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SignalRWebPack.Data;
using SignalRWebPack.Data.EntityRepositories;
using SignalRWebPack.Hubs;
using System;

namespace SignalRWebPack
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            string connectionString = PrvGetConnectionStringFromAppSettingsJson(Configuration);

            services.AddScoped<EfContext>();
            services.AddDbContextPool<EfContext>(opt =>
            {
                opt.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("SignalRWebPack"));
            });

            services.AddTransient<UserRepository>();
            services.AddTransient<Seed>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hub");
            });
        }

        private string PrvGetConnectionStringFromAppSettingsJson(IConfiguration configuration)
        {
            var connectionStringConfiguration = PrvGetConfigurationFromAppSettingsJson<ConnectionStringConfiguration>(configuration);
            return connectionStringConfiguration.Default;
        }

        private T PrvGetConfigurationFromAppSettingsJson<T>(IConfiguration configuration)
            where T : class, new()
        {
            T configModel = new T();

            var configSection = new ConfigureFromConfigurationOptions<T>(
                configuration.GetSection(typeof(T).Name));

            configSection.Configure(configModel);

            return configModel;
        }
    }
}
