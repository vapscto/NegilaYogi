using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PreadmissionDTOs.com.vaps.COE;
using DomainModel.Model.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.College.COE;
using ClgCOEServiceHub.com.vaps.Interfaces;
using ClgCOEServiceHub.com.vaps.Services;
using DomainModel.Model.com.vapstech.College.COE;
using PreadmissionDTOs.com.vaps.College.COE;

namespace ClgCOEServiceHub
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
            // var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            //var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus_2018-04-20T05-24Z;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";

            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";


            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));


            services.AddScoped<ClgCOEContext>().AddDbContext<ClgCOEContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ClgCOEServiceHub")
            )
            );

            services.AddScoped<ClgCOEContext>().AddDbContext<ClgCOEContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ClgCOEServiceHub")
            )
            );


            services.AddScoped<ClgMasterCOEInterface, ClgMasterCOEImpl>();
            services.AddScoped<ClgCOEReportInterface, ClgCOEReportImpl>();

            services.AddScoped<ClgCOEMailSMSInterface, ClgCOEMailSMSImpl>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<COE_Master_EventsDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_EventsDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_CourseBranchDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_EmployeesDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_ImagesDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_OthersDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_VideosDMO, ClgMasterCOEDTO>().ReverseMap();
                config.CreateMap<COEReportDTO, COEReportDTO>().ReverseMap();

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
