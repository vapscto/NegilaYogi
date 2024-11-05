using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CollegeExamServiceHub.Interfaces;
using CollegeExamServiceHub.Services;
using CollegeExamServiceHub.StudentMentorMapping.Interface;
using CollegeExamServiceHub.StudentMentorMapping.Services;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;

namespace CollegeExamServiceHub
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
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            // var sqlConnectionString = "Data Source=stjameskolkata.database.windows.net,1433;Initial Catalog=Stjames;Persist Security Info=False;User ID=stjameskolkata;Password = Stjames@123; Connection Timeout = 30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ClgExamContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeExamServiceHub")));

            services.AddScoped<StudentMentorMappingContext>().AddDbContext<StudentMentorMappingContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeExamServiceHub")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

            services.AddScoped<ClgExamMasterInterface, ClgExamMasterImpl>();
            services.AddScoped<ClgExamMasterGradeInterface, ClgExamMasterGradeImpl>();
            services.AddScoped<ClgMasterSubjectGroupInterface, ClgMasterSubjectGroupImpl>();
            services.AddScoped<ClgSubjectMasterInterface, ClgSubjectMasterImpl>();
            services.AddScoped<ClgSubExamMasterInterface, ClgSubExamMasterImpl>();
            services.AddScoped<ClgmastersubsubjectInterface, ClgmastersubsubjectImpl>();
            services.AddScoped<ClgExamStandardInterface, ClgExamStandardImpl>();
            services.AddScoped<ClgExamSubjectWizardInterface, ClgExamSubjectWizardImpl>();
            services.AddScoped<ClgcoursebranchmappingInterface, ClgcoursebranchmappingIMPL>();
            services.AddScoped<ClgMarksEntryInterface, ClgMarksEntryIMPL>();
            services.AddScoped<ClgStudentMappingInterface, ClgStudentMappingIMPL>();
            services.AddScoped<ClgCumulativeReportInterface, ClgCumulativeReportIMPL>();
            services.AddScoped<ClgExammarksCalculationInterface, ClgExammarksCalculationIMPL>();
            services.AddScoped<ClgExamMonthEndReportInterface, ClgExamMonthEndReportImpl>();
            services.AddScoped<CollegeRuleSettingsInterface, CollegeRuleSettingsImpl>();
            services.AddScoped<CollegeBMCPUProgresscardReportInterface, CollegeBMCPUProgresscardReportImpl>();
            services.AddScoped<CollegeCumulativeAvgBestReportInterface, CollegeCumulativeAvgBestReportImpl>();
            services.AddScoped<CollegedepartmentcoursebranchmappingInterface, CollegedepartmentcoursebranchmappingImpl>();
            services.AddScoped<CollegestudentmentormappingInterface, CollegestudentmentormappingImpl>();
            services.AddScoped<CollegeMarksEntryInterface, CollegeMarksEntryImpl>();
            services.AddScoped<CollegeExamGeneralReportInterface, CollegeExamGeneralReportImpl>();
            services.AddScoped<HallTicketGenerationCollege, HallTicketGenerationCollegeIMPL>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Exm_Col_Student_MarksDMO, ClgMarksEntryDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Student_Marks_SubSubjectDMO, ClgMarksEntryDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO, Exm_Yrly_Sch_Exams_Subwise_SubExamsDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yrly_Sch_Exams_SubwiseDMO, ClgSubjectWizardDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yearly_Scheme_ExamsDMO, ClgSubjectWizardDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Studentwise_SubjectsDMO, Exm_Col_Studentwise_SubjectsDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yearly_SchemeDMO, Exm_Col_CourseBranchDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yearly_Scheme_GroupDMO, Exm_Col_CourseBranchDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yearly_Scheme_Group_SubjectsDMO, Exm_Col_CourseBranchDTO>().ReverseMap();
                config.CreateMap<Exm_Yearly_Category_ExamsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_SubwiseDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO, ExamSubjectMappingDTO>().ReverseMap();
                config.CreateMap<Exm_ConfigurationDMO, ExamStandardDTO>().ReverseMap();
                config.CreateMap<mastersubsubjectDMO, mastersubsubjectDTO>().ReverseMap();
                config.CreateMap<mastersubexamDMO, mastersubexamDTO>().ReverseMap();
                config.CreateMap<exammasterDMO, exammasterDTO>().ReverseMap();
                config.CreateMap<Exm_Master_GradeDMO, MasterExamGradeDTO>().ReverseMap();
                config.CreateMap<Exm_Master_GroupDMO, MasterSubjectGroupDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Master_Group_SubjectsDMO, Exm_Col_Master_Group_SubjectsDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Master_Group_SubjectsDMO, MasterSubjectGroupDTO>().ReverseMap();
                config.CreateMap<IVRM_School_Master_SubjectsDMO, subjectmasterDTO>().ReverseMap();
                config.CreateMap<Exm_Category_ClassDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<Exm_Master_CategoryDMO, exammastercategoryDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yrly_Sch_Exams_SubwiseDMO, Exm_Yrly_Sch_Exams_SubwiseDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO, Exm_Yrly_Sch_Exams_Subwise_SubSubjectsDTO>().ReverseMap();
                config.CreateMap<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO, ClgSubjectWizardDTO>().ReverseMap();
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
            // loggerFactory.AddFile("Logs/CollegeExamServiceHub-{Date}.txt");
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
