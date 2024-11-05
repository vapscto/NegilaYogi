using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Birthday;
using ClgBirthdayServiceHub.com.vaps.Interfaces;
using ClgBirthdayServiceHub.com.vaps.Services;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;

namespace ClgBirthdayServiceHub
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            //var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus_2018-04-20T05-24Z;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";

            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //var sqlConnectionString = "Data Source = bdcampus.database.windows.net,1433; Initial Catalog = bdcampus; Persist Security Info = False;   User ID = baldwincampus; Password = b@Ldw!nDig!tal; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

            //var sqlConnectionString = "Data Source=demovaps.database.windows.net,1433;Initial Catalog=TestingDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;";


            services.AddScoped<ClgBirthdayContext>().AddDbContext<ClgBirthdayContext>(options =>
    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ClgBirthdayServiceHub")
    )
    );
            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeServiceHub")
)
);


            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);
            //====================================College Birthday Interface and Impl
            services.AddScoped<ClgBirthdayInterface, ClgBirthdayImpl>();
            services.AddScoped<BdayMonthEndReportInterface, BdayMonthEndReportImpl>();

            Mapper.Initialize(config =>
            {

            });

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
