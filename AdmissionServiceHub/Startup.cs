using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using DomainModel.Model;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;

namespace AdmissionServiceHub
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

            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=Demo_Hutchings;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            //  var sqlConnectionString = "Data Source=calcuttacampus.database.windows.net,1433;Initial Catalog=calcuttacampus;Persist Security Info=False;User ID=calcutta;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<AttendanceEntryTypeContext>().AddDbContext<AttendanceEntryTypeContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            //}).AddEntityFrameworkStores<ApplicationDBContext, int>()
            // .AddDefaultTokenProviders();
          
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<AcademicContext>().AddDbContext<AcademicContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<MasterSectionContext>().AddDbContext<MasterSectionContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<SubjectwisePeriodSettingsContext>().AddDbContext<SubjectwisePeriodSettingsContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<MasterPeriodContext>().AddDbContext<MasterPeriodContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<ActivateDeactivateContext>().AddDbContext<ActivateDeactivateContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<AdmissionStandardContext>().AddDbContext<AdmissionStandardContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<AdmissionFormContext>().AddDbContext<AdmissionFormContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<mastercasteContext>().AddDbContext<mastercasteContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<castecategoryContext>().AddDbContext<castecategoryContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<MasterDocumentContext>().AddDbContext<MasterDocumentContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<MasterActivityContext>().AddDbContext<MasterActivityContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<StudentTcReportContext>().AddDbContext<StudentTcReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<StudentYearLossReportContext>().AddDbContext<StudentYearLossReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<studentbirthdayreportContext>().AddDbContext<studentbirthdayreportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<StudentAddressBook1Context>().AddDbContext<StudentAddressBook1Context>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<StudentAddressBook2Context>().AddDbContext<StudentAddressBook2Context>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<AdmissionRegisterContext>().AddDbContext<AdmissionRegisterContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<ClassWiseDailyAttendanceContext>().AddDbContext<ClassWiseDailyAttendanceContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<monthendreportContext>().AddDbContext<monthendreportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<GovernmentBondContext>().AddDbContext<GovernmentBondContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<StudentAttendancecustomReportContext>().AddDbContext<StudentAttendancecustomReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<readmitstudentContext>().AddDbContext<readmitstudentContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<ClasswisestudentdetailsContext>().AddDbContext<ClasswisestudentdetailsContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<ClassTeacherMappingContext>().AddDbContext<ClassTeacherMappingContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<ClasssectionorderContext>().AddDbContext<ClasssectionorderContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<MasterSubjectContext>().AddDbContext<MasterSubjectContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<HHSTCCustomReportContext>().AddDbContext<HHSTCCustomReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<AdhaarNotEnteredListInterface, AdhaarNotEnteredListImpl>();
            services.AddScoped<Adm_School_Master_StreamInterface, Adm_School_Master_StreamImpl>();
            services.AddScoped<Adm_School_Master_CEInterface, Adm_School_Master_CEImpl>(); 

            services.AddScoped<AttendanceRunInterface, AttendanceRunImp>();

            services.AddScoped<AttendanceLPInterface, AttendanceLPImpl>();
            services.AddScoped<SectionAllotmentInterface, SectionAllotmentImpl>();
            services.AddScoped<BatchwiseStudentMappingInterface, BatchwiseStudentMappingImpl>();
            services.AddScoped<AttendanceEntryTypeInterface, AttendanceEntryTypeImp>();
            services.AddScoped<SubjectwisePeriodSettingsInterface, SubjectwisePeriodSettingsImp>();
            services.AddScoped<MasterPeriodInterface, MasterPeriodImp>();
            services.AddScoped<MasterClassHeldInterface, MasterClassHeldImpl>();
            services.AddScoped<ActivateDeactivateStudentInterface, ActivateDeactivateStudentImpl>();
            services.AddScoped<AdmissionStandardInterface, AdmissionStandardImpl>();
            services.AddScoped<SmsEmailSettingInterface, SmsEmailSettingImpl>();
            services.AddScoped<StudentAttendanceEntryInterface, StudentAttendanceEntryImpl>();
            services.AddScoped<mastercasteInterface, mastercasteImpl>();
            services.AddScoped<StudentAdmissionInterface, StudentAdmissionImp>();
            services.AddScoped<castecategoryInterface, castecategoryImpl>();
            services.AddScoped<MasterReligionInterface, MasterReligionImpl>();
            services.AddScoped<MasterDocumentInterface, MasterDocumentImp>();
            services.AddScoped<MasterActivityInterface, MasterActivityImpl>();
            services.AddScoped<StudentTcReportInterface, StudentTcReportImpl>();
            services.AddScoped<StudentYearLossReportInterface, StudentYraeLossReportImpl>();
            services.AddScoped<StudentSmartCardLogReportInterface, StudentSmartCardLogReportImpl>();
            services.AddScoped<TotalStrengthInterface, TotalStrengthImpl>();
            services.AddScoped<HostelFoodConveyanceReportInterface, HostelFoodConveyanceReportImpl>();
            services.AddScoped<StudentAchievementReportInterface, StudentAchievementReportImpl>();
            services.AddScoped<DOBcertificateInterface, DOBcertificateImpl>();
            services.AddScoped<StudyCertificateInterface, StudyCertificateImpl>();
            services.AddScoped<StudentAtttendanceReportInterface, StudentAttendanceReportImpl>();
            services.AddScoped<StudentYearlyAttendanceInterface, StudentYearlyAttendanceImpl>();
            services.AddScoped<AttendanceReportInterface, AttendanceReportImpl>();
            services.AddScoped<OverallDailyAttendanceInterface, OverallDailyAttendanceReportImpl>();
            services.AddScoped<CategoryWiseAttendanceInterface, CategoryWiseAttendanceImpl>();
            services.AddScoped<JSHSAdmissionCertificateInterface, JSHSAdmissionCertificateImpl>();
            services.AddScoped<studentbirthdayreportInterface, studentbirthdayreportImpl>();
            services.AddScoped<StudentAddressBook1Interface, StudentAddressBook1Impl>();
            services.AddScoped<StudentAddressBook2Interface, StudentAddressBook2Impl>();
            services.AddScoped<AdmissionRegisterInterface, AdmissionRegisterImpl>();
            services.AddScoped<ClassWiseDailyAttendanceInterface, ClassWiseDailyAttendanceImpl>();
            services.AddScoped<monthendreportInterface, monthendreportImpl>();
            services.AddScoped<GovernmentBondInterface, GovernmentBondImpl>();
            services.AddScoped<StudenttcReportcustomInterface, StudenttcReportcustomImpl>();
            services.AddScoped<MasterReligionInterface, MasterReligionImpl>();
            services.AddScoped<MasterDocumentInterface, MasterDocumentImp>();
            services.AddScoped<StudentTCInterface, StudentTCImpl>();
            services.AddScoped<StudentSearchInterface, StudentSearchImpl>();
            services.AddScoped<AdditionalFieldInterface, AdditionalFieldImpl>();
            services.AddScoped<readmitstudentInterface, readmitstudentImpl>();
            services.AddScoped<AdmissionImportInterface, AdmissionImportImpl>();
            services.AddScoped<SendingSMSandMailsInterface, SendSMSandMailsImpl>();
            services.AddScoped<ClasswisestudentdetailsInterface, classwisestudentdetailsssImpl>();
            services.AddScoped<ClassTeacherMappingInterface, ClassTeacherMappingImpl>();
            services.AddScoped<ClasssectionorderInterface, ClasssectionorderImpl>();
            services.AddScoped<HHSTCCustomReportInterface, HHSTCCustomReportImpl>();
            services.AddScoped<ClassTeacherReportAttendanceInterface, ClassTeacherReportAttendanceReportImpl>();
            services.AddScoped<CategoryWiseTotalStrengthInterface, CategoryWiseTotalStrengthImpl>();
            services.AddScoped<HHSStudyCertificateInterface, HHSStudyCertificateImpl>();
            services.AddScoped<SmartcarddetailsInterface, SmartcarddetailsImpl>();
            services.AddScoped<overalldailyattendanceabsentsmsInterface, overalldailyattendanceabsentsmsImpl>();
            services.AddScoped<DocumentViewReportAdmInterface, DocumentViewReportAdmImpl>();
            services.AddScoped<VikasaAdmissionReportInterface, VikasaAdmissionReportImpl>();
            services.AddScoped<AdmissionSMSReportInterface, AdmissionSMSReportImpl>();
            services.AddScoped<BBKVCustomReportInterface, BBKVCustomReportImpl>();
            services.AddScoped<DatewiseAttendanceReportInterface, DatewiseAttendanceReportImpl>();
            services.AddScoped<SwimmingAttendanceInterface, SwimmingAttendanceImpl>();
            services.AddScoped<SwimmingAttendanceReportInterface, SwimmingAttendanceReportImpl>();
            services.AddScoped<ECSReportInterface, ECSReportImpl>();
            services.AddScoped<SiblingEmployeeStudentReportInterface, SiblingEmployeeStudentReportImpl>();
            services.AddScoped<SchoolTpinGenreationInterface, SchoolTpinGenreationImpl>();
            services.AddScoped<YearlyAnalysisReportInterface, YearlyAnalysisReportImpl>();
            services.AddScoped<PercentageWiseAttendanceReportInterface, PercentageWiseAttendanceReportImpl>();
            services.AddScoped<SRKVSStudyCertificateInterface, SRKVSStudycertificateImpl>();
            services.AddScoped<studenttccustomreportInterface, studenttccustomreportImpl>();
            services.AddScoped<SMSMasterApprovalInterface, SMSMasterApprovalImpl>();
            services.AddScoped<SMSApprovalTransactionInterface, SMSApprovalTransactionImpl>();
            services.AddScoped<SMSGenaralInterface, SMSGenaralImpl>();
            services.AddScoped<SMSResendInterface, SMSResendImpl>();
            services.AddScoped<RFIDDashboardInterface, RFIDDashboardImpl>();
            services.AddScoped<EMAILSMSTemplateSettingInterface, EMAILSMSTemplateSettingImpl>();
            services.AddScoped<MasterSmsEmailParameterInterface, MasterSmsEmailParameterImpl>();
            services.AddScoped<ReligionCasteCategoryReportInterface, ReligionCasteCategoryReportImpl>();
            services.AddScoped<GeneralSiblingEmployeeMappingInterface, GeneralSiblingEmployeeMappingImpl>();
            services.AddScoped<UserMergeInterface, UserMergeImpl>();
            services.AddScoped<SMSMail_HeaderInterface, SMSMail_HeaderImpl>();
            services.AddScoped<StudentCompliantsInterface, StudentCompliantsImpl>();
            services.AddScoped<MasterHeaderDetailsInterface, MasterHeaderDetailsImp>();             
            services.AddScoped<VaccineAgeCriteriaInterface, VaccineAgeCriteriaImpl>();             
            services.AddScoped<StudentIdCardFormatInterface, StudentIdCardFormatImpl>();            
            services.AddScoped<SmsEmailModuleCountInterface, SmsEmailModuleCountIMPL>();
            services.AddScoped<LeftStudentsReportInterface, LeftStudentsReportImp>();
            services.AddScoped<StudentConsolidatedCertificateReportInterface, StudentConsolidatedCertificateReportImpl>();//QRCodeGenerationImpl
            services.AddScoped<QRCode_Generation_Interface, QRCodeGenerationImpl>();
            services.AddScoped<Master_ExamQualified_ClassInterface, Master_ExamQualified_ClassImpl>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Adm_SchoolAttendanceLoginUserClassSubject, MasterSubjectAllMDTO>().ReverseMap();
                config.CreateMap<Adm_SchoolAttendanceLoginUserClass, ClassSectionDTO>().ReverseMap();
                config.CreateMap<Adm_SchoolAttendanceLoginUser, AttendanceLPDTO>().ReverseMap();
                config.CreateMap<MasterClassHeld, MasterClassHeldDTO>().ReverseMap();
                config.CreateMap<AttendanceEntryTypeDMO, AttendanceEntryTypeDTO>().ReverseMap();
                config.CreateMap<SubjectwisePeriodSettingsDMO, SubjectwisePeriodSettingsDTO>().ReverseMap();
                config.CreateMap<MasterPeriodDMO, MasterPeriodDTO>().ReverseMap();
                config.CreateMap<MasterPeriodCategoryDMO, MasterPeriodCategoryDTO>().ReverseMap();
                config.CreateMap<ActivateDeactivateStudentDMO, ActivateDeactivateStudentDTO>().ReverseMap();
                config.CreateMap<AdmissionStandardDMO, AdmissionStandardDTO>().ReverseMap();
                config.CreateMap<SMSEmailSetting, SmsEmailDTO>().ReverseMap();
                config.CreateMap<Adm_studentAttendance, StudentAttendanceEntryDTO>().ReverseMap();
                config.CreateMap<Adm_studentAttendance, StudentAttTempDTO>().ReverseMap();
                config.CreateMap<Adm_studentAttendanceStudents, StudentAttTempDTO>().ReverseMap();
                config.CreateMap<Adm_studentAttendanceSubjects, StudentAttendanceEntryDTO>().ReverseMap();
                config.CreateMap<Adm_StudentAttendancePeriodwiseDMO, StudentAttendanceEntryDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, Adm_M_StudentDTO>().ReverseMap();
                config.CreateMap<StudentTC, StudentTCDTO>().ReverseMap();
                config.CreateMap<mastercasteDMO, mastercasteDTO>().ReverseMap();
                config.CreateMap<castecategoryDMO, castecategoryDTO>().ReverseMap();
                config.CreateMap<MasterReligionDMO, MasterReligionDTO>().ReverseMap();
                config.CreateMap<MasterDocumentDMO, MasterDocumentDTO>().ReverseMap();
                config.CreateMap<MasterActivityDMO, MasterActivityDTO>().ReverseMap();
                config.CreateMap<StudentTC, StudentTcReportDTO>().ReverseMap();
                config.CreateMap<StudentTC, StudentYearLosReportDTO>().ReverseMap();
                config.CreateMap<StudentTC, StudentSmartCardLogReportDTO>().ReverseMap();
                config.CreateMap<StudentAchivementDMO, StudentAchivementDTO>().ReverseMap();
                config.CreateMap<IVRM_COLOUMN_REPORT, SchoolYearWiseStudentDTO>().ReverseMap();
                config.CreateMap<GovernmentBondDMO, GovernmentBondDTO>().ReverseMap();
                config.CreateMap<AdditionalField, StudentAttendanceReportDTO>().ReverseMap();
                config.CreateMap<School_Adm_Y_StudentDMO, SchoolYearWiseStudentDTO>().ReverseMap();
                config.CreateMap<AdditionalField, AdditionalFieldDTO>().ReverseMap();
                config.CreateMap<readmitstudentDMO, readmitstudentDTO>().ReverseMap();
                config.CreateMap<AdmSchoolAttendanceSubjectBatch, AdmSchoolAttendanceSubjectBatchDTO>().ReverseMap();
                config.CreateMap<AdmSchoolAttendanceSubjectBatchStudents, AdmSchoolAttendanceSubjectBatchStudentsDTO>().ReverseMap();
                config.CreateMap<StudentSiblingDMO, AdmittedStudentSiblingDTO>().ReverseMap();
                config.CreateMap<StudentActitvityDMO, StudentActivityDTO>().ReverseMap();
                config.CreateMap<StudentSourceDMO, StudentSourceDTO>().ReverseMap();
                config.CreateMap<StudentReferenceDMO, StudentReferenceDTO>().ReverseMap();
                config.CreateMap<MasterStudentBondDMO, GovernmentBondDTO>().ReverseMap();
                config.CreateMap<StudentPrevSchoolDMO, AdmittedStudentPrevSchoolDTO>().ReverseMap();
                config.CreateMap<StudentGuardianDMO, AdmittedStudentGuardianDTO>().ReverseMap();
                config.CreateMap<StudentDocumentDMO, StudentDocumentDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, ClasswisestudentdetailsDTO>().ReverseMap();
                config.CreateMap<ClassTeacherMappingDMO, ClassTeacherMappingDTO>().ReverseMap();
                config.CreateMap<School_M_Class, ClasssectionorderDTO>().ReverseMap();
                config.CreateMap<School_M_Section, ClasssectionorderDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, StudycertificateDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, HHSTCCustomReportDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, ClassTeacherReportAttendanceDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, CategoryWiseTotalStrengthDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, Adm_M_Student_TempDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student_FatherMobileNo, Adm_M_Student_TempDTO>().ReverseMap();
                config.CreateMap<Adm_Master_Father_Email, Adm_M_Student_Eamil>().ReverseMap();
                config.CreateMap<Adm_M_Mother_MobileNo, Adm_M_Mother_MobileNo1>().ReverseMap();
                config.CreateMap<Adm_M_Mother_Emailid, Adm_M_Mother_Emailid1>().ReverseMap();
                config.CreateMap<Adm_M_Student_MobileNo, Adm_M_Student_MobileNoDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student_Email_Id, Adm_M_Student_EmailIdDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, HHSStudyCertificateDTO>().ReverseMap();
                //adedd By 
               
            });

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
            loggerFactory.AddFile("Logs/Admmissionhub-{Date}.txt");

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