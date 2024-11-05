using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExamServiceHub.com.vaps.Interfaces;
using ExamServiceHub.com.vaps.Services;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.AspNetCore.HttpOverrides;
using DomainModel.Model.com.vapstech.Exam;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using ExamServiceHub.com.vaps.StudentMentor.Interface;
using ExamServiceHub.com.vaps.StudentMentor.Services;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;

namespace ExamServiceHub
{
    public class Startup
    {
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
           // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase_CLG;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //  var sqlConnectionString = "Data Source = kusumavaps.database.windows.net,1433; Initial Catalog = vapskusuma; Persist Security Info = False; User ID = vapskusuma; Password = @zure2021V@p$EcaMpU$; Connection Timeout = 30;";
            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //   var sqlConnectionString = "Data Source = hutchingsserver.database.windows.net,1433; Initial Catalog = Hutchings; Persist Security Info = False; User ID = hutchingsadmin; Password = Hutchpune@123; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=Demo_Hutchings;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            // var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
            // var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            // var sqlConnectionString = "Data Source = dcampus.database.windows.net,1433; Initial Catalog = DCAMPUS; Persist Security Info = False; User ID = decampus; Password = Digit@lc@mpu$@1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            // var sqlConnectionString = "Data Source=jesussacredheart.database.windows.net,1433;Initial Catalog=JSHS;Persist Security Info=False;User ID=jesussacredheart;Password=jesus@321;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=demovaps.database.windows.net,1433;Initial Catalog=IVRMInhouse;Persist Security Info=False;User ID=demovaps;Password=vaps@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;";
            // var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=JSVALUMNI;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<ExamContext>().AddDbContext<ExamContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<exammastercategoryContext>().AddDbContext<exammastercategoryContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<exammasterContext>().AddDbContext<exammasterContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<mastersubexamContext>().AddDbContext<mastersubexamContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<mastersubsubjectContext>().AddDbContext<mastersubsubjectContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<subjectmasterContext>().AddDbContext<subjectmasterContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<ExamTimeTableContext>().AddDbContext<ExamTimeTableContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<MasterSubjectContext>().AddDbContext<MasterSubjectContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("DataAccessMsSqlServerProvider")));

