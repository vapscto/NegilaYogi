using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using NaacServiceHub.FeedBack.Interface;
using NaacServiceHub.FeedBack.Services;
using NaacServiceHub.OnlineProgram.Interfaces;
using NaacServiceHub.OnlineProgram.Impl;
using DataAccessMsSqlServerProvider.FeedBack;
using NaacServiceHub.com.vaps.LessonPlanner.Interface;
using NaacServiceHub.com.vaps.LessonPlanner.Services;
using NaacServiceHub.LessonPlanner.Interface;
using NaacServiceHub.LessonPlanner.Services;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Admission.Services;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using NaacServiceHub.Documents.Interface;
using NaacServiceHub.Documents.Services;
using NaacServiceHub.HRMS.Interface;
using NaacServiceHub.HRMS.Services;
using DataAccessMsSqlServerProvider.HRMS;
using NaacServiceHub.Reports.Interface;
using NaacServiceHub.Reports.Services;
using NaacServiceHub.Common.Interface;
using NaacServiceHub.Common.Service;
using NaacServiceHub.Admission.Interface.Criteria7;
using NaacServiceHub.Admission.Services.Criteria7;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using NaacServiceHub.Admission.Interface.Criteria8;
using NaacServiceHub.Admission.Services.Criteria8;
using DomainModel.Model.HRMS;
using PreadmissionDTOs.HRMS;
using NaacServiceHub.Medical.Interface;
using NaacServiceHub.Medical.Service;
using NaacServiceHub.University.Interface;
using NaacServiceHub.University.Implimentation;
using NaacServiceHub.University.Service;
using NaacServiceHub.OnlineProgram.Interface;
using NaacServiceHub.LP_OnlineExam.Interface;
using NaacServiceHub.LP_OnlineExam.Services;
using NaacServiceHub.Feedback.Interface;
using NaacServiceHub.Feedback.Services;

