using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataAccessMsSqlServerProvider;
using DomainModel;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Localization;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using CommonServiceHub.Interfaces;
using CommonServiceHub.Services;
using DomainModel.Model;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.HRMS;

using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;

namespace CommonServiceHub
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);


            // Use a PostgreSQL database
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            services.AddSession();

            //var sqlConnectionString = "Data Source=VAPS-PC;Initial Catalog=Preadmission24Jan;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=300;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            // var sqlConnectionString = "Data Source=VAPS-PC;Initial Catalog=SaaSPreadmission;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=3000;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //  var sqlConnectionString = "Data Source=VAPS-PC;Initial Catalog=Preadmission2018;Integrated Security=False;User ID=sa;Password=vts@123;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                   options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
                   )
               );
            services.AddDbContext<FeeGroupContext>(options =>
               options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
               )
           );
            services.AddDbContext<COEContext>(options =>
               options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
               )
           );
            services.AddDbContext<ExamContext>(options =>
              options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
              )
          );
            services.AddDbContext<TTContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
            )
        );

            services.AddDbContext<HRMSContext>(options =>
           options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
           )
       );
            services.AddDbContext<StudentAttendanceReportContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
          )
      );
            services.AddDbContext<FOContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
        )
    );
            services.AddDbContext<PortalContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
        )
    );
            services.AddDbContext<LMContext>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CommonServiceHub")
      )
  );
            services.AddScoped<DomainModelMsSqlServerContext>();
            services.AddScoped<FeeGroupContext>();
            services.AddScoped<COEContext>();
            services.AddScoped<ExamContext>();
            services.AddScoped<TTContext>();
            services.AddScoped<HRMSContext>();
            services.AddScoped<StudentAttendanceReportContext>();
            services.AddScoped<FOContext>();
            services.AddScoped<PortalContext>();
            services.AddScoped<LMContext>();
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

             services.AddScoped<LoginMinterface, LoginMImpl>();

            Mapper.Initialize(config =>
            {
                

                //config.CreateMap<PaymentDetails, Prospectus>().ReverseMap();
                //config.CreateMap<Prospepaymentamount, Prospectus>().ReverseMap();

            });

            // services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/hub-{Date}.txt");
            loggerFactory.AddDebug();
            app.UseDeveloperExceptionPage();
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
           // app.UseIdentity();
            app.UseMvc();
        }
    }
}
