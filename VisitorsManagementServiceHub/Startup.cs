using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VisitorsManagementServiceHub.Interfaces;
using VisitorsManagementServiceHub.Services;
using AutoMapper;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using DomainModel.Model;

namespace VisitorsManagementServiceHub
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


            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;"; 

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("VisitorsManagementServiceHub")));

            services.AddScoped<VisitorsManagementContext>().AddDbContext<VisitorsManagementContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("VisitorsManagementServiceHub")));

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>();

            // Add framework services.


            services.AddScoped<AddVisitorsInterface, AddVisitorsImpl>();
            services.AddScoped<AppointmentStatusInterface, AppointmentStatusImpl>();
            services.AddScoped<GetVisitorReportInterface, GetVisitorReportImpl>();
            services.AddScoped<InwardInterface, InwardImpl>();
            services.AddScoped<OutwardInterface, OutwardImpl>();
            services.AddScoped<InwardOutwardReportInterface, InwardOutwardReportImpl>();
            services.AddScoped<GatePassInterface, GatePassImpl>();
            services.AddScoped<GatePassReportInterface, GatePassReportImpl>();

            //aman
            services.AddScoped<MasterLocationInterface, MasterLocationImpl>();
            services.AddScoped<VisitorAppointmentInterface, VisitorAppointmentImpl>();
            services.AddScoped<StudentGatePassInterface, StudentGatePassImpl>();
            services.AddScoped<StaffGatePassInterface, StaffGatePassImpl>();
            services.AddScoped<MonthEndReportInterface, MonthEndReportImpl>();
            services.AddScoped<LateInStudentInterface, LateInStudentImpl>();
            services.AddScoped<StudentLateInReportInterface, StudentLateInReportImpl>();
            services.AddScoped<V_AppointmentApprovalStatusInterface, V_AppointmentApprovalStatusImpl>();
            services.AddScoped<V_AppointmentApprovalReportInterface, V_AppointmentApprovalReportImpl>();


            Mapper.Initialize(config =>
            {
                //config.CreateMap<AddVisitorsDMO, AddVisitorsDTO>().ReverseMap();
                config.CreateMap<InwardDMO, InwardDTO>().ReverseMap();
                config.CreateMap<OutwardDMO, OutwardDTO>().ReverseMap();
                config.CreateMap<GatePassDMO, GatePassDTO>().ReverseMap();

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
            // loggerFactory.AddFile("Logs/Sports-{Date}.txt");

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
