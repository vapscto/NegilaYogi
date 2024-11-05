using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlacementServiceHub.com.Interfaces;
using PlacementServiceHub.com.Services;

namespace PlacementServiceHub
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

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
            //  var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PlacementServiceHub")
                      .UseRowNumberForPaging()
                       )
                   );
            services.AddDbContext<PlacementContext>(options =>
                    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PlacementServiceHub")
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
             services.AddScoped<PL_Master_CompanyInterface, PL_Master_CompanyIMPL>();

           // services.AddScoped<PL_Master_CompanyInterface, PL_Master_CompanyIMPL>();
            services.AddScoped<PL_Master_Company_ContactInterface, PL_Master_Company_ContactIMPL>();
            services.AddScoped<PL_CI_Schedule_CompanyInterface, PL_CI_Schedule_CompanyIMPL>();
            services.AddScoped<PlacementJobScheduleTitleInterface, PlacementJobScheduleTitleImp>();

            services.AddScoped<PL_CI_StudentStatusInterface, PL_CI_StudentStatusImp>();

            services.AddScoped<PL_CI_Schedule_Company_JobTitleInterface, PL_CI_Schedule_Company_JobTitleImpl>();
            services.AddScoped<PL_CI_Schedule_Company_JobTitle_CriteriaInterface, PL_CI_Schedule_Company_JobTitle_CriteriaImpl>();
            services.AddScoped<PL_CI_Schedule_Company_JobTitle_CourseBranchInterface, PL_CI_Schedule_Company_JobTitle_CourseBranchImpl>();


            services.AddScoped<Master_CISInterface, Master_CISIMPL>();
            services.AddScoped<semmarkInterface, semmarkIMPL>();
            services.AddScoped<mappingInterface, mappingIMPL>();
            services.AddScoped<CISReportInterface, CISReportIMPL>();


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/PlacementServiceHub-{Date}.txt");

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

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.Run(async (context) =>
        //    {
        //        await context.Response.WriteAsync("Hello World!");
        //    });
        //}
    }
}
