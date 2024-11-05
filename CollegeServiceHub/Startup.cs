using AutoMapper;
using CollegeServiceHub.Impl;
using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace CollegeServiceHub
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

            //var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeServiceHub")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<ClgMasterBranchInterface, ClgMasterBranchImpl>();
            services.AddScoped<ClgMasterAcademicYearInterface, ClgMasterAcademicYearImpl>();
            services.AddScoped<Atten_Subject_MaxPeriodInterface, Atten_Subject_MaxPeriodImpl>();
            services.AddScoped<CLGMasterSemisterInterface, CLGMasterSemisterImpl>();
            services.AddScoped<ClgMasterQuotaInterface, ClgMasterQuotaImpl>();
            services.AddScoped<CollegeStudentAdmissionInterface, CollegeStudentAdmissionImpl>();
            services.AddScoped<MasterCourseInterface, MasterCourseImpl>();
            services.AddScoped<ClgMasterCourseCategoryMapInterface, ClgMasterCourseCategoryMapImpl>();
            services.AddScoped<ClgMasterCourseBranchMapInterface, ClgMasterCourseBranchMapImpl>();
            services.AddScoped<ClgSectionAllotmentInterface, ClgSectionAllotmentImpl>();
            services.AddScoped<clg_CB_SEM_MappingInterface, clg_CB_SEM_MappingImpl>();
            services.AddScoped<StudentGeneralRegisterInterface, StudentGeneralRegisterImpl>();
            services.AddScoped<ClgSeatDistributionInterface, ClgSeatDistributionImpl>();
            services.AddScoped<MasterBatchInterface, MasterBatchImpl>();
            services.AddScoped<CLGSubjectSchemeTypeInterface, CLGSubjectSchemeTypeImpl>();
            services.AddScoped<ClgMasterCategoryInterface, ClgmasterCategoryImpl>();
            services.AddScoped<ClgyearlycoursemappingInterface, ClgyearlycoursemappingImpl>();
            services.AddScoped<CLGRegNoFormatInterface, CLGRegNoFormatImpl>();
            services.AddScoped<Atten_Login_UserInterface, Atten_Login_UserImpl>();
            services.AddScoped<Atten_Batch_MappingInterface, Atten_Batch_MappingImpl>();
            services.AddScoped<ClgAttendanceEntryInterface, ClgAttendanceEntryImpl>();
            services.AddScoped<CollegeMasterSectionInterface, CollegeMasterSectionImpl>();
            services.AddScoped<TotalSeatAllotmentInterface, TotalSeatAllotmentImpl>();
            services.AddScoped<TeresianReportInterface, TeresianReportImpl>();
            services.AddScoped<QuotaCategoryReportInterface, QuotaCategoryReportImpl>();
            services.AddScoped<CategorySeatDistributionInterface, CategorySeatDistributionImpl>();
            services.AddScoped<collegeadmissionImportInterface, collegeadmissionImportImpl>();
            services.AddScoped<CollegeAdmissionStandardInterface, CollegeAdmissionStandardImpl>();
            services.AddScoped<CollegemastercasteInterface, CollegemastercasteImpl>();
            services.AddScoped<CollegecastecategoryInterface, CollegecastecategoryImpl>();
            services.AddScoped<AdmissionRegisterInterface, AdmissionRegisterImpl>();
            services.AddScoped<CollegeDailyAttendanceInterface, CollegeDailyAttendanceImpl>();
            services.AddScoped<CollegeMultiHoursAttendanceEntryInterface, CollegeMultiHoursAttendanceEntryImpl>();
            services.AddScoped<MonthEndReportInterface, MonthEndReportImpl>();
            services.AddScoped<NAACReportInterface, NAACReportImpl>();            
            services.AddScoped<CollegegeneralsmsInterface, CollegegeneralsmsImpl>();
            services.AddScoped<ClgAttendanceSMSDetailsReportInterface, ClgAttendanceSMSDetailsReportImpl>();
            services.AddScoped<statewisestudentadmissionInterface, statewisestudentadmissionImpl>();
            services.AddScoped<BranchChangeInterface, BranchChangeIMPL>();
            services.AddScoped<TeressianCertificateInterface, TeressianCertificateIMPL>();
            services.AddScoped<CollegeUsernameCreationInterface, CollegeUsernameCreationImpl>();
            services.AddScoped<CollegeMasterDocumentInterface, CollegeMasterDocumentImpl>();
            services.AddScoped<CollegeActiveDeactiveStudentsInterface, CollegeActiveDeactiveStudentsImpl>();
            services.AddScoped<CollegeCancellationConfigurationInterface, CollegeCancellationConfigurationImpl>();
            services.AddScoped<CollegeAdmssionCancelProcessInterface, CollegeAdmssionCancelProcessImpl>();
            services.AddScoped<CollegeStudyCertificateReportInterface, CollegeStudyCertificateReportImpl>();
            services.AddScoped<CollegeStudenttctransactionInterface, CollegeStudenttctransactionImpl>();
            services.AddScoped<CollegeStudentTCReportInterface, CollegeStudentTCReportImpl>();
            services.AddScoped<CollegeAttendanceAbsentSMSInterface, CollegeAttendanceAbsentSMSImpl>();
            services.AddScoped<CollegeTpinGenerationInterface, CollegeTpinGenerationImpl>();
            services.AddScoped<CollegeAttendanceEntryNewInterface, CollegeAttendanceEntryNewImpl>();
            services.AddScoped<BloodGroupWiseStudentDetailsReportInterface, BloodGroupWiseStudentDetailsReportImpl>();
            services.AddScoped<CLG_PrincipleSMS_SendInterface, CLG_PrincipleSMS_SendImpl>();
            services.AddScoped<ChangeOfBranchReportInterface, ChangeOfBranchReportImpl>();     
            services.AddScoped<StudentAddressBookInterface, StudentAddressBookImpl>();
            services.AddScoped<CollegeDocumentReportInterface, CollegeDocumentReportImpl>();
            services.AddScoped<StudentActiveInactiveReportInterface, StudentActiveInactiveReportImpl>();
            //ClgSMSEmailCountIMPL
            services.AddScoped<ClgSMSEmailCountInterface, ClgSMSEmailCountIMPL>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<ClgMasterBranchDMO, ClgMasterBranchDTO>().ReverseMap();
                config.CreateMap<ClgMasterAcademicYearDMO, ClgMasterAcademicYearDTO>().ReverseMap();
                config.CreateMap<Adm_College_Atten_Subject_MaxPeriodDMO, Atten_Subject_MaxPeriodDTO>().ReverseMap();
                config.CreateMap<CLG_Adm_Master_SemesterDMO, CLGMasterSemisterDTO>().ReverseMap();
                config.CreateMap<Clg_Adm_College_QuotaDMO, ClgQuotaDTO>().ReverseMap();
                config.CreateMap<Clg_Adm_College_Quota_CategoryDMO, ClgQuotaDTO>().ReverseMap();
                config.CreateMap<Adm_Master_College_StudentDMO, AdmMasterCollegeStudentDTO>().ReverseMap();
                config.CreateMap<Adm_Master_College_StudentDMO, save_firsttab_details>().ReverseMap();
                config.CreateMap<MasterCourseDMO, MasterCourseDTO>().ReverseMap();
                config.CreateMap<Adm_Course_Branch_MappingDMO, ClgMasterCourseBranchMapDTO>().ReverseMap();
                config.CreateMap<ClgMasterCourseCategoryMapDMO, ClgMasterCourseCategoryMapDTO>().ReverseMap();
                config.CreateMap<AdmCollegeMasterBatchDMO, AdmCollegeMasterBatchDTO>().ReverseMap();
                config.CreateMap<Adm_College_Yearly_StudentDMO, ClgYearWiseStudentDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentSMSNoDMO, Adm_College_Student_SMSNoDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentEmailIdDMO, Adm_College_Student_EmailIdDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentReferenceDMO, AdmCollegeStudentReferenceDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentSourceDMO, AdmCollegeStudentSourceDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentPrevSchoolDMO, AdmCollegeStudentPrevSchoolDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentGuardianDMO, AdmCollegeStudentGuardianDTO>().ReverseMap();
                config.CreateMap<AdmCollegeStudentSiblingsDetailsDMO, AdmCollegeStudentSiblingsDetailsDTO>().ReverseMap();
                config.CreateMap<ClgMasterCategoryDMO, ClgMasterCategoryDTO>().ReverseMap();
                config.CreateMap<Adm_College_Student_AttendanceDMO, ClgAttendanceEntryDTO>().ReverseMap();
                config.CreateMap<CollegeAdmissionStandardDMO, CollegeAdmissionStandardDTO>().ReverseMap();
                config.CreateMap<CollegecastecaegoryDMO, CollegecastecategoryDTO>().ReverseMap();
                config.CreateMap<CollegemastercasteDMO, CollegemastercasteDTO>().ReverseMap();
                config.CreateMap<BranchChangeDMO, BranchChangeDTO>().ReverseMap();
                config.CreateMap<Adm_College_Student_SubjectMarksDMO, Adm_College_Student_SubjectMarksDTO>().ReverseMap();
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

            loggerFactory.AddFile("Logs/CollegeAdmmissionhub-{Date}.txt");

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
