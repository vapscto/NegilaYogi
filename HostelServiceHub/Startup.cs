using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using HostelServiceHub.Interface;
using HostelServiceHub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HostelServiceHub
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

            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=College;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
          
            services.AddScoped<HostelContext>().AddDbContext<HostelContext>(options => options.UseSqlServer(sqlConnectionString,
                b => b.MigrationsAssembly("HostelServiceHub")));

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HostelServiceHub")));

            services.AddScoped<AdmissionFormContext>().AddDbContext<AdmissionFormContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HostelServiceHub")));

            services.AddScoped<HRMSContext>().AddDbContext<HRMSContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HostelServiceHub")));
          
            services.AddScoped<MasterHostelInterface, MasterHostelImpl>();
            services.AddScoped<HS_Master_Interface, HS_Master_IMPL>();
            services.AddScoped<Room_TariffInterface, Room_TariffImpl>();
            services.AddScoped<StudentRequestConfirmInterface, StudentRequestConfirmImpl>();
            services.AddScoped<StaffRequestConfirmInterface, StaffRequestConfirmImpl>();
            services.AddScoped<HostelAllotForStudentInterface, HostelAllotForStudentImpl>(); 
            services.AddScoped<HostelAllotForCLGStudentInterface, HostelAllotForCLGStudentIMPL>(); 
            services.AddScoped<HostelAllotForStaffInterface, HostelAllotForStaffImpl>();
            services.AddScoped<HostelAllotForGuestInterface, HostelAllotForGuestImpl>();
            services.AddScoped<StudentRequestInterface, StudentRequestImpl>();
            services.AddScoped<StaffRequestInterface, StaffRequestImpl>();
            services.AddScoped<Hostel_Request_ReportInterface, Hostel_Request_ReportImpl>();
            services.AddScoped<Hostel_Allotment_ReportInterface, Hostel_Allotment_ReportImpl>();
            services.AddScoped<StudentVacantInterface, StudentVacantImpl>();
            services.AddScoped<HostelVacateReportInterface, HostelVacateReportImpl>();
            services.AddScoped<CLGStudentRequestInterface, CLGStudentRequestImpl>();
            services.AddScoped<CLGStudentRequestConfirmInterface, CLGStudentRequestConfirmIMPL>();
            services.AddScoped<CLGStudentRequestReportInterface, CLGStudentRequestReportIMPL>();
            services.AddScoped<CLGHostelVacantInterface, CLGHostelVacantIMPL>();
            services.AddScoped<HostelAllotAndConfermForCLGStudentInterface, HostelAllotAndConfermForCLGStudentIMPL>();
            services.AddScoped<CLGHostelVacateReportInterface, CLGHostelVacateReportIMPL>();
            services.AddScoped<MasterMess_MessCategoryInterface, MasterMess_MessCategoryIMPL>();
            
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
            loggerFactory.AddFile("Logs/Hostel-{Date}.txt");
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