namespace NaacServiceHub
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

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeServiceHub")));

            services.AddDbContext<FeedBackContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<LessonplannerContext>().AddDbContext<LessonplannerContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<GeneralContext>().AddDbContext<GeneralContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("NaacServiceHub")));

            services.AddScoped<DocumentsContext>().AddDbContext<DocumentsContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("NaacServiceHub")));         

            services.AddScoped<NaacHRMSContext>().AddDbContext<NaacHRMSContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("NaacServiceHub")));

            services.AddScoped<HRMSContext>().AddDbContext<HRMSContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("NaacServiceHub")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            //Feed Back Forms 
            services.AddScoped<FeedBackMasterTypeInterface, FeedBackMasterTypeImpl>();
            services.AddScoped<FeedbackTypeQuestionMappingInterface, FeedbackTypeQuestionMappingImpl>();
            services.AddScoped<FeedbackTransactionInterface, FeedbackTransactionImpl>();
            services.AddScoped<FeedBackReportInterface, FeedBackReportImpl>();
            services.AddScoped<FeedbackSchoolGeneralTransactionInterface, FeedbackSchoolGeneralTransactionImpl>();
            services.AddScoped<AcademicCalenderReportInterface, AcademicCalenderReportImpl>();
            services.AddScoped<FeedBackSchoolReportInterface, FeedBackSchoolReportIMPL>();
            //online program
            services.AddScoped<YearlyProgramInterface, YearlyProgramImpl>();
            services.AddScoped<ProgramDetailsInterface, ProgramDetailsImpl>();
            services.AddScoped<GuestDetailsInterface, GuestDetailsImpl>();
            services.AddScoped<CompletedEventInterface, CompletedEventImpl>();
            services.AddScoped<ConferenceDetailsInterface, ConferenceDetailsImpl>();
            services.AddScoped<ProgramMasterInterface, ProgramMasterImpl>();
            services.AddScoped<OnlineProgramReportInterface, OnlineProgramReportImpl>();

            //Lesson Planner 
            services.AddScoped<MasterSchoolTopicInterface, MasterSchoolTopicImpl>();
            services.AddScoped<SchoolSubjectWithMasterTopicMappingInterface, SchoolSubjectWithMasterTopicMappingImpl>();
            services.AddScoped<SchoolStaffperiodmappingInterface, SchoolStaffperiodmappingImpl>();
            services.AddScoped<SchoolStaffperiodtransactionreportInterface, SchoolStaffperiodtransactionreportImpl>();
            services.AddScoped<SchoolMasterUnitInterface, SchoolMasterUnitImpl>();
            services.AddScoped<CollegeStaffPeriodMappingInterface, CollegeStaffPeriodMappingImpl>();
            services.AddScoped<CollegeStaffperiodtransactionreportInterface, CollegeStaffperiodtransactionreportImpl>();
            services.AddScoped<LMSStudentInterface, LMSStudentImpl>();

            // Admission 
            services.AddScoped<SeatallotmentReportInterface, SeatallotmentReportImpl>();
            services.AddScoped<NaacExtnActivitiesInterface, NaacExtnActivitiesImpl>();
            services.AddScoped<NaacCommiteeInterface, NaacCommiteeImpl>();
            services.AddScoped<NaacActivityInterface, NaacActivityImpl>();
            services.AddScoped<NaacIPRInterface, NaacIPRImpl>();
            services.AddScoped<NAAC_AC_EthicInterface, NAAC_AC_EthicImpl>();
            services.AddScoped<NAAC_AC_AwardsInterface, NAAC_AC_AwardsImpl>();
            services.AddScoped<NaacLinkageInterface, NaacLinkageImpl>();
            services.AddScoped<NaacBudget_414_Interface, NaacBudget_414_Impl>();
            services.AddScoped<Naac_Memberships_423_Interface, Naac_Memberships_423_Impl>();
            services.AddScoped<NAAC_EContent_434_Interface, NAAC_EContent_434_Impl>();
            services.AddScoped<NaacExpenditure424Interface, NaacExpenditure424Impl>();
            services.AddScoped<NaacExpAcaFacility441Interface, NaacExpAcaFacility441Impl>();
            services.AddScoped<Naac_MOUInterface, Naac_MOUImpl>();
            //naac 6
            services.AddScoped<NaacEGovernance623Interface, NaacEGovernance623Impl>();
            services.AddScoped<NaacFinanceSupport632Interface, NaacFinanceSupport632Impl>();
            //Document Upload
            services.AddScoped<NaacDocumentUploadInterface, NaacDocumentUploadImpl>();
            services.AddScoped<NaacConsolidatProcessInterface, NaacConsolidatProcessImpl>();
            services.AddScoped<NaacMarksSlabInterface, NaacMarksSlabImpl>();
            //HRMS
            services.AddScoped<naacHrmsDetailsInterface, naacHrmsDetailsImpl>();
            services.AddScoped<HrmsNAACReportInterface, HrmsNAACReportImpl>();
            services.AddScoped<HrmsConsolidatedReportInterface, HrmsConsolidatedReportImpl>();
            services.AddScoped<naacHrmsDetailsmultifileInterface, naacHrmsDetailsmultifileImpl>();
            //Reports
            services.AddScoped<CurricularAspectsInterface, CurricularAspectsImpl>();
            services.AddScoped<NaacCriteria4ReportInterface, NaacCriteria4ReportImpl>();
            services.AddScoped<NAAC_AC_351_Linkage_ReportInterface, NAAC_AC_351_Linkage_ReportImpl>();
            services.AddScoped<IVRM_BranchSubjectMappingInterface, IVRM_BranchSubjectMappingImpl>();
            services.AddScoped<ProgramIntroduceInterface, ProgramIntroduceImpl>();
            services.AddScoped<StaffParticipationInterface, StaffParticipationImpl>();
            services.AddScoped<StudentParticipationInterface, StudentParticipationImpl>();
            services.AddScoped<Naac_VACInterface, Naac_VACImpl>();
            services.AddScoped<StudentProjectInterface, StudentProjectImpliment>();
            services.AddScoped<DisabilityStudentInterface, DisabilityStudentImpl>();
            services.AddScoped<NaacDocumentUploadReportInterface, NaacDocumentUploadReportImpl>();
            services.AddScoped<NaacActivityInterface, NaacActivityImpl>();
            services.AddScoped<Naac_M_811StudentsEnrolledInProgrammeInterface, Naac_M_811StudentsEnrolledInProgrammeImpl>();
            //shilpa hsu
            services.AddScoped<Naac_HSU_CR6ReportInterface, Naac_HSU_CR6ReportImpl>();

            services.AddScoped<NAAC_HSU_StudentComplaints252Interface, NAAC_HSU_StudentComplaints252Impl>();
            services.AddScoped<NAAC_HSU_InterdisciplinaryProgrammes_123Interface, NAAC_HSU_InterdisciplinaryProgrammes_123Impl>();

            //savita
            services.AddScoped<NAAC_HSU_Course_StaffMapping_122Interface, NAAC_HSU_Course_StaffMapping_122Impl>();
            services.AddScoped<NAAC_HSU_EvaluationRelated_253Interface, NAAC_HSU_EvaluationRelated_253Impl>();


            //naac 3
            services.AddScoped<NaacAward342ReportInterface, NaacAward342ReportImpl>();
            services.AddScoped<NaacMOU352ReportInterface, NaacMOU352ReportImpl>();
            services.AddScoped<NAACCriteria3ReportInterface, NAACCriteria3ReportImpl>();
            services.AddScoped<UC_312_TeachersResearchInterface, UC_312_TeachersResearchImpl>();
            services.AddScoped<MC_314_ResearchAssociatesInterface, MC_314_ResearchAssociatesImpl>();
            services.AddScoped<MC_342_HRIncentivesInterface, MC_342_HRIncentivesImpl>();
            services.AddScoped<MC_343_TechnologyTransferredInterface, MC_343_TechnologyTransferredImpl>();
            services.AddScoped<HSU_341_EthicsInterface, HSU_341_EthicsImpl>();
            services.AddScoped<HSU_323_ResearchProjectsRatioInterface, HSU_323_ResearchProjectsRatioImpl>();
            services.AddScoped<HSU_362_ExtensionActivitiesInterface, HSU_362_ExtensionActivitiesImpl>();
            services.AddScoped<HSU_352_RevenueGeneratedInterface, HSU_352_RevenueGeneratedImpl>();
            services.AddScoped<HSU_334_CampusStartUpsInterface, HSU_334_CampusStartUpsImpl>();
            services.AddScoped<NAAC_MC_312_TeachersResearchInterface, NAAC_MC_312_TeachersResearchImpl>();
            services.AddScoped<NAAC_HSU_323_ResearchProjectsRatioInterface, NAAC_HSU_323_ResearchProjectsRatioImpl>();
            services.AddScoped<HSU_316_Dept_AwardsInterface, HSU_316_Dept_AwardsImpl>();
            services.AddScoped<HSU_348_BibliometricPublicationsInterface, HSU_348_BibliometricPublicationsImpl>();
            services.AddScoped<HSU_349_HindexInterface, HSU_349_HindexImpl>();
            services.AddScoped<NAAC_MC_351MasterInterface, NAAC_MC_351MasterImpl>();

            // criteria 6
            services.AddScoped<NAAC_AC_633_AdmTrainingInterface, NAAC_AC_633_AdmTrainingImpl>();
            services.AddScoped<NAAC_AC_634_DevProgramsInterface, NAAC_AC_634_DevProgramsImpl>();
            services.AddScoped<NAAC_AC_642_FundsInterface, NAAC_AC_642_FundsImpl>();
            services.AddScoped<NAAC_AC_653_IQACInterface, NAAC_AC_653_IQACImpl>();
            services.AddScoped<NAAC_AC_654_QualityAssuranceInterface, NAAC_AC_654_QualityAssuranceImpl>();
            services.AddScoped<NAAC_Criteria_6_ReportInterface, NAAC_Criteria_6_ReportImpl>();

            //CRITERIA7
            services.AddScoped<GenderEquityInterface, GenderEquityImpl>();
            services.AddScoped<MasterCycleYearMappingInterface, MasterCycleYearMappingImpl>();
            services.AddScoped<Naac_ICTInterface, Naac_ICTImpl>();
            services.AddScoped<AlternateEnergyInterface, AlternateEnergyImpl>();
            services.AddScoped<WasteManagementInterface, WasteManagementImpl>();
            services.AddScoped<LEDBulbsInterface, LEDBulbsImpl>();
            services.AddScoped<DifferentlyAbledInterface, DifferentlyAbledImpl>();
            services.AddScoped<LocalCommunityInterface, LocalCommunityImpl>();
            services.AddScoped<LocationalAdvtgInterface, LocationalAdvtgImpl>();
            services.AddScoped<HumanValuesInterface, HumanValuesImpl>();
            services.AddScoped<ProfessionalEthicsInterface, ProfessionalEthicsImpl>();
            services.AddScoped<StatutoryBodiesInterface, StatutoryBodiesImpl>();
            services.AddScoped<UniversalValuesInterface, UniversalValuesImpl>();
            services.AddScoped<NAAC711GenderEquityReportInterface, NAAC711GenderEquityReportImpl>();
            services.AddScoped<NAAC_User_PrivilegesInterface, NAAC_User_PrivilegesImpl>();
            services.AddScoped<NaacCodeOfCoduct7112Interface, NaacCodeOfCoduct7112Impl>();
            services.AddScoped<NaacCoreValues7113Interface, NaacCoreValues7113Impl>();
            services.AddScoped<AuditOnEnvironmentInterface, AuditOnEnvironmentImpl>();


            // shilpa cr3 master page
            services.AddScoped<NAAC_HSU_345_TeacherResearchPapersInterface, NAAC_HSU_345_TeacherResearchPapersImpl>();
            services.AddScoped<HSU_346_EMPApprovedJournalListInterface, HSU_346_EMPApprovedJournalListImpl>();
            //Criteria 8
            services.AddScoped<NAAC_811MC_NEETInterface, NAAC_811MC_NEETImpl>();
            services.AddScoped<MC_819_Accredition_ClinicallabInterface, MC_819_Accredition_ClinicallabImpl>();
            services.AddScoped<DC_8111_ExpenditureInterface, DC_8111_ExpenditureImpl>();
            services.AddScoped<NC_818_EmpCommitteesInterface, NC_818_EmpCommitteesImpl>();

            //Criteria 5
            services.AddScoped<NAACGovtShcrShipInterface, NAACGovtShcrShipImpl>();
            services.AddScoped<NAACInstShcrshipInterface, NAACInstShcrshipImpl>();
            services.AddScoped<NAACEncDevSchemeInterface, NAACEncDevSchemeImpl>();
            services.AddScoped<NAACCompExamsInterface, NAACCompExamsImpl>();
            services.AddScoped<NAACVETInterface, NAACVETImpl>();
            services.AddScoped<NAACGRIInterface, NAACGRIImpl>();
            services.AddScoped<NAACCriteriaFiveReportInterface, NAACCriteriaFiveReportImpl>();
            services.AddScoped<NAACPlacementInterface, NAACPlacementImpl>();
            services.AddScoped<NAACHrEducationInterface, NAACHrEducationImpl>();
            services.AddScoped<NAACQualifyInterface, NAACQualifyImpl>();
            services.AddScoped<NAACSportsInterface, NAACSportsImpl>();
            services.AddScoped<NAACMasterSportsCAInterface, NAACMasterSportsCAImpl>();
            services.AddScoped<NAACAlumniContributionInterface, NAACAlumniContributionImpl>();
            services.AddScoped<NAACAlumniMeetingInterface, NAACAlumniMeetingImpl>();
            services.AddScoped<NAACNonGovShcrshipHsuInterface, NAACNonGovShcrshipHsuImpl>();

            services.AddScoped<NAACGeneralCriteriaInterface, NAACGeneralCriteriaImpl>();
            services.AddScoped<NaacPGDegrees813Interface, NaacPGDegrees813Impl>();
            services.AddScoped<NaacImmunisation8110Interface, NaacImmunisation8110Impl>();

            //Medical
            services.AddScoped<MC_Programs_112Interface, MC_Programs_112Impl>();
            services.AddScoped<NAAC_422_Clinical_LaboratoryInterface, NAAC_422_Clinical_LaboratoryImpl>();
            services.AddScoped<NAAC_MC_VACcommonInterface, NAAC_MC_VACcommonImpl>();
            services.AddScoped<NAAC_MC_436_EContentInterface, NAAC_MC_436_EContentImpl>();
            //University
            services.AddScoped<NAAC_HSU_Accreditation_424Interface, NAAC_HSU_Accreditation_424Impl>();


            //medical naac 4 report shilpa
            services.AddScoped<Naac_MC_CR4_Interface, Naac_MC_CR4_Impl>();
            services.AddScoped<Naac_MC_CR6Interface, Naac_MC_CR6Impl>();
            services.AddScoped<NAAC_MC_443_BandWidth_RangeInterface, NAAC_MC_443_BandWidth_RangeImpl>();
            services.AddScoped<Naac_MC_IctFacility441Interface, Naac_MC_IctFacility441Impl>();
            services.AddScoped<NAAC_MC_423_StuLearningResourceInterface, NAAC_MC_423_StuLearningResourceImpl>();

            //shilpa
            services.AddScoped<NAAC_MC_EmpTrainedDevelopment244Interface, NAAC_MC_EmpTrainedDevelopment244Impl>();
            services.AddScoped<Naac_HSU_CR6ReportInterface, Naac_HSU_CR6ReportImpl>();

            services.AddScoped<Medical_Criteria1ReportsInterface, Medical_Criteria1ReportsImpl>();
            services.AddScoped<Medical_Criteria2ReportsInterface, Medical_Criteria2ReportsImpl>();
            services.AddScoped<Medical_Criteria3ReportsInterface, Medical_Criteria3ReportsImpl>();
            services.AddScoped<MC_121_IntDept_CourseInterface, MC_121_IntDept_CourseImpl>();

            services.AddScoped<HSU_CR1_ReportInterface, HSU_CR1_ReportImpl>();
            services.AddScoped<HSU_CR2_ReportInterface, HSU_CR2_ReportImpl>();
            services.AddScoped<HSU_MasterCR2Interface, HSU_MasterCR2Impl>();           

            services.AddScoped<Naac_HSU_CR4ReportInterface, Naac_HSU_CR4ReportImpl>();

            services.AddScoped<LP_OnlineExamInterface, LP_OnlineExamImpl>();
            services.AddScoped<LP_OnlineStudentExamInterface, LP_OnlineStudentExamImpl>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<NAAC_AC_711_GenderEquityDMO, NAAC_AC_711_GenderEquity_DTO>().ReverseMap();
                config.CreateMap<HR_Employee_ExamDutyDMO, HR_Employee_ExamDutyDTO>().ReverseMap();
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
            loggerFactory.AddFile("Logs/NaacServiceHub-{Date}.txt");
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
