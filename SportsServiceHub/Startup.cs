using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using SportsServiceHub.com.vaps.Services;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using SportServiceHub.com.vaps.Interfaces;
using SportServiceHub.com.vaps.Services;
using PreadmissionDTOs.com.vaps.Sport;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;

namespace SportsServiceHub
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

            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            // var sqlConnectionString = "Data Source=14.192.18.8;Initial Catalog=HHS;User ID=sa;Password=Welcome@2018;Connection Timeout=30;";

            //var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";           

            //var sqlConnectionString = "Data Source = tcp:vikasa.database.windows.net,1433; Initial Catalog = VIKASASCHOOL; Persist Security Info = False;User ID = Vikasa;  Password = V!kasa@321; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ";

            //var sqlConnectionString = "Data Source=demovaps.database.windows.net,1433;Initial Catalog=TestingDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;";

            //   var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames_2022-05-23T05-38Z; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            //var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
           // var sqlConnectionString = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=DCAMPUS;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;";

            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=chikkatti.database.windows.net,1433;Initial Catalog=chikkatti;Persist Security Info=False;User ID=chikkatti;Password=vaps@123;Connection Timeout=30;";
            // var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            // var sqlConnectionString = "Data Source=hutchingsserver.database.windows.net,1433;Initial Catalog=Hutchings_2018-10-24-Backup;Persist Security Info=False;User ID=hutchingsadmin;Password=Hutchpune@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source=hutchingsserver.database.windows.net,1433;Initial Catalog=Hutchings;Persist Security Info=False;User ID=hutchingsadmin;Password=Hutchpune@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
          //  var sqlConnectionString = "Data Source = dcampus.database.windows.net,1433; Initial Catalog = DCAMPUS; Persist Security Info = False; User ID = decampus; Password = Digit@lc@mpu$@1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("SportsServiceHub")));
            services.AddScoped<SportsContext>().AddDbContext<SportsContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("SportsServiceHub")));
