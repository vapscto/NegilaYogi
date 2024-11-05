using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider.com.vapstech.PDA;
using PDAServiceHub.com.vaps.interfaces;
using PDAServiceHub.com.vaps.services;
using DomainModel.Model.com.vapstech.PDA;
using PreadmissionDTOs.com.vaps.PDA;

namespace PDAServiceHub
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

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMemoryCache();
            // var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];




            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PDAServiceHub")));
            services.AddScoped<PDAContext>().AddDbContext<PDAContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PDAServiceHub")));

            
            // Add framework services.

            services.AddScoped<PDAMasterHeadInterface, PDAMasterHeadImpl>();
            services.AddScoped<PDATransactionInterface, PDATransactionImpl>();
            services.AddScoped<PDARefundInterface, PDARefundImpl>();
            services.AddScoped<PDAReportInterface,PDAReportImpl>();

            services.AddScoped<PDABalanceReportInterface, PDABalanceReportImpl>();
            services.AddScoped<PDAClassSectionReportInterface, PDAClassSectionReportImpl>();
            services.AddScoped<PDADueListReportInterface, PDADueListReportImpl>();
            services.AddScoped<PDAHeadWiseReportInterface, PDAHeadWiseReportImpl>();
            services.AddScoped<PDAMonthEndReportInterface, PDAMonthEndReportImpl>();
            services.AddScoped<PDARefundableReportInterface, PDARefundableReportImpl>();
            services.AddScoped<PDASummaryReportInterface, PDASummaryReportImpl>();
          

            Mapper.Initialize(config =>
            {
                // config.CreateMap<SMSEmailSetting, SmsEmailDTO>().ReverseMap(); // 15/11/2016
                config.CreateMap<PDA_Master_HeadDMO, PdaDTO>().ReverseMap();
                config.CreateMap<PDA_ExpenditureDMO, PdaDTO>().ReverseMap();
                config.CreateMap<PDA_StatusDMO, PDATransactionDTO>().ReverseMap();
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                //options.SerializerSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/PDA-{Date}.txt");

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
