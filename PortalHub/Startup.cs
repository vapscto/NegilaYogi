using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PortalHub.com.vaps.Student.Interfaces;
using PortalHub.com.vaps.Student.Services;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using PortalHub.com.vaps.Employee.Services;
using PortalHub.com.vaps.Employee.Interfaces;
using PortalHub.com.vaps.Chairman.Services;
using PortalHub.com.vaps.Chairman.Interfaces;
using PortalHub.com.vaps.Principal.Interfaces;
using PortalHub.com.vaps.Principal.Services;
using DomainModel.Model.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs;
using PortalHub.com.vaps.HOD.Interfaces;
using PortalHub.com.vaps.HOD.Services;
using DomainModel.Model.com.vapstech.Portals.Student;
using PortalHub.com.vaps.IVRS.Interfaces;
using PortalHub.com.vaps.IVRS.Services;
using PortalHub.com.vaps.IVRM.Interfaces;
using PortalHub.com.vaps.IVRM.Services;
using PreadmissionDTOs.com.vaps.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using PortalHub.com.vaps.Interfaces;
using PortalHub.com.vaps.Services;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using PortalHub.com.vaps.MobileApp.Interfaces;
using PortalHub.com.vaps.MobileApp.Services;

namespace PortalHub
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

            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            //var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=vidyabharathi.database.windows.net,1433;Initial Catalog=VidyaBharathi;Persist Security Info=False;User ID=vidyabharathi;Password=vaps@123;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<PortalContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PortalHub")));            

            services.AddScoped<PortalContext>().AddDbContext<PortalContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PortalHub")));

            services.AddScoped<ProspectusContext>().AddDbContext<ProspectusContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<HRMSContext>().AddDbContext<HRMSContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HRMSServiceHub")));

            services.AddScoped<TTContext>().AddDbContext<TTContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("TimeTableServiceHub")));

            services.AddScoped<FeeGroupContext>().AddDbContext<FeeGroupContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("FeeServiceHub")));

            services.AddScoped<ExamContext>().AddDbContext<ExamContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<LMContext>().AddDbContext<LMContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("LeaveManagementServiceHub")));

            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<FOContext>().AddDbContext<FOContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("FrontOfficeHub")));

            services.AddScoped<COEContext>().AddDbContext<COEContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CoeServiceHub")));

            services.AddScoped<PortalContext>().AddDbContext<PortalContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PortalHub")));

            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeServiceHub")));

            services.AddScoped<HomeworkstaffUploadReportInterface, HomeworkstaffUploadReportIMPL>();
            services.AddScoped<Fee_PaymentManagementInterface, Fee_PaymentManagementImpl>();
            services.AddScoped<HomeworkUploadInterface, HomeworkUploadImpl>();
            services.AddScoped<Interaction_Delete_ReportInterface, Interaction_Delete_Report_IMPL>();
            services.AddScoped<JSHSPortal_StudentReportsInterface, JSHSProtal_StudentReportsImpl>();
            services.AddScoped<StudentCompliantsViewInterface, StudentCompliantsViewImpl>();
            services.AddScoped<EmployeePtalInterface, EmployeePtalImpl>();
            services.AddScoped<EmployeeTTInterface, EmployeeTTImpl>();
            services.AddScoped<EmployeeStudentSearchInterface, EmployeeStudentSearchImpl>();
            services.AddScoped<EmployeeSalaryDetailsInterface, EmployeeSalaryDetailsImpl>();
            services.AddScoped<EmployeeStudentDetailsInterface, EmployeeStudentDetailsImpl>();
            services.AddScoped<EmployeeLeaveApplyInterface, EmployeeLeaveApplyImpl>();
            services.AddScoped<EmployeeStudentExamResultsInterface, EmployeeStudentExamResultsImpl>();
            services.AddScoped<EmployeePunchAttendenceInterface, EmployeePunchAttendenceImpl>();
            services.AddScoped<EmployeeStudentReportCardInterface, EmployeeStudentReportCardImpl>();
            services.AddScoped<EmployeeStudentAttendenceDetailsInterface, EmployeeStudentAttendenceDetailsImpl>();
            services.AddScoped<Student_TTInterface, Student_TTImpl>();
            services.AddScoped<ParentsSmartCardInterface, ParentsSmartCardImpl>();
            services.AddScoped<LiveMeetingScheduleInterface, LiveMeetingScheduleImpl>();
            services.AddScoped<EmployeeForm12BBInterface, EmployeeForm12BBImpl>();
            services.AddScoped<EmployeeForm16Interface, EmployeeForm16Impl>();
            services.AddScoped<HREmpInvestmentInterface, HREmpInvestmentService>();
            services.AddScoped<HREmpInvestmentSubsectionInterface, HREmpInvestmentSubsectionService>();
            services.AddScoped<HREmpInvestmentotherInterface, HREmpInvestmentotherService>();
            services.AddScoped<StudentKIOSKInterface, StudentKIOSKImpl>();
            services.AddScoped<EmployeePtalKIOSKInterface, EmployeePtalKIOSKImpl>();
            services.AddScoped<IVRM_DocsUploadInterface, IVRM_DocsUploadImpl>();
            services.AddScoped<EmployeeStudentHomeworkInterface, EmployeeStudentHomeworkImpl>();
            services.AddScoped<NoticeBoardInterface, NoticeBoardImpl>();
            services.AddScoped<IVRM_ClassWorkInterface, IVRM_ClassWorkImpl>();
            services.AddScoped<IVRM_PushNotificationInterface, IVRM_PushNotificationImpl>();
            services.AddScoped<UpdateRequestInterface, UpdateRequestImpl>();
            services.AddScoped<StudentDashboardInterface, StudentDashboardImpl>();
            services.AddScoped<FeeDetailsInterface, FeeDetailsImpl>();
            services.AddScoped<CumulativeFeeAnalysisInterface, CumulativeFeeAnalysisImpl>();
            services.AddScoped<FeeReceiptInterface, FeeReceiptImpl>();
            services.AddScoped<CumulativeSubjectInterface, CumulativeSubjectImpl>();
            services.AddScoped<AttendanceDetailsInterface, AttendanceDetailsImpl>();
            services.AddScoped<COEInterface, COEImpl>();
            services.AddScoped<ExamReportInterface, ExamReportImpl>();
            services.AddScoped<CovidTestUploadInterface, CovidTestUploadIMPL>();
            services.AddScoped<Master_CovidVaccineTypeInterface, Master_CovidVaccineTypeIMPL>();
            services.AddScoped<CovidVaccinationInterface, CovidVaccinationIMPL>();

            //Praveen Gouda 19/08/2023
            services.AddScoped<Employee_MedicalRecordInterface, Employee_MedicalRecordImpl>();

            // Principal
            services.AddScoped<AttendanceStaffWiseInterface, AttendanceStaffWiseImpl>();
            services.AddScoped<PrincipalDefaulterFeeInterface, PrincipalDefaulterFeeImpl>();
            services.AddScoped<PrincipalDashboardInterface, PrincipalDashboardImpl>();
            services.AddScoped<SendSMSInterface, SendSMSImpl>();
            services.AddScoped<StudentSearchInterface, StudentSearchImpl>();
            services.AddScoped<TimeTableInterface, TimeTableImpl>();
            services.AddScoped<SalaryDetailsInterface, SalaryDetailsImpl>();
            services.AddScoped<LateInDetailsInterface, LateInDetailsImpl>();
            services.AddScoped<CareerReportInterface, CareerReportImpl>();
            services.AddScoped<PushNotifyInterface, PushNotifyImpl>();
            //End  Principal

            //chairman
            services.AddScoped<ChairmanDashboardInterface, ChairmanDashboardImpl>();
            services.AddScoped<HomeSchoolAdmInterface, HomeSchoolAdmImpl>();
            services.AddScoped<ADMClassSectionStrengthInterface, ADMClassSectionStrengthImpl>();
            services.AddScoped<ADMAttendenceInterface, ADMAttendenceImpl>();
            services.AddScoped<FEESOverAllStatusSchoolInterface, FEESOverAllStatusSchoolImpl>();
            services.AddScoped<FEESGroupHeadWiseDetailsSchoolInterface, FEESGroupHeadWiseDetailsSchoolImpl>();
            services.AddScoped<FEESTodayCollectionInterface, FEESTodayCollectionImpl>();
            services.AddScoped<PAYCAREEmployeeDetailsInterface, PAYCAREEmployeeDetailsImpl>();
            services.AddScoped<PAYCARELeaveDetailsInterface, PAYCARELeaveDetailsImpl>();
            services.AddScoped<Ch_EmployeeSalaryDetailsInterface, Ch_EmployeeSalaryDetailsImpl>();
            services.AddScoped<ADMCasteStrengthInterface, ADMCasteStrengthImpl>();
            services.AddScoped<ExamHomeInterface, ExamHomeImpl>();
            services.AddScoped<ExmSectionPerformInterface, ExmSectionPerformImpl>();
            services.AddScoped<ExamToppersListInterface, ExamToppersListImpl>();
            services.AddScoped<Ch_DatewiseAttendanceInterface, Ch_DatewiseAttendanceImpl>();
            services.AddScoped<Ch_LopInterface, Ch_LopImpl>();
            services.AddScoped<StudentBuspassFormInterface, StudentBuspassFormImpl>();
            services.AddScoped<HOD_PRINCInterface, HOD_PRINCImpl>();
            services.AddScoped<Ch_feedbackInterface, Ch_feedbackImpl>();
            services.AddScoped<AllFeeCollectionInterface, AllFeeCollectionImpl>();
            services.AddScoped<ModewiseFeeCollectionInterface, ModewiseFeeCollectionImpl>();
            services.AddScoped<ChairmanloginCountInterface, ChairmanloginCountImpl>();
            services.AddScoped<ChairmanFeeAudcntInterface, ChairmanFeeAudcntImpl>();
            services.AddScoped<NewChairmanDashboardInterface, NewChairmanDashboardImpl>();
            services.AddScoped<InstituteWiseFeeCollectionInterface, InstituteWiseFeeCollectionImpl>();





            //IVRS
            services.AddScoped<IVRSInterface, IVRSImpl>();
            services.AddScoped<IVRSMasterLanguagesInterface, IVRSMasterLanguagesImpl>();
            services.AddScoped<InOutCallsReportInterface, InOutCallsReportImpl>();
            services.AddScoped<IVRSRechargeInterface, IVRSRechargeIMPL>();
            services.AddScoped<IVRSOBDInterface, IVRSOBDIMPL>();

            //IVRM
            services.AddScoped<IVRM_InteractionsInterface, IVRM_InteractionsImpl>();
            services.AddScoped<IVRM_HODMappingInterface, IVRM_HODMappingImpl>();
            services.AddScoped<IVRM_PrincipalMappingInterface, IVRM_PrincipalMappingImpl>();
            services.AddScoped<OnlineLeaveAppInterface, OnlineLeaveAppImpl>();
            services.AddScoped<TransferCertificateInterface, TransferCertificateImpl>();
            services.AddScoped<SalarySlipInterface, SalarySlipImpl>();

            // ==============================Vikasa Reports
            services.AddScoped<VProgressReportExamInterface, VProgressReportExamImpl>();
            services.AddScoped<StudentProgressCardReportInterface, StudentProgressCardReportImpl>();

            // ==============================Portal MonthEnd Report
            services.AddScoped<PortalMonthEndReportInterface, PortalMonthEndReportImpl>();
            services.AddScoped<StudentFeedbackFormInterface, StudentFeedbackFormImpl>();
            services.AddScoped<IVRM_GalleryInterface, IVRM_GalleryImpl>();
            services.AddScoped<HWMonthEndReportInterface, HWMonthEndReportImpl>();
            

            //=================HOD PORTAL
            services.AddScoped<StudentHODInterface, StudentHODImpl>();
            services.AddScoped<HODAttendanceDetailsInterface, HODAttendanceDetailsImpl>();
            services.AddScoped<StudentRoueLocationUpdateInterface, StudentRoueLocationUpdateImpl>();
            services.AddScoped<SalarySlipInterface, SalarySlipImpl>();
            services.AddScoped<HODStudentStrengthInterface, HODStudentStrengthImpl>();
            services.AddScoped<HODFeesCollectionInterface, HODFeesCollectionImpl>();
            services.AddScoped<HODStudentSearchInterface, HODStudentSearchImpl>();
            services.AddScoped<HODExamTopperInterface, HODExamTopperImpl>();
            services.AddScoped<HODExamSectionPerformanceInterface, HODExamSectionPerformanceImpl>();
            services.AddScoped<HODPaycareStaffDetailsInterface, HODPaycareStaffDetailsImpl>();
            services.AddScoped<SmsEmailDetailsInterface, SmsEmailDetailsImpl>();
            services.AddScoped<StudentHallticketInterface, StudentHallticketImpl>();
            services.AddScoped<SmsEmailReportInterFace, SmsEmailReportIMPL>();
            services.AddScoped<EmployeeProfileUpdateApprovalInterface, EmployeeProfileUpdateApprovalImpl>();



            //Mobile API
            services.AddScoped<AdmissionInterface, AdmissionImpl>();            services.AddScoped<FeesInterface, FeesImpl>();            services.AddScoped<ExamInterface, ExamImpl>();            services.AddScoped<HRMSInterface, HRMSImpl>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<EmployeeDashboardDTO, EmployeeStudentExamResultDMO>().ReverseMap();
                config.CreateMap<StudentDashboardDTO, AcademicYear>().ReverseMap();
                // config.CreateMap<StudentDashboardDTO, Fee_Student_StatusDMO>().ReverseMap();
                config.CreateMap<StudentDashboardDTO, AcademicYear>().ReverseMap();
                config.CreateMap<ExamDTO, School_M_Class>().ReverseMap();
                config.CreateMap<ExamDTO, School_M_Section>().ReverseMap();
                config.CreateMap<ExamDTO, School_Adm_Y_StudentDMO>().ReverseMap();
                config.CreateMap<ExamDTO, StudentMappingDMO>().ReverseMap();
                config.CreateMap<ExamDTO, IVRM_Master_SubjectsDMO>().ReverseMap();
                config.CreateMap<ExamDTO, ExmStudentMarksProcessSubjectwiseDMO>().ReverseMap();
                config.CreateMap<ExamDTO, exammasterDMO>().ReverseMap();
                config.CreateMap<StudentBuspassFormDTO, Adm_Student_Transport_ApplicationDMO>().ReverseMap();
                config.CreateMap<ParentSmartCardDTO, StudentDetailsupdateDMO>().ReverseMap();
                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();
                config.CreateMap<MasterEmployee, MasterEmployeeDTO>().ReverseMap();
                config.CreateMap<HR_Configuration, HR_ConfigurationDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Salary, HR_Employee_SalaryDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Investment, EmployeeInvestmentDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Investment, EmployeeInvestmentDTO>().ReverseMap();
                config.CreateMap<HREmpInvestmentotherService, HREmpInvestmentotherInterface>().ReverseMap();
                config.CreateMap<HR_Employee_Subsection_Investment, EmployeeInvestmentSubsectionDTO>().ReverseMap();

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
            loggerFactory.AddFile("Logs/PortalHub-{Date}.txt");

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



