using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetInsights.Shared.Services;
using DotNetInsights.Shared.Library.Extensions;
using System.Reflection;
using AutoMapper;
using DotNetInsights.Shared.Services.Middleware;
using System.Collections.Generic;

namespace DotNetInsights.Shared.WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .RegisterServiceBroker<AppQueueServiceBroker>(ServiceLifetime.Scoped)
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMvc(options => options.Filters.Add<HandleModelStateErrorFilter>());

            foreach (var service in services.Where(a => a.ServiceType.FullName.Contains("IEventHandler", StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("{2}|{0}:{1}", service.ServiceType.FullName, service.ImplementationType.FullName, service.Lifetime);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers();
            });
        }
    }

    public class AppQueueServiceBroker : DefaultServiceBroker
    {
        public override IEnumerable<Assembly> GetAssemblies => new [] { 
            DefaultAssembly, 
            Assembly.GetAssembly(typeof(AppQueueServiceBroker)) };
    }
}
