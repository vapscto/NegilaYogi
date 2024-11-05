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
using DataAccessMsSqlServerProvider.com.vapstech.Birthday;
using Microsoft.EntityFrameworkCore;
using BirthdayServiceHub.com.vaps.Interfaces;
using BirthdayServiceHub.com.vaps.Services;
using PreadmissionDTOs.com.vaps.BirthDay;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.BirthDay;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.College.Birthday;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;

namespace BirthdayServiceHub
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
            //var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var sqlConnectionString = "Data Source = stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<BirthdayContext>().AddDbContext<BirthdayContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("BirthdayServiceHub")));

            services.AddScoped<FOContext>().AddDbContext<FOContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("BirthdayServiceHub")));

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddScoped<ClgBirthdayContext>().AddDbContext<ClgBirthdayContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("BirthdayServiceHub")));
            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("BirthdayServiceHub")));

            services.AddScoped<BirthDayInterface, BirthDayImpl>();
            services.AddScoped<BirthDayGraphsInterface, BirthDayGraphsIMPL>();
            services.AddScoped<BirthdayClgInterface, BirthdayClgImpl>();
            services.AddScoped<ClgBdayMonthEndReportInterface, ClgBdayMonthEndReportImpl>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<BirthDayDMO, BirthDayDTO>().ReverseMap();
                // config.CreateMap<exammastercategoryDMO, exammastercategoryDTO>().ReverseMap();
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
            loggerFactory.AddFile("Logs/Birthday-{Date}.txt");

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
