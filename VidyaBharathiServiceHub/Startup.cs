using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VidyaBharathiServiceHub.com.vaps.Interfaces;
using VidyaBharathiServiceHub.com.vaps.Services;

namespace VidyaBharathiServiceHub
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
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
          
           services.AddApplicationInsightsTelemetry(Configuration);
            
            services.AddSession();
               var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
          //  var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("VidyaBharathiServiceHub")
                      .UseRowNumberForPaging()
                       )
                   );
            services.AddDbContext<VidyaBharathiContext>(options =>
                    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("VidyaBharathiServiceHub")
                   .UseRowNumberForPaging()
                    )
                );

            services.AddScoped<OrganisationContext>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
            services.AddScoped<IVRM_User_Login_StateInterface, IVRM_User_Login_StateIMPL>();
            services.AddScoped<VBSC_MasterCompetition_CategoryInterface, VBSC_MasterCompetition_CategoryIMPL>();
            services.AddScoped<VBSC_Master_EventsInterface, VBSC_Master_EventsIMPL>();
            //added
         
            services.AddScoped<IVRM_User_Login_DistrictInterface, IVRM_User_Login_DistrictIMPL>();
            services.AddScoped<VBSC_Events_CategoryInterface, VBSC_Events_CategoryIMPL>();
            services.AddScoped<VBSC_Master_UOMInterface, VBSC_Master_UOMIMPL>();            services.AddScoped<VBSC_Master_SportsCCName_UOMInterface, VBSC_Master_SportsCCName_UOMImpl>();       
            services.AddScoped<VBSC_Master_SportsCCGroupNameInterface, VBSC_Master_SportsCCGroupNameIMPL>();
            services.AddScoped<VBSC_Master_SportsCCNameInterface, VBSC_Master_SportsCCNameIMPL>();
            services.AddScoped<VBadminInterface, VBadminInterIMPL>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/VidyaBharathiServiceHub-{Date}.txt");

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
