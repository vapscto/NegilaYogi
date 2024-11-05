using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using AlumniHub.Com.Interface;
using AlumniHub.Com.Service;
using DomainModel.Model.com.vapstech.Alumni;
using PreadmissionDTOs.com.vaps.Alumni;

namespace AluminiHub
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

            var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";


            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));


            services.AddDbContext<AlumniContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AlumniHub")
)
);
            services.AddScoped<Alumni_Interactions_Interface, Alumni_Interactions_Impl>();
            services.AddScoped<Alumni_NoticeBoard_Interface, Alumni_NoticeBoard_Impl>();
            services.AddScoped<Alumni_Gallery_Interface, Alumni_Gallery_Impl>();
            services.AddScoped<AlumniMembershipInterface, AlumniMembershipImpl>();
           

            services.AddScoped<Alumni_Student_FriendRequestInterface, Alumni_Student_FriendRequestImpl>();
            services.AddScoped<CLGAlumniApprovalInterface, CLGAlumniApprovalImpl>();
            services.AddScoped<AlumniDonationInterface, AlumniDonationImpl>();
            services.AddScoped<CLGAlumnistudentsearchInterface, CLGAlumnistudentsearchipImpl>();
            services.AddScoped<ALUDASHInterface, ALUDASHImpl>();
            services.AddScoped<SchoolALUDASHInterface, SchoolALUDASHImpl>();
            services.AddScoped<AlumniSearchInterface, AlumniSearchImpl>();
            services.AddScoped<AlumnilettersInterface, AlumnilettersImpl>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<AlumniStudentDTO, Alumni_M_StudentDMO>().ReverseMap();
                config.CreateMap<CLGAlumniStudentDTO, CLGAlumni_M_StudentDMO>().ReverseMap();
            });

          

            services.AddIdentity<ApplicationUser, ApplicationRole>()
         .AddEntityFrameworkStores<ApplicationDBContext>()
         .AddDefaultTokenProviders();

            



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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiforgery)
        {
            loggerFactory.AddFile("Logs/AluminiHub-{Date}.txt");

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
