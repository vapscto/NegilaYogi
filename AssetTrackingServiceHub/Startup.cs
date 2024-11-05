using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using AssetTrackingServiceHub.com.vaps.Implementation;
using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using AssetTrackingServiceHub.com.vaps.AssetTag.Implementation;

namespace AssetTrackingServiceHub
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<AssetTrackingContext>().AddDbContext<AssetTrackingContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AssetTrackingServiceHub")
         )
         );
            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDBContext>()
             .AddDefaultTokenProviders();

            //================ ASSETS TRACKING INTERFACE / IMPLEMENTION
            services.AddScoped<Assets_Expiredate_Interface, Assets_Expiredate_IMPL>();
            services.AddScoped<AT_MasterSiteInterface, AT_MasterSiteImpl>();
            services.AddScoped<AT_MasterLocationInterface, AT_MasterLocationImpl>();
            services.AddScoped<CheckOutAssetsInterface, CheckOutAssetsImpl>();
            services.AddScoped<CheckInAssetsInterface, CheckInAssetsImpl>();
            services.AddScoped<DisposeAssetsInterface, DisposeAssetsImpl>();
            services.AddScoped<TransferAssetsInterface, TransferAssetsImpl>();
            //================================= ASSETS TRACKING REPORT
            services.AddScoped<SiteLocationsReportInterface, SiteLocationsReportImpl>();
            services.AddScoped<AssetsReportInterface, AssetsReportImpl>();
            services.AddScoped<CheckoutReportInterface, CheckoutReportImpl>();
            services.AddScoped<CheckInReportInterface, CheckInReportImpl>();
            services.AddScoped<DisposeReportInterface, DisposeReportImpl>();

            //================================= ASSETS TAG
            services.AddScoped<AssetTagInterface, AssetTagImpl>();
            services.AddScoped<AssetTagCheckOutInterface, AssetTagCheckOutImpl>();
            services.AddScoped<AssetTagCheckInInterface, AssetTagCheckInImpl>();
            services.AddScoped<AssetTagDisposeInterface, AssetTagDisposeImpl>();
            services.AddScoped<AssetTagTransferInterface, AssetTagTransferImpl>();
            //================================= ASSETS TAG Report
            services.AddScoped<AssetTag_ReportInterface, AssetTag_ReportImpl>();
            services.AddScoped<AssetTagCheckout_ReportInterface, AssetTagCheckout_ReportImpl>();
            services.AddScoped<AssetTagCheckIn_ReportInterface, AssetTagCheckIn_ReportImpl>();
            services.AddScoped<AssetTagDispose_ReportInterface, AssetTagDispose_ReportImpl>();
            services.AddScoped<AssetTagTransfer_ReportInterface, AssetTagTransfer_ReportImpl>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddFile("Logs/AssetTrackingServiceHub-{Date}.txt");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.Use((context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    return context.Response.WriteAsync("handled");
                }
                return next.Invoke();
            });
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
