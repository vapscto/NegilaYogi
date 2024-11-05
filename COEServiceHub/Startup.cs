using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CoeServiceHub.com.vaps.Interfaces;
using CoeServiceHub.com.vaps.Services;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PreadmissionDTOs.com.vaps.COE;
using DomainModel.Model.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using COEServiceHub.com.vaps.Interfaces;
using COEServiceHub.com.vaps.Services;
using DataAccessMsSqlServerProvider.com.vapstech.College.COE;
using DomainModel.Model.com.vapstech.College.COE;
using PreadmissionDTOs.com.vaps.College.COE;

namespace COEServiceHub
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

            //var sqlConnectionString = "Data Source=demovaps.database.windows.net,1433;Initial Catalog=IVRMInhouse;Persist Security Info=False;User ID=demovaps;Password=vaps@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;";
            //var sqlConnectionString = "Data Source=172.16.32.21;Initial Catalog=Vapsecampus;Integrated Security=False;User ID=Vaps;Password=Vts@321;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=vidyabharathi.database.windows.net,1433;Initial Catalog=VidyaBharathi;Persist Security Info=False;User ID=vidyabharathi;Password=vaps@123;Connection Timeout=30;";
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=tcp:jesussacredheart.database.windows.net,1433;Initial Catalog=JSHS;Persist Security Info=False;User ID = jesussacredheart; Password = jesus@321;Connection Timeout = 30; ";
            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddScoped<COEContext>().AddDbContext<COEContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CoeServiceHub")));
            services.AddScoped<FOContext>().AddDbContext<FOContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CoeServiceHub")));


            services.AddScoped<ClgCOEContext>().AddDbContext<ClgCOEContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CoeServiceHub")));


            //  services.AddSingleton<MasterCOEInterface, MasterCOEImpl>();
            services.AddScoped<MasterCOEInterface, MasterCOEImpl>();
            services.AddScoped<COEReportInterface, COEReportImpl>();
            services.AddScoped<COEMailSMSInterface, COEMailSMSImpl>();
            services.AddScoped<COEInterface, COEImpl>();
            services.AddScoped<CoeReportGraphInterface, CoeReportGraphIMPL>();
            services.AddScoped<ClgCOEMasterInterface, ClgCOEMasterImpl>();
            services.AddScoped<ClgReportCOEInterface, ClgReportCOEImpl>();
            services.AddScoped<COEClgMailSMSInterface, COEClgMailSMSImpl>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<COE_Master_EventsDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_EventsDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_ClassesDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_EmployeesDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_ImagesDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_OthersDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COE_Events_VideosDMO, MasterCOEDTO>().ReverseMap();
                config.CreateMap<COEReportDTO, COEReportDTO>().ReverseMap();

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
            loggerFactory.AddFile("Logs/COE-{Date}.txt");
            loggerFactory.AddConsole(Configuration.GetSection("Logging")); loggerFactory.AddConsole(Configuration.GetSection("Logging"));

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
