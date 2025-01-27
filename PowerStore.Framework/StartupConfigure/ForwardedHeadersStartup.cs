﻿using PowerStore.Core;
using PowerStore.Core.Configuration;
using PowerStore.Core.Data;
using PowerStore.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PowerStore.Framework.StartupConfigure
{
    public class ForwardedHeadersStartup : IPowerStoreStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="webHostEnvironment">WebHostEnvironment</param>
        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            //check whether database is installed
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var serviceProvider = application.ApplicationServices;
            var hostingConfig = serviceProvider.GetRequiredService<HostingConfig>();

            if (hostingConfig.UseForwardedHeaders)
                application.UsePowerStoreForwardedHeaders();

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order {
            //ForwardedHeadersStartup should be loaded before authentication 
            get { return -20; }
        }
    }
}
