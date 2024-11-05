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
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
//using CoeServiceHub.com.vaps.Services;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Services;
using DomainModel.Model.com.vapstech.TT;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.TT;

namespace LeaveManagementServiceHub
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
          //  var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Integrated Security=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
          //var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
             var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames_2022-05-23T05-38Z; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LeaveManagementServiceHub")));

            services.AddScoped<LMContext>().AddDbContext<LMContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LeaveManagementServiceHub")));

            services.AddScoped<FOContext>().AddDbContext<FOContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LeaveManagementServiceHub")));
            services.AddScoped<TTContext>().AddDbContext<TTContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LeaveManagementServiceHub")));


            // services.AddSingleton<exammastercategoryInterface, exammastercategoryImpl>();
            services.AddScoped<LeaveCreditInterface, LeaveCreditImpl>();
            services.AddScoped<LeaveOpeningBalanceInterface, LeaveOpeningBalanceImpl>();
            services.AddScoped<LeaveTransactionManualInterface, LeaveTransactionManualImpl>();
            services.AddScoped<MasterLeaveInterface, MasterLeaveImpl>();
            services.AddScoped<LeaveReportInterface, LeaveReportImpl>();
            services.AddScoped<OnlineLeaveApplicationInterface, OnlineLeaveApplicationImpl>();
            services.AddScoped<LeaveApprovalInterface, LeaveApprovalImpl>();
            services.AddScoped<LeaveApprovalInterface, LeaveApprovalImpl>();
            services.AddScoped<LeaveAuthorizationInterface, LeaveAuthorizationImpl>();
            services.AddScoped<LeaveTransferInterface, LeaveTransferImpl>();
            services.AddScoped<LeaveConfigInterface, LeaveConfigImpl>();
            services.AddScoped<PlicyInterface, PolicyImpl>();
            services.AddScoped<LeaveStatusReportInterface, LeaveStatusReportImpl>();
            services.AddScoped<LeaveYearlyReportInterface, LeaveYearlyReportImpl>();
            services.AddScoped<AdminondutyapplyInterface, AdminondutyapplyIMPL>();
            services.AddScoped<PeriodWseLeavReportInterface, PeriodwseLeaveReportServices>();

            Mapper.Initialize(config =>
            {
                // config.CreateMap<exammastercategoryDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<HR_Master_GroupType_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Employee_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_Trans_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_Credit_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Grade_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Leave_Authorisation_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Leave_Auth_OrderNo_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_Trans_Details_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_ApplicationDMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_CreditMonth_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_CFMonth_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_CF_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_EnCash_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_Appl_DetailsDMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Emp_OB_Leave_DMO, LeaveCreditDTO>().ReverseMap();
                config.CreateMap<HR_Master_Leave_DMO, MasterLeaveDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Leave_StatusDMO, LeaveCreditDTO>().ReverseMap();

                config.CreateMap<HR_Master_Leave_DMO, MasterLeaveDTO>().ReverseMap();
                config.CreateMap<HR_Master_Employee_DMO, LeaveCreditDTO>().ReverseMap();

                config.CreateMap<HR_Master_Leave_Details_DMO, Grade_TempDto>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_DMO, GroupType_TempDto>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_DMO, Department_TempDto>().ReverseMap();
                config.CreateMap<HR_Master_Leave_Details_DMO, Designation_TempDto>().ReverseMap();
                config.CreateMap<HR_Leave_Policy_Config_DMO, HR_Leave_Policy_Config_DTO>().ReverseMap();

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
            try
            {
                loggerFactory.AddFile("Logs/LeaveManagement-{Date}.txt");
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