            services.AddScoped<LMContext>().AddDbContext<LMContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<LessonplannerContext>().AddDbContext<LessonplannerContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<StudentMentorContext>().AddDbContext<StudentMentorContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("ExamServiceHub")));

            services.AddScoped<subjectmasterInterface, subjectmasterImpl>();
            services.AddScoped<ExamStandardInterface, ExamStandardImpl>();
            services.AddScoped<exammastercategoryInterface, exammastercategoryImpl>();
            services.AddScoped<MasterExamGradeInterface, MasterExamGradeImpl>();
            services.AddScoped<MasterSubjectGroupInterface, MasterSubjectGroupImpl>();
            services.AddScoped<CategorySubjectMappingInterface, CategorySubjectMappingImpl>();
            services.AddScoped<ExamSubjectMappingInterface, ExamMarksprocessconditionImpl>();
            services.AddScoped<PromotionSettingInterface, PromotionSettingImpl>();
            services.AddScoped<mastersubsubjectInterface, mastersubsubjectImpl>();
            services.AddScoped<StudentMappingInterface, StudentMappingImpl>();
            services.AddScoped<exammasterInterface, exammasterImpl>();
            services.AddScoped<mastersubexamInterface, mastersubexamImpl>();
            services.AddScoped<ExamLoginPrivilagesInterface, ExamLoginPrivilagesImpl>();
            services.AddScoped<CumulativeReportInterface, CumulativeReportImpl>();
            services.AddScoped<ProgressCardReportInterface, ProgressCardReportImpl>();
            services.AddScoped<BaldwinAllReportInterface, BaldwinAllReportImpl>();
            services.AddScoped<ExamGraphInterface, ExamGraphImpl>();
            services.AddScoped<ExamImportInterface, ExamImportImpl>();
            services.AddScoped<MarksEntryInterface, MarksEntryImpl>();
            services.AddScoped<ExamCalculationInterface, ExamCalculationImpl>();
            services.AddScoped<ExamsubjectGroupMappingInterface, ExamsubjectGroupMappingImpl>();
            services.AddScoped<MarksEntry_SSSEInterface, MarksEntry_SSSEImpl>();
            services.AddScoped<MarksEntry_SSInterface, MarksEntry_SSImpl>();
            services.AddScoped<MarksEntry_SEInterface, MarksEntry_SEImpl>();
            services.AddScoped<MarksEntry_SInterface, MarksEntry_SImpl>();
            services.AddScoped<ExamCalculation_SSSEInterface, ExamCalculation_SSSEImpl>();
            services.AddScoped<Baldwin_Final_P_ReportInterface, Baldwin_Final_P_ReportImpl>();
            services.AddScoped<Baldwin_Final_P_C_ReportInterface, Baldwin_Final_P_C_ReportImpl>();
            services.AddScoped<MarksEntryHHSInterface, MarksEntryHHSImpl>();
            services.AddScoped<Baldwin_Subj_G_F_ReportInterface, Baldwin_Subj_G_F_ReportImpl>();
            services.AddScoped<PromotionCalculationInterface, PromotionCalculationImpl>();
            services.AddScoped<HHSAllReportInterface, HHSAllReportImpl>();
            services.AddScoped<ExamTTsessionmasterInterface, ExamTTsessionmasterImpl>();
            services.AddScoped<ExamTTTransactionInterface, ExamTTTransactionImpl>();
            services.AddScoped<exammasterPersonalityInterface, exammasterpersonalityImpl>();
            services.AddScoped<exammasterCoCurricularInterface, exammasterCoCurricularImpl>();
            services.AddScoped<exammasterRemarkInterface, exammasterRemarkImpl>();
            services.AddScoped<exammasterPointInterface, exammasterPointImpl>();
            services.AddScoped<MasterLifeSkillInterface, MasterLifeSkillImpl>();
            services.AddScoped<MasterLifeSkillAreaInterface, MasterLifeSkillAreaImpl>();
            services.AddScoped<MasterLifeSkillAreaMappingInterface, MasterLifeSkillAreaMappingImpl>();
            services.AddScoped<MasterScholasticActivityInterface, MasterScholasticActivityImpl>();
            services.AddScoped<CoScholasticActivityAreasInterface, CoScholasticActivityAreasImpl>();
            services.AddScoped<ExamTermAndExamMappingInterface, ExamTermAndExamMappingImpl>();
            services.AddScoped<VikasaLUInterface, VikasaLUImpl>();
            services.AddScoped<ExamLoginPrivilegesReportInterface, ExamLoginPrivilegesReportImpl>();
            services.AddScoped<GroupwiseSubListReportInterface, GroupwiseSubListReportImpl>();
            services.AddScoped<Baldwin_Final_P_ReportBGHSInterface, Baldwin_Final_P_ReportBGHSImpl>();
            services.AddScoped<Baldwin_Electives_ReportInterface, Baldwin_Electives_ReportImpl>();
            services.AddScoped<Promotion_Marks_UpdateInterface, Promotion_Marks_UpdateImpl>();
            services.AddScoped<ExamPassFailConditionInterface, ExamPassFailConditionImpl>();
            services.AddScoped<VikasaSubjectwiseCumulativeReportInterface, VikasaSubjectwiseCumulativeReportImpl>();
            services.AddScoped<VikasaSchoolExamWiseCumulativeReportInterface, VikasaSchoolExamWiseCumulativeReportImpl>();
            services.AddScoped<VikasaSchoolTermWiseSubjectCumulativeReportInterface, VikasaSchoolTermWiseSubjectCumulativeReportImpl>();
            services.AddScoped<VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportInterface, VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportImpl>();
            services.AddScoped<VikasaAssessment2ReportInterface, VikasaAssessment2ReportImpl>();
            services.AddScoped<VikasaProgressReportExamInterface, VikasaProgressReportExamImpl>();
            services.AddScoped<VikasaMinorPregressExamReportInterface, VikasaMinorPregressExamReportImpl>();
            services.AddScoped<CoScholasticActivityInterface, CoScholasticActivityImpl>();
            services.AddScoped<StudentTransactionInterface, StudentTransactionImpl>();
            services.AddScoped<ExamCategoryReportInterface, ExamCategoryReportImpl>();
            services.AddScoped<GradeSlabReportInterface, GradeSlabReportImpl>();
            services.AddScoped<MeritListInterface, MeritListImpl>();
            services.AddScoped<PassFailReportInterface, PassFailReportImpl>();
            services.AddScoped<StudentPerformanceReportInterface, StudentPerformanceReportImpl>();
            services.AddScoped<PercentagewiseDetailsReportInterface, PercentagewiseDetailsReportImpl>();
            services.AddScoped<ExamMonthEndReportInterface, ExamMonthEndReportImpl>();
            services.AddScoped<ClassSectionAvgInterface, ClassSectionAvgImpl>();
            services.AddScoped<ToppersListReportInterface, ToppersListReportImpl>();
            services.AddScoped<HHSMIDFINALCumReportInterface, HHSMIDFINALCumReportImpl>();
            services.AddScoped<HallTicketGenerationInterface, HallTicketGenerationImpl>();
            services.AddScoped<General_SendSMSInterface, General_SendSMSImpl>();
            services.AddScoped<ExamTTTransSettingsInterface, ExamTTTransSettingsImpl>();
            services.AddScoped<ExamTTSmsEmailInterface, ExamTTSmsEmailImpl>();
            services.AddScoped<VikasaHallTicketReportInterface, VikasaHallTicketReportImpl>();
            services.AddScoped<ExamPromotionRemarksInterface, ExamPromotionRemarksImpl>();
            services.AddScoped<SNSPROGRESSCARDReportInterface, SNSPROGRESSCARDReportImpl>();
            services.AddScoped<BBHSCUMReportInterface, BBHSCUMReportImpl>();
            services.AddScoped<VikasaHallTicketReportInterface, VikasaHallTicketReportImpl>();
            services.AddScoped<BaldwinPUReportInterface, BaldwinPUReportImp>();
            services.AddScoped<VikasaFinalClasswisecumulativeInterface, VikasaFinalClasswisecumulativeImpl>();
            services.AddScoped<HHSReport_5to7Interface, HHSReport_5to7Impl>();
            services.AddScoped<HHSReport_10thInterface, HHSReport_10thImpl>();        
            services.AddScoped<PromotionSmsAndEmailDetailsReportInterface, PromotionSmsAndEmailDetailsReportImpl>();
            services.AddScoped<HHSReport_PreNursInterface, HHSReport_PreNursImpl>();
            services.AddScoped<SchoolstudentmentormappingInterface, SchoolstudentmentormappingImpl>();
            services.AddScoped<ExamPromotionReportInterface, ExamPromotionReportImpl>();
            services.AddScoped<ExamMarksprocessconditionInterface, ExamMarksProcessConditionsImpl>();
            services.AddScoped<MaldaProgressReportExamInterface, MaldaProgressReportExamImpl>();
            services.AddScoped<ExamWiseTermReportInterface, ExamWiseTermReportImpl>();
            services.AddScoped<JSHSExamReportsInterface, JSHSExamReportsImpl>();
            services.AddScoped<MarksEntryReportInterface, MarksEntryReportImpl>();
            services.AddScoped<ExamTermWiseRemarksInterface, ExamTermWiseRemarksImpl>();
            services.AddScoped<PromotionReportDetailsInterface, PromotionReportDetailsImpl>();
            services.AddScoped<ExamWiseSMSAndEmailInterface, ExamWiseSMSAndEmailImpl>();
            services.AddScoped<ExamWiseRemarksReportInterface, ExamWiseRemarksReportImpl>();
            services.AddScoped<MasterExamSlabInterface, MasterExamSlabImpl>();
            services.AddScoped<SlabWiseExamReportinterface, SlabWiseExamReportImpl>();

            services.AddScoped<MarksEntry_Ent_ReportInterface, MarksEntry_Ent_ReportImpl>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Exm_ConfigurationDMO, ExamStandardDTO>().ReverseMap();
                config.CreateMap<exammasterDMO, exammasterDTO>().ReverseMap();
                config.CreateMap<mastersubexamDMO, mastersubexamDTO>().ReverseMap();
                config.CreateMap<subjectmasterDMO, subjectmasterDTO>().ReverseMap();
                config.CreateMap<IVRM_School_Master_SubjectsDMO, subjectmasterDTO>().ReverseMap();
                config.CreateMap<mastersubsubjectDMO, mastersubsubjectDTO>().ReverseMap();
                config.CreateMap<Exm_Login_PrivilegeDMO, Exm_Login_PrivilegeDTO>().ReverseMap();
                config.CreateMap<StudentMappingDMO, StudentMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Category_ClassDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<Exm_Master_CategoryDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<Exm_Master_GradeDMO, MasterExamGradeDTO>().ReverseMap();
                config.CreateMap<Exm_Master_Grade_DetailsDMO, MasterExamGradeDTO>().ReverseMap();
                config.CreateMap<Exm_Master_Grade_DetailsDMO, Exm_Master_Grade_DetailsDTO>().ReverseMap();
                config.CreateMap<Exm_Master_GroupDMO, MasterSubjectGroupDTO>().ReverseMap();
                config.CreateMap<Exm_Master_Group_SubjectsDMO, MasterSubjectGroupDTO>().ReverseMap();
                config.CreateMap<Exm_Yearly_CategoryDMO, CategorySubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yearly_Category_GroupDMO, CategorySubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yearly_Category_Group_SubjectsDMO, CategorySubjectMappingDTO>().ReverseMap();
                config.CreateMap<StudentMappingDMO, CategorySubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yearly_Category_ExamsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_SubwiseDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_SubwiseDMO, Exm_Yrly_Cat_Exams_SubwiseDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO, Exm_Yrly_Cat_Exams_Subwise_SubExamsDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO, Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDTO>().ReverseMap();
                config.CreateMap<Exm_M_PromotionDMO, PromotionSettingDTO>().ReverseMap();
                config.CreateMap<Exm_M_Promotion_SubjectsDMO, PromotionSettingDTO>().ReverseMap();
                config.CreateMap<Exm_M_Prom_Subj_GroupDMO, PromotionSettingDTO>().ReverseMap();
                config.CreateMap<Exm_M_Prom_Subj_Group_ExamsDMO, PromotionSettingDTO>().ReverseMap();
                config.CreateMap<Exm_M_Promotion_SubjectsDMO, Exm_M_Promotion_SubjectsDTO>().ReverseMap();
                config.CreateMap<Exm_M_Prom_Subj_GroupDMO, Exm_M_Prom_Subj_GroupDTO>().ReverseMap();
                config.CreateMap<ExamMarksDMO, ExamMarksDTO>().ReverseMap();
                config.CreateMap<ExamMarksDMO, ExamImportStudentDTO>().ReverseMap();
                config.CreateMap<ExamsubjectGroupMappingDMO, ExamsubjectGroupMappingDTo>().ReverseMap();
                config.CreateMap<Exm_TT_M_SessionDMO, ExamTTsessionmasterDTO>().ReverseMap();
                config.CreateMap<exammasterpersonalityDMO, exammasterpersonalityDTO>().ReverseMap();
                config.CreateMap<exammasterCoCulrricularDMO, exammasterCoCurricularDTO>().ReverseMap();
                config.CreateMap<exammasterRemarkDMO, exammasterRemarkDTO>().ReverseMap();
                config.CreateMap<exammasterPointDMO, exammasterpointDTO>().ReverseMap();
                config.CreateMap<CCE_M_CoScholasticActivitiesDMO, MasterScholasticActivityDTO>().ReverseMap();
                config.CreateMap<CCE_Master_Life_Skill_AreasDMO, MasterLifeSkillAreaDTO>().ReverseMap();
                config.CreateMap<CCE_M_Scholastic_AreasDMO, CoScholasticActivityAreasDTO>().ReverseMap();
                config.CreateMap<Exm_CCE_ActivitiesDMO, CoScholasticActivityDTO>().ReverseMap();
                config.CreateMap<EXM_CCE_Activities_AREADMO, CoScholasticActivityDTO>().ReverseMap();
                config.CreateMap<Exm_PassFailRank_ConditionDMO, ExamPassFailConditionDTO>().ReverseMap();
                config.CreateMap<Exm_CCE_Activities_TransactionDMO, StudentTransactionDTO>().ReverseMap();
                config.CreateMap<Exm_CCE_SKILLS_TransactionDMO, StudentTransactionDTO>().ReverseMap();
                config.CreateMap<CumulativeReportDTO, ExpandoObject>().ReverseMap();
                config.CreateMap<ExamPromotionRemarksDMO, ExamPromotionRemarksDTO>().ReverseMap();
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
            loggerFactory.AddFile("Logs/examhub-{Date}.txt");

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
