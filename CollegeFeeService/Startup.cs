using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs;
using CollegeFeeService;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using PreadmissionDTOs.com.vaps.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using CollegeFeeService.com.vaps.Implementation;
using CollegeFeeService.com.vaps.interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService
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
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMemoryCache();
            //var sqlConnectionString = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=VapsCollege;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source = dcampus.database.windows.net,1433; Initial Catalog = VapsCollege; Persist Security Info = False; User ID = decampus; Password = Digit@lc@mpu$@1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
           //  var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
           // var sqlConnectionString = "Data Source=Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password =Vts@1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ";
          // var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddScoped<ApplicationDBContext>().AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<FeeGroupContext>().AddDbContext<FeeGroupContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));


            services.AddScoped<CollFeeGroupContext>().AddDbContext<CollFeeGroupContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));


            services.AddScoped<CollFeeGroupContext>().AddDbContext<CollFeeGroupContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeFeeService")));
            services.AddScoped<FeeGroupClgInterface, FeeGroupClgImpl>();
            services.AddScoped<FeeHeadClgInterface, FeeHeadClgImpl>();
            services.AddScoped<ClgFeeInstallmentInterface, ClgFeeInstallmentImpl>();
            services.AddScoped<Clg_YearlyFeeGroupMappingInterface, Clg_YearlyFeeGroupMappingImpl>();
            services.AddScoped<CLGFeeAmountEntryInterfaces, CLGFeeAmountEntryImpl>();
            services.AddScoped<CLGStudentFeeEnablePartialPaymentInterface, CLGStudentFeeEnablePartialPaymentImpl>();
            services.AddScoped<Clg_StudentFeeGroupMappingInterface, Clg_StudentFeeGroupMappingImpl>();

            services.AddScoped<CollegeFeeTransactionInterface, CollegeFeeTransactionImpl>();
            services.AddScoped<StaffAndOtherConcessionInterface, StaffAndOtherConcessionImpl>();

            services.AddScoped<CollegemasterstudentconcessionInterface, CollegemasterstudentconcessionImpl>();
            services.AddScoped<CollegeDailyCollectionReportInterface, CollegeDailyCollectionReportImpl>();
            services.AddScoped<CollegeDefaultersReportInterface, CollegeDefaultersReportImpl>();
            services.AddScoped<CollegeHeadWiseCollectionReportInterface, CollegeHeadWiseCollectionReportImpl>();
            services.AddScoped<CollegeStudentwiseAmtEntryInterfaces, CollegeStudentwiseAmtEntryImpl>();

            services.AddScoped<SpecialFeeHeadClgInterface, SpecialFeeHeadClgImpl>();
            services.AddScoped<MasterClgFeeConfigInterface, MasterClgFeeConfigImpl>();
            services.AddScoped<MasterClgFeePrevilegeInterface, MasterClgFeePrevilegeImpl>();
            services.AddScoped<MasterFeeFineSlabClgInterface, MasterFeeFineSlabClgImpl>();
            services.AddScoped<MasterFeeGroupwiseAutoReceiptInterface, MasterFeeGroupwiseAutoReceiptImpl>();
            services.AddScoped<StaffAndOtherAmountEntryInterface, StaffAndOtherAmountEntryImpl>();
            services.AddScoped<StaffAndOtherFeeGroupMappingInterface, StaffAndOtherFeeGroupMappingImpl>();
            services.AddScoped<CollegeStaffAndOtherTransactionInterface, CollegeStaffAndOtherTransactionImpl>();
            services.AddScoped<CLGFeeOpeningBalanceInterface, CLGFeeOpeningBalanceImpl>();
            services.AddScoped<CLGFeeRefundableInterface, CLGFeeRefundableImpl>();
            services.AddScoped<CLGFeeAdjustmentInterface, CLGFeeAdjustmentImpl>();
            services.AddScoped<CLGFeeChequeBounceInterface, CLGFeeChequeBounceImpl>();
            services.AddScoped<CLGFeeWaivedOffInterface, CLGFeeWaivedOffImpl>();
            services.AddScoped<AmountEntryReportInterface, AmountEntryReportImpl>();
            services.AddScoped<ColStudentConcessionReportInterface, ColStudentConcessionReportImpl>();
            services.AddScoped<CollegeOverallFeeStatusInterface, CollegeOverallFeeStatusImpl>();
            services.AddScoped<ClgDatewiseHeadCollectionInterface, ClgDatewiseHeadCollectionImpl>();
            services.AddScoped<CollegeStudentLedgerInterface, CollegeStudentLedgerImpl>();
            services.AddScoped<CollegeYearlyStatusReportInterface, CollegeYearlyStatusReportImpl>();
            services.AddScoped<MakerAndCheckerReportInterface, MakerAndCheckerReportImpl>();
            
            services.AddScoped<CLGFeeGroupwiseRecieptInterfaces, CLGFeeGroupwiseRecieptImpl>();
            services.AddScoped<CollegeFeedetailedReportInterface, CollegeFeedetailedReportImpl>();
            services.AddScoped<CollegeMothEndReportInterface, CollegeMothEndReportImpl>();
            services.AddScoped<CLGOnlinePaymentHeadGroupMappingInterface, CLGOnlinePaymentHeadGroupMappingImpl>();
            services.AddScoped<CollegeFeeDetailsInterface, CollegeFeeDetailsImpl>();
            services.AddScoped<CollegeFeePreadmissionTransactionInterface, CollegeFeePreadmissionTransactionImpl>();
            services.AddScoped<ClgQuotaFeeGroupInterface, ClgQuotaFeeGroupImpl>();
            services.AddScoped<CollegeFeeOnlinePaymentInterface, CollegeFeeOnlinePaymentImpl>();
            services.AddScoped<CollegePaymentApprovalInterface, CollegePaymentApprovalImpl>();
            services.AddScoped<PDC_EntryFormInterface, PDC_EntryFormImpl>();
            services.AddScoped<OtherCollegeStudentEntryInterface, OtherCollegeStudentEntryImpl>();
            services.AddScoped<CollegePreAdmOnlinePaymentInterface, CollegePreAdmOnlinePaymentImpl>();
            services.AddScoped<PDCReportInterface, PDCReportImpl>();
            services.AddScoped<CollegeDemandRegisterReportInterface, CollegeDemandRegisterReportImpl>();
            services.AddScoped<CollegeYearlyCollectionReportInterface, CollegeYearlyCollectionReportImpl>();
            services.AddScoped<CollegeGroupWiseStudentReportInterface, CollegeGroupWiseStudentReportImpl>();
        

            Mapper.Initialize(config =>
            {
                config.CreateMap<FeeGroupClgDMO, FeeGroupClgDTO>().ReverseMap();
                config.CreateMap<FeeYearGroupClgDMO, FeeYearlyGroupClgDTO>().ReverseMap();
                config.CreateMap<FeeHeadClgDMO, FeeHeadClgDTO>().ReverseMap();
                config.CreateMap<Clg_Fee_Installment_DMO, Clg_Fee_Installment_DTO>().ReverseMap();
                config.CreateMap<Clg_Fee_Installments_Yearly_DMO, Clg_Fee_Installment_DTO>().ReverseMap();
                config.CreateMap<Clg_Fee_Installments_Yearly_DMO, Clg_Fee_Installments_Yearly_DTO>().ReverseMap();
                config.CreateMap<Clg_Fee_Installment_Due_Date_DMO, Clg_Fee_Installment_Due_Date_DTO>().ReverseMap();
                config.CreateMap<FeeHeadClgDMO, CLG_YearlyFeeGroupHeadMapping_DTO>().ReverseMap();
              //  config.CreateMap<Fee_Master_College_OtherStudents, Fee_Master_College_OtherStudentsDTO>.ReverseMap();
                config.CreateMap<FeeSpecialFeeGroupDMO, SpecialFeeHeadClgDTO>().ReverseMap();
                config.CreateMap<FeeSpecialFeeGroupsGroupingDMO, SpecialFeeHeadClgGroupsGroupDTO>().ReverseMap();
                config.CreateMap<FeeMasterConfigurationDMO, MasterClgFeeConfigDTO>().ReverseMap();
                config.CreateMap<FEeGroupLoginPreviledgeDMO, MasterClgFeePrevilegeDTO>().ReverseMap();
                config.CreateMap<FeeFineSlabDMO, MasterFeeFineSlabClg_DTO>().ReverseMap();
                config.CreateMap<ClgQuotaFeeGroupDMO, ClgQuotaFeeGroupDTO>().ReverseMap();
                config.CreateMap<Fee_College_Studentwise_PDCDMO, PDC_EntryFormDTO>().ReverseMap();
                 config.CreateMap<Fee_College_Studentwise_PDCDMO, PDC_EntryFormDTO>().ReverseMap();
                config.CreateMap<Fee_Y_PaymentDMO, CollegeStaffAndOtherTransactionDTO>().ReverseMap();
              
            });


            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/Collegefee-{Date}.txt");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
