using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using CollegePortals.com.Student.Interfaces;
using CollegePortals.com.Student.Services;
using CollegePortals.com.Staff.Interfaces;
using CollegePortals.com.Staff.Services;
using CollegePortals.com.Chairman.Interfaces;
using CollegePortals.com.Chairman.Services;
using CollegePortals.com.vaps.Student.Interfaces;
using CollegePortals.com.vaps.Student.Services;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using CollegePortals.com.IVRM.Interfaces;
using CollegePortals.com.IVRM.Services;
using CollegePortals.com.vaps.IVRM.Interfaces;
using CollegePortals.com.vaps.IVRM.Services;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using PortalHub.com.vaps.MobileApp.Services;
using PortalHub.com.vaps.MobileApp.Interfaces;

namespace CollegePortals
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=CollegeTest;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //  var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            // var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<CollegeportalContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegePortalHub")));
            services.AddDbContext<CollFeeGroupContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegePortalHub")));
            services.AddDbContext<HRMSContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegePortalHub")));
            services.AddDbContext<FOContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegePortalHub")));
            services.AddDbContext<ExamContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegePortalHub")));
            services.AddDbContext<PortalContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PortalHub")));
            services.AddScoped<TTContext>().AddDbContext<TTContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("TimeTableServiceHub")));

            //============================Student Portal
            services.AddScoped<ClgStudentDashboardInterface, ClgStudentDashboardImpl>();
            services.AddScoped<ClgAttendanceDetailsInterface, ClgAttendanceDetailsImpl>();
            services.AddScoped<ClgFeeDetailsInterface, ClgFeeDetailsImpl>();
            services.AddScoped<ClgCOEInterface, ClgCOEImpl>();
            services.AddScoped<ClgStudentFeedbackFormInterface, ClgStudentFeedbackFormImpl>();
            services.AddScoped<ClgFeeReceiptInterface, ClgFeeReceiptImpl>();
            services.AddScoped<ClgExamReportInterface, ClgExamReportImpl>();
            services.AddScoped<CollegeStudent_TTInterface, CollegeStudent_TTImpl>();
            //============================Staff Portal
            services.AddScoped<ClgStaffDashboardInterface, ClgStaffDashboardImpl>();
            services.AddScoped<ClgSalaryDetailsInterface, ClgSalaryDetailsImpl>();
            services.AddScoped<ClgStudentAttendanceInterface, ClgStudentAttendanceImpl>();
            services.AddScoped<ClgStudentSearchInterface, ClgStudentSearchImpl>();
            services.AddScoped<ClgEmployeePunchAttendenceInterface, ClgEmployeePunchAttendenceImpl>();
            services.AddScoped<ClgLiveMeetingScheduleInterface, ClgLiveMeetingScheduleImpl>();
            //============================ Portal IVRM
            services.AddScoped<ClgNoticeBoardInterface, ClgNoticeBoardImpl>();
            services.AddScoped<ClgPushNotificationInterface, ClgPushNotificationImpl>();
            services.AddScoped<Clg_IVRM_InteractionsInterface, Clg_IVRM_InteractionsImpl>();
            services.AddScoped<Clg_IVRM_GalleryInterface, Clg_IVRM_GalleryImpl>();

            //===================== college Class Details =====================
            services.AddScoped<Clg_ClassDetailsInterface, Clg_ClassDetailsImpl>();

            //============================ Portal Chairman
            services.AddScoped<clgChairmanDashboardInterface, clgChairmanDashboardImpl>();
            services.AddScoped<CLGCHStudentStrengthInterface, CLGCHStudentStrengthImpl>();
            services.AddScoped<CLGCasteWiseStudentStrengthInterface, CLGCasteWiseStudentStrengthImpl>();
            services.AddScoped<CLGGRPHeadFeeDetailsInterface, CLGGRPHeadFeeDetailsImpl>();
            services.AddScoped<CLGCHOverAllFeeInterface, CLGCHOverAllFeeImpl>();
            services.AddScoped<CHHRMSEmpSalaryInterface, CHHRMSEmpSalaryImpl>();
            services.AddScoped<CHHRMSEmpDetailsInterface, CHHRMSEmpDetailsImpl>();
            services.AddScoped<CLGCHSubjectDetailsInterface, CLGCHSubjectDetailsImpl>();

            //=========================== HOD

            services.AddScoped<ClgHODDashboardInterface, ClgHODDashboardImpl>();
            services.AddScoped<Clg_HODEmpSalaryInterface, Clg_HODEmpSalaryImpl>();
            services.AddScoped<Clg_HODCasteWiseStudentStrengthInterface, Clg_HODCasteWiseStudentStrengthImpl>();

            //======================= College Principal ==========

            services.AddScoped<ClgPrincipalDashboardInterface, ClgPrincipalDashboardImpl>();


            //======================= College Principal =========

            //Mobile App Impl
            services.AddScoped<MobileInterface, MobileImpl>();
            //=============END

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
            loggerFactory.AddFile("Logs/CollegePortalHub-{Date}.txt");

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
