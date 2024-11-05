using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using IssueManager.com.PettyCash.Interface;
using IssueManager.com.PettyCash.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;
using Recruitment.com.vaps.Services;

namespace Recruitement
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
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";


            //var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus_2022-09-07T12-43Z;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddTransient<VMSContext>().AddDbContext<VMSContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("Recruitement")));

            services.AddTransient<FeeGroupContext>().AddDbContext<FeeGroupContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("Recruitement")));

            services.AddTransient<HRMSContext>().AddDbContext<HRMSContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("Recruitement")));

            services.AddTransient<PettyCashContext>().AddDbContext<PettyCashContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("Recruitement")));

            services.AddTransient<HostelContext>().AddDbContext<HostelContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("Recruitement")));

            services.AddScoped<ISM_Client_ProjectInterface, ISM_Client_Project_IMPL>();
            services.AddScoped<AppointmentInterface, AppointmentService>();
            services.AddScoped<AddJobVMSInterface, AddJobVMSService>();
            services.AddTransient<AddJobVMSInterface, AddJobVMSService>();
            services.AddScoped<JobListVMSInterface, JobListVMSService>();
            services.AddScoped<MasterJobInterface, MasterJobService>();
            services.AddScoped<masterPriorityInterface, masterPriorityService>();
            services.AddScoped<masterPositionTypeInterface, masterPositionTypeService>();
            services.AddScoped<masterPositionInterface, masterPositionService>();
            services.AddScoped<JobListHRVMSInterface, JobListHRVMSService>();
            services.AddScoped<MasterLocationInterface, MasterLocationService>();
            services.AddScoped<JobListMDVMSInterface, JobListMDVMSService>();
            
            services.AddScoped<IVRTM_TrainingInterface, IVRTM_TrainingImpl>();


            services.AddScoped<CandidateListVMSInterface, CandidateListVMSService>();
            services.AddScoped<AddCandidateVMSInterface, AddCandidateVMSService>();
            services.AddScoped<CandidateInterviewListVMSInterface, CandidateInterviewListVMSService>();
            services.AddScoped<AddCandidateInterviewVMSInterface, AddCandidateInterviewVMSService>();
            services.AddScoped<AddtoHRMSInterface, AddtoHRMSService>();
            
            services.AddScoped<Induction_Training_Interface, Induction_Training_IMPL>();
            services.AddScoped<Training_Master_Interface, Training_Master_IMPL>();
            services.AddScoped<Training_Feedback_Interface, Training_Feedback>();
            services.AddScoped<Master_External_TrainingTypeInterface, Master_External_TrainingTypeImpl>();
            services.AddScoped<Master_External_TrainingCentersInterface, Master_External_TrainingCentersIMPL>();
            services.AddScoped<External_Training_ApprovalInterface, External_Training_ApprovalImpl>();
            services.AddScoped<External_TrainingInterface, External_TrainingImpl>();
            services.AddScoped<TrainingAuthorizationInterface, TrainingAuthorizationIMPL>();
            services.AddScoped<SummaryreportsInterface, SummaryreportsIMPL>();
            services.AddScoped<staffwisereportInterface, staffwisereportIMPL>();
            services.AddScoped<TrainingtypewisereportInterface, TrainingtypewisereportIMPL>();

            services.AddScoped<Sales_Lead_Master_Interface, Sales_Lead_Master_IMPL>();
            services.AddScoped<Exit_Employee_Interface, Exit_Employee_IMPL>();
            services.AddScoped<Induction_Training_Interface, Induction_Training_IMPL>();
            services.AddScoped<Training_Master_Interface, Training_Master_IMPL>();
            services.AddScoped<RoomWebInterface, RoomWebService>();
            services.AddScoped<SalesSMSEMAILInterface, SalesSMSEMAILService>();
            services.AddScoped<SalesLeadImportInterface, SalesLeadImportImpl>();

            //ONLINE TEST
            services.AddScoped<QuestionPaperTypeInterface, QuestionPaperTypeImpl>();
            services.AddScoped<OnlineTestInterface, OnlineTestImpl>();
            services.AddScoped<OnlineTestCandidateInterface, OnlineTestCandidateImpl>();


            //Petty Cash
            services.AddScoped<PC_Master_ParticularsInterface, PC_Master_ParticularsImpl>();
            services.AddScoped<PC_RequisitionInterface, PC_RequisitionImpl>();
            services.AddScoped<PC_IndentInterface, PC_IndentImpl>();
            services.AddScoped<PC_Indent_ApprovalInterface, PC_Indent_ApprovalImpl>();
            services.AddScoped<PC_ReportInterface, PC_ReportImpl>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<HR_JobDetailsDMO, HR_JobDetailsDTO>().ReverseMap();
                config.CreateMap<HR_MRFRequisitionDMO, HR_MRFRequisitionDTO>().ReverseMap();
                config.CreateMap<HR_Master_JobsDMO, HR_Master_JobsDTO>().ReverseMap();
                config.CreateMap<HR_Master_PriorityDMO, HR_Master_PriorityDTO>().ReverseMap();
                config.CreateMap<HR_Master_PostionTypeDMO, HR_Master_PostionTypeDTO>().ReverseMap();
                config.CreateMap<HR_Master_PositionDMO, HR_Master_PositionDTO>().ReverseMap();
                config.CreateMap<HR_Master_LocationDMO, HR_Master_LocationDTO>().ReverseMap();
                config.CreateMap<HR_Master_EarningsDeductions, HR_Master_EarningsDeductionsDTO>().ReverseMap();
                config.CreateMap<HR_Candidate_DetailsDMO, HR_Candidate_DetailsDTO>().ReverseMap();
                config.CreateMap<HR_CandidateInterviewScheduleDMO, HR_CandidateInterviewScheduleDTO>().ReverseMap();
                config.CreateMap<HR_Master_EarningsDeductions, HR_Candidate_DetailsDTO>().ReverseMap();
                config.CreateMap<HR_Candidate_InterviewStatusDMO, HR_CandidateInterviewScheduleDTO>().ReverseMap();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile("Logs/HRMSServicesHub-{Date}.txt");

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