//
            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
            ));
            // Add framework services.
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<MasterSponserInterface, MasterSponserImpl>();
            services.AddScoped<SportMasterDivisionInterface, SportMasterDivisionImpl>();
            services.AddScoped<SportsMasterHouseInterface, SportMasterHouseImpl>();
            services.AddScoped<SportMasterCompitionLevelInterface, SportMasterCompitionLevelImpl>();
            services.AddScoped<SportMasterUOMInterface, SportMasterUOMImpl>();
            services.AddScoped<SportMasterHouseDessignationInterface, SportMasterHouseDessignationImpl>();
            services.AddScoped<SportMasterHouseCommitteInterface, SportMasterHouseCommitteImpl>();
            //services.AddScoped<SportStudentHouseDivisionInterface, SportStudentHouseDivisionImpl>();
            services.AddScoped<SportStudentParticipationReportInterface, SportStudentParticipationReportImpl>();
            services.AddScoped<SportVenuewiseParticipationReportInterface, SportVenuewiseParticipationReportImpl>();
            services.AddScoped<SportPointsAndRecordReportInterface, SportPointsAndRecordReportImpl>();
            services.AddScoped<SportHouseReportInterface, SportHouseReportImpl>();
            services.AddScoped<SportStudentEligbleReportInterface, SportStudentEligbleReportImpl>();
            services.AddScoped<SportTopperListReportInterface, SportTopperListReportImpl>();
            services.AddScoped<SportPointAndRecordReportInterface, SportPointAndRecordReportImpl>();
            services.AddScoped<SportFitnessReportInterface, SportFitnessReportImpl>();
            services.AddScoped<SportHouseCommitteeReportInterface, SportHouseCommitteeReportImpl>();

            services.AddScoped<MasterSponserInterface, MasterSponserImpl>();
            services.AddScoped<MasterEventVenueInterface, MasterEventVenueImpl>();
            services.AddScoped<MasterEventsInterface, MasterEventsImpl>();
            services.AddScoped<EventVenueMappingInterface, EventVenueMappingImpl>();
            services.AddScoped<EventsSponsorInterface, EventsSponsorImpl>();
            services.AddScoped<EventsStudentRecordInterface, EventsStudentRecordImpl>();
            services.AddScoped<MasterCompetitionCategoryInterface, MasterCompetitionCategoryImpl>();
            services.AddScoped<MasterSportsCCGroupInterface, MasterSportsCCGroupImpl>();
            services.AddScoped<MasterSportsCCNameInterface, MasterSportsCCNameImpl>();
            services.AddScoped<MasterSportsCCNameUOMInterface, MasterSportsCCNameUOM_Impl>();
            services.AddScoped<YearEndReportInterface, YearEndReportImpl>();
            services.AddScoped<StudentAgeCalcInterface, StudentAgeCalcImpl>();
            services.AddScoped<HSEligibilityCerficateInterface, HSEligibilityCerficateImpl>();
            services.AddScoped<BMICalculationInterface, BMICalculationImpl>();
            services.AddScoped<BMIReportInterface, BMIReportImpl>();
            services.AddScoped<ProgramMasterInterface, ProgramMasterImpl>();
            services.AddScoped<SportsStudentHouseMappingInterface, SportsStudentHouseMappingImpl>();

            services.AddScoped<SportsMonthEndReportInterface, SportsMonthEndReportImpl>();
            services.AddScoped<StudentAgeCalcReportInterface, StudentAgeCalcReportImpl>();

            services.AddScoped<HouseInchargeInterface,HouseInchargeImpl>();

            services.AddScoped<HouseInchargeReportInterface, HouseInchargeReportImpl>();
            services.AddScoped<SRKVSSportsReportInterface, SRKVSSportsReportIMPL>();
            services.AddScoped<SportsReportTeamPageInterface, SportsReportTeamPageIMPL>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<MasterSponserDMO, MasterSponserDTO>().ReverseMap();
                config.CreateMap<SportMasterDivisionDMO, SportMasterDivisionDTO>().ReverseMap();
                config.CreateMap<SportMasterCompitionLevelDMO, SportMasterCompitionLevelDTO>().ReverseMap();
                config.CreateMap<SportMasterUOMDMO, SportMasterUOMDTO>().ReverseMap();
                config.CreateMap<SportMasterHouseDessignationDMO, SPCC_Master_House_Designation_DTO>().ReverseMap();
                //config.CreateMap<SportMasterHouseCommitteDMO, SPCC_Master_House_Committe_DTO>().ReverseMap(); 
                config.CreateMap<MasterEventVenueDMO, MasterEventVenueDTO>().ReverseMap();
                config.CreateMap<MasterEventsDMO, MasterEventsDTO>().ReverseMap();
                config.CreateMap<EventsMappingDMO, EventsMappingDTO>().ReverseMap();
                config.CreateMap<EventsSponsorDMO, EventsSponsorDTO>().ReverseMap();

                config.CreateMap<MasterCompitionCategoryDMO, MasterCompetitionCategoryDTO>().ReverseMap();
                config.CreateMap<MasterSportsCCGroupDMO, MasterSportsCCGroupDTO>().ReverseMap();
                config.CreateMap<MasterSportsCCNameDMO, MasterSportsCCNameDTO>().ReverseMap();
                config.CreateMap<MasterSportsCCNameUOM_DMO, MasterSportsCCNameUMO_DTO>().ReverseMap();
                config.CreateMap<StudentAgeCalcDMO, StudentAgeCalcDTO>().ReverseMap();
                config.CreateMap<BMICalculationDMO, BMICalculationDTO>().ReverseMap();
                config.CreateMap<ProgramMasterDMO, ProgramMasterDTO>().ReverseMap();

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
            loggerFactory.AddFile("Logs/Sports-{Date}.txt");

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
