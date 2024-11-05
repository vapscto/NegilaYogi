using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using TimeTableServiceHub.Interfaces;
using TimeTableServiceHub.Services;
using PreadmissionDTOs.com.vaps.TT;
using DomainModel.Model.com.vapstech.TT;
using TimeTableServiceHub.com.vaps.Interfaces;
using TimeTableServiceHub.com.vaps.Services;
using TimeTableServiceHub.com.vaps.Interfaces.College;
using TimeTableServiceHub.com.vaps.Services.College;

namespace TimeTableServiceHub
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

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            //  var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=CollegeTest;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";

            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_NewSharedHost;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<TTContext>().AddDbContext<TTContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("TimeTableServiceHub")));

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            services.AddScoped<CategoryClassMappInterface, CategoryClassMappImpl>();
            services.AddScoped<BifurcationInterface, BifurcationImpl>();
            services.AddScoped<PeriodAllocationInterface, TTPeriodAllocationImpl>();
            services.AddScoped<StaffMasterInterface, TTStaffMasterImpl>();
            services.AddScoped<ClassMasterInterface, TTClassMasterImpl>();
            services.AddScoped<PeriodTimeSettingInterface, PeriodTimeSettingImpl>();
            services.AddScoped<MasterDayInterface, MasterDayImpl>();
            services.AddScoped<FixingInterface, TTFixingImpl>();
            services.AddScoped<RestrictionInterface, TTRestrictionImpl>();
            services.AddScoped<StaffReplacementInTheirTTInterface, TTStaffReplacementInTheirTTImpl>();
            services.AddScoped<StaffReplacementForClassSectionInterface, TTStaffReplacementForClassSectionImpl>();
            services.AddScoped<ClassWiseTTInterface, TTClassWiseTTImpl>();
            services.AddScoped<StaffWiseTTInterface, TTStaffWiseTTImpl>();
            services.AddScoped<StaffReplacementInUnallocatedPeriodInterface, TTStaffReplacementInUnallocatedPeriodImpl>();
            services.AddScoped<TTGenerationFromPreviousVersionInterface, TTTTGenerationFromPreviousVersionImpl>();
            services.AddScoped<DeputationInterface, TTDeputationImpl>();
            services.AddScoped<ConfigurationInterface, ConfigurationImpl>();
            services.AddScoped<TTCategoryInterface, TTCategoryImpl>();
            services.AddScoped<BreaktimesettingsInterface, TTBreaktimesettingImpl>();
            services.AddScoped<TT_Master_Subject_AbbreviationInterface, TT_Master_Subject_AbbreviationImpl>();
            services.AddScoped<TT_ConsecutiveInterface, TT_ConsecutiveImpl>();
            services.AddScoped<LabconstraintsInterface, LabconstraintsImpl>();
            services.AddScoped<TimeTableGenerationInterface, TimeTableGenerationImpl>();
            services.AddScoped<StaffReplacementUnalocatedPeriodInterface, StaffReplacementUnalocatedPeriodImpl>();
            services.AddScoped<MasterBuildingInterface, MasterBuildingImpl>();
            services.AddScoped<TTConsolidatedInterface, TTConsolidatedImpl>();
            services.AddScoped<ConstraintReportInterface, ConstraintReportImpl>();
            services.AddScoped<StaffReplacementFromExistingToNewInterface, StaffReplacementFromExistingToNewImpl>();
            services.AddScoped<StaffMaxMinDaySettingInterface, StaffMaxMinDaySettingImpl>();
            services.AddScoped<TTExistingToNewInterface, TTExistingToNewImpl>();
            services.AddScoped<TTMonthEndReportInterface, TTMonthEndReportImpl>();
            services.AddScoped<SubjectwiseInterface, SubjectwiseImpl>();
            services.AddScoped<DeputationReportInterface, DeputationReportIMPL>();
            services.AddScoped<StaffPeriodTransformInterface, StaffPeriodTransformImpl>();
            services.AddScoped<ManualperiodinsertionInterface, ManualperiodinsertionImpl>();
            services.AddScoped<DeputationNewInterface, DeputationNewImpl>();

            //College Timetable
            services.AddScoped<CLGCategoryMappingInterface, CLGCategoryMappingImpl>();
            services.AddScoped<CLGMasterDayInterface, CLGMasterDayImpl>();
            services.AddScoped<CLGTTCommonInterface, CLGTTCommonImpl>();
            services.AddScoped<CLGPRDDistributionInterface, CLGPRDDistributionImpl>();
            services.AddScoped<CLGBreakTimeSettingInterface, CLGBreakTimeSettingImpl>();
            services.AddScoped<ClgPeriodAllocationInterface, ClgPeriodAllocationImpl>();
            services.AddScoped<CLGMasterBuildingInterface, CLGMasterBuildingImpl>();
            services.AddScoped<CLGBifurcationInterface, CLGBifurcationImpl>();
            services.AddScoped<CLGConsecutiveInterface, CLGConsecutiveImpl>();
            services.AddScoped<CLGLabInterface, CLGLabImpl>();
            services.AddScoped<CLGFixingInterface, CLGFixingImpl>();
            services.AddScoped<CLGRestrictionInterface, CLGRestrictionImpl>();
            services.AddScoped<CLGTTGenerationInterface, CLGTTGenerationImpl>();
            services.AddScoped<CLGTTStaffWiseReportInterface, CLGTTStaffWiseReportImpl>();
            services.AddScoped<CLGTTCourseWiseReportInterface, CLGTTCourseWiseReportImpl>();
            services.AddScoped<CLGDeputationInterface, CLGDeputationImpl>();
            services.AddScoped<CLGDeputationReportInterface, CLGDeputationReportImpl>();
            services.AddScoped<CLGTTSubjectWiseReportInterface, CLGTTSubjectWiseReportImpl>();
            services.AddScoped<CLGStaffReplacementInSectionInterface, CLGStaffReplacementInSectionImpl>();
            services.AddScoped<CLGStaffRplInTheirTTInterface, CLGStaffRplInTheirTTImpl>();
            services.AddScoped<CLGStaffRplInUnallocatedPeriodInterface, CLGStaffRplInUnallocatedPeriodImpl>();
            services.AddScoped<CLGManualperiodinsertionInterface, CLGManualperiodinsertionImpl>();
            services.AddScoped<CLGStaffPeriodTransformInterface, CLGStaffPeriodTransformImpl>();
            services.AddScoped<CLGConsolidatedReportInterface, CLGConsolidatedReportImpl>();
            services.AddScoped<ClasswiseConsolidatedReportInterface, ClasswiseConsolidatedReportImpl>();
            services.AddScoped<CLGTTConstraintReportInterface, CLGTTConstraintReportImpl>();
            services.AddScoped<CLGTTCollegewiseConsolidatedReportInterface, CLGTTCollegewiseConsolidatedReportImpl>();
            services.AddScoped<TTMasterFacilitiesInterface, TTMasterFacilitiesImpl>();
            services.AddScoped<TTMasterRoomInterface, TTMasterRoomImpl>();
            services.AddScoped<CLGTTCollegewiseConsolidatedReportInterface, CLGTTCollegewiseConsolidatedReportImpl>();
            services.AddScoped<TTMasterFacilitiesInterface, TTMasterFacilitiesImpl>();
            services.AddScoped<CLGRoomMappingInterface, CLGRoomMappingImpl>();
            services.AddScoped<RoomMappingInterface, RoomMappingImpl>();
            services.AddScoped<CLGTTCourseReportBWMCInterface, CLGTTCourseReportBWMCImpl>();
            services.AddScoped<TTStaffHoursInterface, TTStaffHoursImpl>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<TT_Category_Class_DTO, TT_Category_Class_DMO>().ReverseMap();
                config.CreateMap<TT_Bifurcation_DTO, TT_Bifurcation_DMO>().ReverseMap();
                config.CreateMap<TT_Bifurcation_Details_DTO, TT_Bifurcation_Details_DMO>().ReverseMap();
                config.CreateMap<TT_Master_Day_Period_TimeDTO, TT_Master_Day_Period_TimeDMO>().ReverseMap();
                config.CreateMap<TT_Master_PeriodDMO, TTPeriodAllocationDTO>().ReverseMap();
                config.CreateMap<TT_Master_Period_ClasswiseDMO, TTPeriodAllocationDTO>().ReverseMap();
                config.CreateMap<TT_Master_Staff_AbbreviationDMO, TTStaffMasterDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Day_PeriodDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Day_StaffDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Day_Staff_ClassSectionDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Day_SubjectDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Day_Subject_ClassSectionDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Period_StaffDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Period_Staff_ClassSectionDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Period_SubjectDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Fixing_Period_Subject_ClassSectionDMO, TTFixingDTO>().ReverseMap();
                config.CreateMap<TT_Final_Period_DistributionDMO, TTClassMasterDTO>().ReverseMap();
                config.CreateMap<TT_Final_Period_Distribution_DetailedDMO, TTClassMasterDTO>().ReverseMap();
                config.CreateMap<TT_Master_DayDTO, TT_Master_DayDMO>().ReverseMap();
                config.CreateMap<TT_Master_DayDTO, TT_Master_Day_ClasswiseDMO>().ReverseMap();
                config.CreateMap<TT_Master_BuildingDTO, TT_Master_BuildingDMO>().ReverseMap();
                config.CreateMap<TT_Master_Building_Class_SectionDMO, TT_Master_BuildingDTO>().ReverseMap();
                config.CreateMap<TT_ConfigurationDMO, TTConfigurationDTO>().ReverseMap();
                config.CreateMap<TTMasterCategoryDMO, TTMasterCategoryDTO>().ReverseMap();
                config.CreateMap<TTBreakTimeSettingsDMO, TTBreakTimesettingDTO>().ReverseMap();
                config.CreateMap<TT_Master_Subject_AbbreviationDMO, TT_Master_Subject_AbbreviationDTO>().ReverseMap();
                config.CreateMap<TT_ConsecutiveDMO, TT_ConsecutiveDTO>().ReverseMap();
                config.CreateMap<TT_LABLIB_DMO, TT_LABLIB_DTO>().ReverseMap();
                config.CreateMap<TT_LABLIB_DetailsDMO, TT_LABLIB_DetailsDTO>().ReverseMap();
                config.CreateMap<TT_Master_Break_BefPeriodsDMO, TT_Master_Break_BefPeriodsDTO>().ReverseMap();
                config.CreateMap<TT_Master_Break_AftPeriodsDMO, TT_Master_Break_AftPeriodsDTO>().ReverseMap();
                config.CreateMap<TT_Final_GenerationDMO, TT_Final_GenerationDTO>().ReverseMap();
                config.CreateMap<TT_Final_Generation_DetailedDMO, TT_Final_Generation_DetailedDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Day_PeriodDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Day_StaffDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Day_Staff_ClassSectionDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Day_SubjectDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Day_Subject_ClassSectionDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Period_StaffDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Period_Staff_ClassSectionDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Period_SubjectDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Restricting_Period_Subject_ClassSectionDMO, TTRestrictionDTO>().ReverseMap();
                config.CreateMap<TT_Staff_DeputationDMO, TTDeputationDTO>().ReverseMap();
                config.CreateMap<StaffMaxMinDaySettingDMO, StaffMaxMinDaySettingDTO>().ReverseMap();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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
