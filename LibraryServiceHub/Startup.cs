using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using LibraryServiceHub.com.vaps.Services;
using LibraryServiceHub.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.AspNetCore.Builder;
using AutoMapper;
using Microsoft.Extensions.Logging;


namespace LibraryServiceHub
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            //var path = Environment.GetEnvironmentVariable("Cons");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMemoryCache();


            //var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus_2018-04-20T05-24Z;Persist Security Info=False;" + "User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";

            // var sqlConnectionString = "Data Source=demovaps.database.windows.net,1433;Initial Catalog=TestingDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source = tcp:vikasa.database.windows.net,1433; Initial Catalog = VIKASASCHOOL; Persist Security Info = False; User ID = Vikasa;  Password = V!kasa@321; Connection Timeout = 30;";

            //var sqlConnectionString = "Data Source = tcp:vikasa.database.windows.net,1433; Initial Catalog = VIKASASCHOOL; Persist Security Info = False; User ID = Vikasa;  Password = V!kasa@321; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            //var sqlConnectionString = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=DCAMPUS;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=hutchingsserver.database.windows.net,1433;Initial Catalog=Hutchings_2018-10-24-Backup;Persist Security Info=False;User ID=hutchingsadmin;Password=Hutchpune@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //   var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus_2022-09-07T12-43Z;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=chikkatti.database.windows.net,1433;Initial Catalog=chikkatti;Persist Security Info=False;User ID=chikkatti;Password=vaps@123;Connection Timeout=30;";
            var sqlConnectionString = "Data Source=calcuttacampus.database.windows.net,1433;Initial Catalog=calcuttacampus;Persist Security Info=False;User ID=calcutta;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
           // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LibraryServiceHub")));
            services.AddScoped<LibraryContext>().AddDbContext<LibraryContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LibraryServiceHub")));


            // Add framework services. 

            services.AddScoped<MasterCategoryInterface, MasterCategoryImpl>();
            services.AddScoped<MasterDepartmentInterface, MasterDepartmentImpl>();
            services.AddScoped<MasterPeriodicityInterface, MasterPeriodicityImpl>();
            services.AddScoped<MasterLanguageInterface, MasterLanguageImpl>();
            services.AddScoped<MasterSubjectInterface, MasterSubjectImpl>();
            services.AddScoped<MasterFloorInterface, MasterFloorImpl>();
            services.AddScoped<MasterPublisherInterface, MasterPublisherImpl>();
            services.AddScoped<RackDetailsInterface, RackDetailsImpl>();
            services.AddScoped<CirculationParameterInterface, CirculationParameterImpl>();
            services.AddScoped<MasterTimeSlabInterface, MasterTimeSlabImpl>();
            services.AddScoped<MasterVendorInterface, MasterVendorImpl>();
            services.AddScoped<MasterGuestInterface, MasterGuestImpl>();
            services.AddScoped<MasterAuthorInterface, MasterAuthorImpl>();
            services.AddScoped<BookRegisterInterface, BookRegisterImpl>();
            services.AddScoped<MasterDonorInterface, MasterDonorImpl>();
            services.AddScoped<BookTransactionInterface, BookTransactionImpl>();
            services.AddScoped<BookCirculationReportInterface, BookCirculationReportImpl>();
            services.AddScoped<BookRegisterReportInterface, BookRegisterReportImpl>();
            services.AddScoped<BookTypeReportInterface, BookTypeReportImpl>();
            services.AddScoped<RackReportInterface, RackReportImpl>();
            services.AddScoped<BookArrivalReportInterface, BookArrivalReportImpl>();
            services.AddScoped<BookDetailsImportInterface, BookDetailsImportImpl>();
            //services.AddScoped<Staff_BookTranasctionInterface, Staff_BookTranasctionImpl>();
          
            services.AddScoped<MasterAccessoriesInterface, MasterAccessoriesImpl>();
            services.AddScoped<LibTransactionReportInterface, LibTransactionReportImpl>();
            services.AddScoped<AvailableBooksReportInterface, AvailableBooksReportImpl>();


            services.AddScoped<LostBookInterface, LostBookImpl>();
            services.AddScoped<LostBookReportInterface, LostBookReportImpl>();

            services.AddScoped<MasterLibraryInterface, MasterLibraryImpl>();
            services.AddScoped<PurchaseDonateReportInterface, PurchaseDonateReportImpl>();
            services.AddScoped<UserClassLibraryInterface, UserClassLibraryImpl>();
            services.AddScoped<LibraryMonthEndReportInterface, LibraryMonthEndReportImpl>();
            services.AddScoped<ImportLibrarydataInterface, ImportLibrarydataImpl>();
           
           services.AddScoped<CLGBookTransactionInterface, CLGBookTransactionImpl>();
           services.AddScoped<SCHTransactionDetailsReportInterface, SCHTransactionDetailsReportImpl>();

            //////Non-Book
            services.AddScoped<MasterSubscriptionInterface, MasterSubscriptionImpl>();
            services.AddScoped<MasterNonBookInterface, MasterNonBookImpl>();
            services.AddScoped<NewsPaperClippingInterface, NewsPaperClippingImpl>();
            services.AddScoped<NonBookTransactionInterface, NonBookTransactionImpl>();
            services.AddScoped<CLGNonBookTransactionInterface, CLGNonBookTransactionImpl>();
            services.AddScoped<NonBookReportInterface, NonBookReportImpl>();
            services.AddScoped<SubscriptionReportInterface, SubscriptionReportImpl>();
            services.AddScoped<NonBookTransactionReportInterface, NonBookTransactionReportImpl>();
            services.AddScoped<ClgBookTransactionReportInterface, ClgBookTransactionReportImpl>();
            //ELIBRARY
            services.AddScoped<AddELibraryLinksInterface, AddELibraryLinksImpl>();
            services.AddScoped<BookStatusDetailsInterface, BookStatusDetailsImpl>();
            services.AddScoped<OpacSearchInterface, OpacSearchImpl>();
            services.AddScoped<BookTransactionChikkttiInterface, BookTransactionChikkttiIMPL>();
            //added by adarsh
            services.AddScoped<Lib_stu_punch_reportInterface, Lib_stu_punch_reportIMPL>();


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
            //loggerFactory.AddFile("Logs/libhub-{Date}.txt");

           // loggerFactory.AddFile("Logs/libhub-{Date}.txt");

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
