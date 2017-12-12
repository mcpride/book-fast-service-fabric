using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BookFast.Framework;
using BookFast.Security.AspNetCore;
using System.Fabric;

namespace BookFast.Facility
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, StatelessServiceContext serviceContext)
        {
            var builder = new ConfigurationBuilder()
                .AddServiceFabricConfiguration(serviceContext);

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var modules = new List<ICompositionModule>
                          {
                              new Composition.CompositionModule(),
                              new Security.AspNetCore.Composition.CompositionModule(),
                              new Business.Composition.CompositionModule(),
                              new Data.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, Configuration);
            }

            services.AddApplicationInsightsTelemetry(Configuration);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseAuthentication();

            app.UseSecurityContext();
            app.UseMvc();

            app.UseSwagger();
        }
    }
}
