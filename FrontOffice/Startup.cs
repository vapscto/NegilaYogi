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
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using FrontOfficeHub.com.vaps.Interfaces;
using FrontOfficeHub.com.vaps.Services;
using AutoMapper;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using Microsoft.AspNetCore.Http;
using FrontOffice.com.vaps.Interfaces;
using FrontOffice.com.vaps.Services;

namespace FrontOffice
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
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=hutchingsserver.database.windows.net,1433;Initial Catalog=Hutchings;User ID=hutchingsadmin;Password=Hutchpune@123;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //  var sqlConnectionString = "Data Source=chikkatti.database.windows.net,1433;Initial Catalog=chikkatti;Persist Security Info=False;User ID=chikkatti;Password=vaps@123;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddScoped<FOContext>().AddDbContext<FOContext>(options =>
   options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("FrontOfficeHub")



   )
   );

            services.AddScoped<HRMSContext>().AddDbContext<HRMSContext>(options =>
                        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("FrontOfficeHub")));



            services.AddScoped<ShiftSettingMasterInterface, ShiftSettingMasterImpl>();

            services.AddScoped<MasterTimeSettingInterface, MasterTimeSettingImpl>();
            services.AddScoped<EmployeeShiftMappingInterface, EmployeeShiftMappingImpl>();
            services.AddScoped<EmployeeInOutReportInterface, EmployeeInOutReportImpl>();
            services.AddScoped<MasterHolidayInterface, MasterHolidayImpl>();
            services.AddScoped<HolidayReportInterface, HolidayReportImpl>();
            services.AddScoped<Employee_Add_logs_ManualInterface, Employee_Add_logs_ManualImpl>();
            services.AddScoped<StudentInOutReportInterface, StudentInOutReportImp>();

            services.AddScoped<BiometricInterface, BiometricImpl>();
            //services.AddScoped<HolidayReportInterface, HolidayReportImpl>();
            services.AddScoped<EmployeeLogImportInterface, EmployeeLogImportImpl>();

            services.AddScoped<EmployeeLogReportInterface, EmployeeLogReportImpl>();
            services.AddScoped<EmployeeYearlyReportInterface, EmployeeYearlyReportImpl>();
            services.AddScoped<EmployeeMonthlyReportInterface, EmployeeMonthlyReportImpl>();
            services.AddScoped<EmployeeLateInEarlyOutReportInterface, EmployeeLateInEarlyOutReportImpl>();
            services.AddScoped<FrontOfficeMonthEndReportInterface, FrontOfficeMonthEndReportImpl>();

            Mapper.Initialize(config =>
            {
                // config.CreateMap<exammastercategoryDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<MasterShiftsTimingsDMO, MasterShiftsTimingsDTO>().ReverseMap();
                config.CreateMap<MasterShiftsDMO, MasterShiftsTimingsDTO>().ReverseMap();

                config.CreateMap<FODayNameDMO, FODayNameDTO>().ReverseMap();

                config.CreateMap<MasterTimeSettingDTO, MasterTimeSettingDMO>().ReverseMap();
                config.CreateMap<EmployeeShiftMappingDTO, EmployeeShiftMappingDMO>().ReverseMap();

                config.CreateMap<FO_Emp_PunchDTO, FO_Emp_PunchDMO>().ReverseMap();
                config.CreateMap<FO_Emp_Punch_DetailsDTO, FO_Emp_Punch_DetailsDMO>().ReverseMap();

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
            loggerFactory.AddFile("Logs/FrontOfficeHub-{Date}.txt");

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
