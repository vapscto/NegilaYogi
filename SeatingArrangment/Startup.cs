using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.SeatingArrangment;
using DomainModel.Model;
using DomainModel.Model.SeatingArrangment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.SeatingArrangment;
using SeatingArrangment.Interface;
using SeatingArrangment.Services;

namespace SeatingArrangment
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

            var sqlConnectionString = "Data Source=stjameskolkata.database.windows.net,1433;Initial Catalog=Stjames;Persist Security Info=False;User ID=stjameskolkata;Password = Stjames@123; Connection Timeout = 30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1"))); 
            
            services.AddDbContext<SAMasterBuildingContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("SeatingArrangment")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<SA_Exam_TitetableInterface, SA_Exam_TitetableImpl>();
            services.AddScoped<SAMasterBuildingInterface, SAMasterBuildingImpl>();
            services.AddScoped<SAMasterSuperintendent_Interface, SAMasterSuperintendentImpl>();
            services.AddScoped<Exam_Room_DateInterface, Exam_Room_DateImpl>();
            services.AddScoped<SA_ReportInterface, SA_ReportImpl>();

            services.AddScoped<School_Exam_Date_RoomInterface, School_Exam_Date_RoomImpl>();
            services.AddScoped<School_Absent_Student_EntryInterface, School_Absent_Student_EntryImpl>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            Mapper.Initialize(config =>
            {
                config.CreateMap<Exam_SA_BuildingDMO, SAMasterBuildingDTO>().ReverseMap();
                 
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/SeatingArrangement-{Date}.txt");
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
