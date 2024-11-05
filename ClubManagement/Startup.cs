using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using ClubManagement.Interfaces;
using ClubManagement.Services;

namespace ClubManagement
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

            services.AddSession();
           var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";       
          //  var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ClubManagement")
                      .UseRowNumberForPaging()
                       )
                   );
            services.AddDbContext<ClubManagementContext>(options =>
                      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ClubManagement")
                     .UseRowNumberForPaging()
                      )
                  );
            //add framework services club management
            //services.AddScoped<CMS_MasterDepartmentInerface, CMS_MasterDepartmentIMPL>();

            services.AddScoped<OrganisationContext>();

            services.AddMvc().AddJsonOptions(options =>
            {                
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
            //services.AddSingleton<Test,TestImpl>();

            services.AddScoped<CMS_MasterDepartmentInerface, CMS_MasterDepartmentIMPL>();
            services.AddScoped<CMS_Master_InstallmentTypeINTERFACE, CMS_Master_InstallmentTypeIMPL>();
            services.AddScoped<CMS_Master_InstallmentInterface, CMS_Master_InstallmentsIMPL>();
            services.AddScoped<CMS_MemberCategoryInterface, CMS_MemberCategoryIMPL>();
            services.AddScoped<CMS_MembershipApplicationInterface, CMS_MembershipApplicationIMPL>();
            services.AddScoped<CMS_TransactionInterface, CMS_TransactionIMPL>();
            services.AddScoped<CMS_MasterMemberInerface, CMS_MasterMemberIMPL>();
            services.AddScoped<CMS_Master_MemberBlockedInterface, CMS_Master_MemberBlockedIMPL>();
            services.AddScoped<CMS_Member_StatusInterface, CMS_Member_StatusIMPL>();
            services.AddScoped<CMS_TrasanctionTypeInterface, CMS_TrasanctionTypeimpl>();
            //CMS_Member_StatusDMO
            Mapper.Initialize(config =>
            {              


            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/Clubmanagement-{Date}.txt");

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
