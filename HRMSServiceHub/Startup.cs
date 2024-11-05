using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataAccessMsSqlServerProvider;
using DomainModel;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Localization;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PreadmissionDTOs.com.vaps.HRMS;

using HRMSServicesHub.com.vaps.Interfaces;
using HRMSServicesHub.com.vaps.Services;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using HRMSServiceHub.com.vaps.Interfaces;
using HRMSServiceHub.com.vaps.Services;

namespace HRMSServicesHub
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

            // var sqlConnectionString = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=DCAMPUS;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;";
             var sqlConnectionString = " Data Source = stjameskolkata.database.windows.net,1433; Initial Catalog = Stjames_2022-05-23T05-38Z; Persist Security Info = False; User ID = stjameskolkata; Password = Stjames@123; Connection Timeout = 30;";
            //var sqlConnectionString = " Data Source=kusumavaps.database.windows.net,1433;Initial Catalog=vapskusuma;Persist Security Info=False;User ID=vapskusuma;Password=@zure2021V@p$EcaMpU$;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=chikkatti.database.windows.net,1433;Initial Catalog=chikkatti;Persist Security Info=False;User ID=chikkatti;Password=vaps@123;Connection Timeout=30;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddTransient<HRMSContext>().AddDbContext<HRMSContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HRMSServicesHub")));

            services.AddTransient<AdmissionFormContext>().AddDbContext<AdmissionFormContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HRMSServicesHub")));

            //RESUME UPLOAD
            services.AddScoped<HRResumeUploadInterface, HRResumeUploadService>();

            // Add framework services.
            services.AddScoped<masterSpecialisationInterface, masterSpecialisationImpl>();
            services.AddScoped<masterLeavingReasonInterface, masterLeavingReasonImpl>();
            services.AddScoped<MasterEmployeeTypeInterface, MasterEmployeeTypeService>();
            services.AddScoped<MasterDesignationInterface, MasterDesignationService>();
            services.AddScoped<MasterMaritalStatusInterface, MasterMaritalStatusService>();
            services.AddScoped<MasterGenderInterface, MasterGenderService>();
            services.AddScoped<MasterBankInterface, MasterBankService>();
            services.AddScoped<MasterGroupTypeInterface, MasterGroupTypeService>();
            services.AddScoped<MasterDepartmentInterface, MasterDepartmentService>();
            services.AddScoped<MasterEarningsDeductionsInterface, MasterEarningsDeductionsService>();
            services.AddScoped<MasterGradeInterface, MasterGradeService>();
            services.AddScoped<MasterCourseInterface, MasterCourseService>();
            services.AddScoped<MasterIncomeTaxCessInterface, MasterIncomeTaxCessService>();
            services.AddScoped<MasterLeaveYearInterface, MasterLeaveYearService>();
            services.AddScoped<MasterProfessionalTaxInterface, MasterProfessionalTaxService>();
            services.AddScoped<MasterIncomeTaxInterface, MasterIncomeTaxService>();
            services.AddScoped<MasterIncomeTaxDetailsInterface, MasterIncomeTaxDetailsService>();
            services.AddScoped<MasterIncomeTaxDetailsCessInterface, MasterIncomeTaxDetailsCessService>();
            services.AddScoped<HRMasterExamGroupAInterface, HRMasterExamGroupAService>();
            services.AddScoped<HRMasterExamGroupBInterface, HRMasterExamGroupBService>();
            services.AddScoped<ReligionCategory_MappingInterface, ReligionCategory_MappingImpl>();
            services.AddScoped<EmployeeRegistrationInterface, EmployeeRegistrationService>();
            services.AddScoped<GroupDeptDessgInterface, GroupDeptDessgService>();
            services.AddScoped<employeeSalaryCalculationInterface, employeeSalaryCalculationService>();
            services.AddScoped<MasterPayrollStandardInterface, MasterPayrollStandardService>();
            services.AddScoped<EmployeeSalarySlipGenerationInterface, EmployeeSalarySlipGenerationService>();
            services.AddScoped<massUpdationInterface, massUpdationService>();
            services.AddScoped<salaryUpdationInterface, salaryUpdationService>();
            services.AddScoped<PFTransactionInterface, PFTransactionIMPL>();

            //Increemnet
            services.AddScoped<EmployeeSalaryIncreementProcessInterface, EmployeeSalaryIncreementProcessIMPL>();

            //Reports
            services.AddScoped<ProbationaryReportInterface, ProbationaryReportImpl>();
            services.AddScoped<QualificationReportInterface, QualificationReportImpl>();
            services.AddTransient<CumulativeSalaryReportInterface, CumulativeSalaryReportService>();
            services.AddScoped<EmployeeDetailsReportInterface, EmployeeDetailsReportService>();
            services.AddScoped<EmployeeServiceRecordReportInterface, EmployeeServiceRecordReportService>();
            services.AddScoped<EmployeeContributionReportInterface, EmployeeContributionReportService>();
            services.AddScoped<EmployeeStrengthReportInterface, EmployeeStrengthReportService>();
            services.AddScoped<ESIReportInterface, ESIReportService>();
            services.AddScoped<HeadwiseReportsInterface, HeadwiseReportsService>();
            services.AddScoped<MonthEndReportInterface, MonthEndReportService>();
            services.AddScoped<BankCashReportInterface, BankCashReportService>();
            services.AddScoped<EmployeeProfileReportInterface, EmployeeProfileReportService>();
            services.AddScoped<EmployeeOfferAndExperienceReportInterface, EmployeeOfferAndExperienceReportService>();
            services.AddScoped<Transferred_Employee_DetailsInterface, Transferred_Employee_DetailsImpl>();
            services.AddScoped<HRMasterEmpFullTimeInterface, HRMasterEmpFullTimeService>();
               services.AddScoped<PFForm5stopPensionSTJamesReportInterface, PFForm5stopPensionSTJamesReportIMPL>();
            //PF Reports

            services.AddScoped<PFForm3AInterface, PFForm3AService>();
            services.AddScoped<PFForm6AInterface, PFForm6AService>();
            services.AddScoped<PFForm5Interface, PFForm5Service>();
            services.AddScoped<PFForm10Interface, PFForm10Service>();
            services.AddScoped<PFForm12BBInterface, PFForm12BBService>();
            services.AddScoped<PFForm12BBInvestmentDeclarationFormatInterface, PFForm12BBInvestmentDeclarationFormatService>();
            services.AddScoped<PS7andPS8FormReportInterface, PS7andPS8FormReportIMPL>();
            services.AddScoped<PFChallenInterface, PFChallenService>();
            services.AddScoped<EPFcontributionRegisterInterface, EPFcontributionRegisterService>();
            services.AddScoped<EmployeeDetailsImportInterface, EmployeeDetailsImportService>();

            services.AddScoped<CTCReportInterface, CTCReportService>();
            services.AddScoped<FORMNO19Interface, FORMNO19Service>();
            services.AddScoped<FORM12Interface, FORM12Service>();
            services.AddScoped<FORMNO15GInterface, FORMNO15GService>();
            services.AddScoped<FORMTInterface, FORMTService>();
            services.AddScoped<HREmpSalaryAdvanceInterface, HREmpSalaryAdvanceService>();
            services.AddScoped<HRMasterLoanInterface, HRMasterLoanService>();
            services.AddScoped<HREmpLoanInterface, HREmpLoanService>();
            services.AddScoped<EmployeeSalaryDetailsInterface, EmployeeSalaryDetailsService>();
            services.AddScoped<HRProcessConfigurationInterface, HRProcessConfigurationImpl>();

            services.AddScoped<LoannonDeductLetterInterface, LoanLetterRequestservice>();
            services.AddScoped<LoanApprovalInterface, EmployeeLoanApproval>();
            //  services.AddScoped<LoanApprovalInterface, EmployeeLoanApproval>();

            services.AddScoped<masterpointInterface, masterpointsService>();
            //services.AddScoped<masterparameterInterface, masterparameterService>();
            services.AddScoped<SalaryApprovalInterface, SalaryApprovalImpl>();
            services.AddScoped<Form16Interface, Form16Service>();
            services.AddScoped<EmployeeGrauityInterface, EmployeeGrauityService>();
            services.AddScoped<EmployeeSalaryCertificateInterface, EmployeeCertificateServices>();

            services.AddScoped<SalaryadvanceInterfacereport, EmployeeadvanceReportService>();
            services.AddScoped<SalaryloanInterfacereport, EmployeeLoanReportService>();
            services.AddScoped<salaryApprovalflowInterface, salaryApprovalflowService>();
            services.AddScoped<Departmentsalaryinterface, DepartmentsalaryService>();
            services.AddScoped<HRMasterPANInterface, HRMasterPANService>();
            services.AddScoped<EmployeeSalarySlipInterfaceModified, EmployeeSalarySlipServiceModified>();
            services.AddScoped<ECRInterface, ECRImpl>();
            services.AddScoped<MasterQuarterInterface, MasterQuarterService>();
            services.AddScoped<Master80CInterface, Master80CService>();

            services.AddScoped<HREmpTDSInterface, HREmpTDSService>();
            services.AddScoped<MasterAllowanceInterface, MasterAllowanceService>();
            services.AddScoped<HREmpAllowanceInterface, HREmpAllowanceService>();
            services.AddScoped<HREmpOtherIncomeInterface, HREmpOtherIncomeService>();
            services.AddScoped<MasterotherIncomeInterface, MasterotherIncomeService>();
            services.AddScoped<HREmpChapterVIInterface, HREmpChapterVIService>();
            services.AddScoped<MasterChapterVIInterface, HRMasterchapterVIService>();
            services.AddScoped<HREmpTDSQuarterInterface, HREmpTDSQuarterService>();
            services.AddScoped<EmployeeDataImportInterface, EmployeeDataImportIMP>();

            services.AddScoped<EmployeeYearlyReportInterface, EmployeeYearlyService>();
            services.AddScoped<PFForm9AInterface, PFForm9AService>();
            services.AddScoped<RegisterWagesInterface, RegisterWagesService>();
            services.AddScoped<EmployeeAutopromotionInterface, EmployeeAutopromotionService>();
            services.AddScoped<ArrearSalaryReportInterface, ArrearSalaryReport>();
            services.AddScoped<EmployeeAwardInterface, EmployeeAwardImpl>();
            services.AddScoped<StaffCompliantsInterface, StaffCompliantsImpl>();
            services.AddScoped<HealthCardDetailsInterface, HealthCardDetailsIMPL>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<HR_Master_EmployeeType, HR_Master_EmployeeTypeDTO>().ReverseMap();
                config.CreateMap<IVRM_Master_Marital_Status, IVRM_Master_Marital_StatusDTO>().ReverseMap();
                config.CreateMap<IVRM_Master_Gender, IVRM_Master_GenderDTO>().ReverseMap();
                config.CreateMap<HR_Master_BankDeatils, HR_Master_BankDeatilsDTO>().ReverseMap();
                config.CreateMap<HR_Master_GroupType, HR_Master_GroupTypeDTO>().ReverseMap();
                config.CreateMap<HR_Master_Department, HR_Master_DepartmentDTO>().ReverseMap();
                config.CreateMap<HR_Master_EarningsDeductions, HR_Master_EarningsDeductionsDTO>().ReverseMap();
                config.CreateMap<HR_Master_Grade, HR_Master_GradeDTO>().ReverseMap();
                config.CreateMap<HR_Master_CourseDMO, HR_Master_CourseDTO>().ReverseMap();
                config.CreateMap<HR_Master_IncomeTax_CessDMO, HR_Master_IncomeTax_CessDTO>().ReverseMap();
                config.CreateMap<HR_Master_LeaveYearDMO, HR_Master_LeaveYearDTO>().ReverseMap();
                config.CreateMap<HR_Master_ProfessionalTaxDMO, HR_Master_ProfessionalTaxDTO>().ReverseMap();
                config.CreateMap<HR_Master_IncomeTaxDMO, HR_Master_IncomeTaxDTO>().ReverseMap();
                config.CreateMap<HR_Master_Designation, HR_Master_DesignationDTO>().ReverseMap();
                config.CreateMap<HR_Master_IncomeTax_DetailsDMO, HR_Master_IncomeTax_DetailsDTO>().ReverseMap();
                config.CreateMap<HR_Master_IncomeTax_Details_CessDMO, HR_Master_IncomeTax_Details_CessDTO>().ReverseMap();
                config.CreateMap<HR_MasterExam_GroupADMO, HR_MasterExam_GroupADTO>().ReverseMap();
                config.CreateMap<HR_MasterExam_GroupBDMO, HR_MasterExam_GroupBDTO>().ReverseMap();

                config.CreateMap<MasterEmployee, MasterEmployeeDTO>().ReverseMap();

                config.CreateMap<Master_Employee_Experience, Master_Employee_ExperienceDTO>().ReverseMap();
                config.CreateMap<Master_Employee_Qulaification, Master_Employee_QulaificationDTO>().ReverseMap();

                config.CreateMap<Master_Employee_Documents, Master_Employee_DocumentsDTO>().ReverseMap();
                config.CreateMap<HR_Master_Employee_Bank, HR_Master_Employee_BankDTO>().ReverseMap();

                //PayRoll
                config.CreateMap<HR_Employee_Salary, HR_Employee_SalaryDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Salary, HR_Employee_SalaryModifiedDTO>().ReverseMap();
                config.CreateMap<HR_Master_EarningsDeductions_Type, HR_Master_EarningsDeductions_TypeDTO>().ReverseMap();
                config.CreateMap<HR_Configuration, HR_ConfigurationDTO>().ReverseMap();
                config.CreateMap<HR_Master_EarningsDeductionsPer, HR_Master_EarningsDeductionsPerDTO>().ReverseMap();
                config.CreateMap<HR_Employee_EarningsDeductions, HR_Employee_EarningsDeductionsDTO>().ReverseMap();
                config.CreateMap<HR_Master_Employee_IncrementDetails, HR_Master_Employee_IncrementDetailsDTO>().ReverseMap();

                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();
                config.CreateMap<MasterEmployee, MasterEmployeeImportDTO>().ReverseMap();
                config.CreateMap<Multiple_Mobile_DMO, Mobile_Number_DTO>().ReverseMap();
                config.CreateMap<Multiple_Email_DMO, Email_Id_DTO>().ReverseMap();
                config.CreateMap<HR_Emp_SalaryAdvance, HR_Emp_SalaryAdvanceDTO>().ReverseMap();
                config.CreateMap<HRMasterLoan, HRMasterLoanDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Loan, HR_Emp_LoanDTO>().ReverseMap();
                config.CreateMap<Master_Numbering, Master_NumberingDTO>().ReverseMap();

                config.CreateMap<HR_Emp_Loan, HR_Emp_LoanDTO>().ReverseMap();
                config.CreateMap<HR_PROCESSDMO, HR_ProcessDTO>().ReverseMap();
                config.CreateMap<HR_Process_Auth_OrderNoDMO, HR_Process_Auth_OrderNo>().ReverseMap();
                config.CreateMap<HR_PROCESS_PRIVILEGE, HR_PROCESS_PRIVILEGEDTO>().ReverseMap();

                config.CreateMap<HR_Emp_Loan_TransactionDMO, HR_Emp_Loan_TransactionDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Assesment_Points, HR_Employee_AssesmentpointsDTO>().ReverseMap();
               config.CreateMap<HR_Master_IncomeTax_DetailsDMO , HR_Master_IncomeTaxDTO>().ReverseMap();
                config.CreateMap<HRMasterPAN, HRMasterPANDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Salary_Approval, HR_Emp_Salary_ApprovalDTO>().ReverseMap();
                config.CreateMap<HR_ECRDMO, ECRDTO>().ReverseMap();
                config.CreateMap<HR_Master_Quarter, HR_Master_QuarterDTO>().ReverseMap();
                config.CreateMap<HR_Master_Quarter_Month, HR_Master_QuarterDTO>().ReverseMap();
                config.CreateMap<HR_Master_Allowance, MasterAllowanceDTO>().ReverseMap();
                config.CreateMap<HR_Emp_Allowance, HR_Emp_AllowanceDTO>().ReverseMap();
                config.CreateMap<HR_Emp_OtherIncome, HR_Emp_otherIncomeDTO>().ReverseMap();
                config.CreateMap<HR_Master_OtherIncome, HR_master_otherIncomeDTO>().ReverseMap();
                config.CreateMap<HR_Emp_ChapterVI, HR_Emp_ChapterVIDTO>().ReverseMap();
                config.CreateMap<HR_Master_ChapterVI, MasterAllowanceDTO>().ReverseMap();
                config.CreateMap<HR_Emp_TDS, HR_Emp_TDSDTO>().ReverseMap();
                config.CreateMap<HR_Employee_TDS_Quarter, HR_Emp_TDS_QUARTERDTO>().ReverseMap();

                config.CreateMap<HR_Employee_Increment, HR_Emp_IncrementDTO>().ReverseMap();
                config.CreateMap<HR_Employee_Increment_EDHeads, HR_Emp_IncrementDTO>().ReverseMap();
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
            loggerFactory.AddFile("Logs/HRMSServicesHub-{Date}.txt");

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
