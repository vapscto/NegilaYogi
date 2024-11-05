using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;

using DomainModel.Model;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using CollegePreadmission.Interfaces;
using CollegePreadmission.Services;
using PreadmissionDTOs.com.vaps.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;

namespace CollegePreadmission
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("config.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);


            // Use a PostgreSQL database
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSession();

            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=CollegeTest;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<ClgAdmissionContext>().AddDbContext<ClgAdmissionContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeFeeService")
         )
         );
            services.AddScoped<CollFeeGroupContext>().AddDbContext<CollFeeGroupContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("CollegeFeeService")
         )
         );

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<ApplicationDBContext>().AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<CollegepreadmissionContext>().AddDbContext<CollegepreadmissionContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<logincontext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApstplication1")
         )
     );

            services.AddDbContext<registrationcontext>(options =>
               options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
               )
           );
            services.AddScoped<monthendreportContext>().AddDbContext<monthendreportContext>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
      )
      );

            services.AddDbContext<Enquirycontext>(options =>
             options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
             )
         );
            services.AddDbContext<userDetailscontext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            )
        );
            services.AddDbContext<StudentApplicationContext>(options =>
           options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
           )
       );

            services.AddScoped<OrganisationContext>().AddDbContext<OrganisationContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          .UseRowNumberForPaging()
          )
      );

            services.AddDbContext<MasterPageContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          )
      );
            services.AddDbContext<MasterTemplateContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
         )
     );
            services.AddDbContext<MasterCategoryContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
        )
    );
            services.AddDbContext<MasterBoardandSchoolTypeContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
       )
   );
            //       services.AddDbContext<CommunicationContext>(options =>
            //    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            //    )
            //);

            services.AddScoped<MasterPageModuleMappingContext>().AddDbContext<MasterPageModuleMappingContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
         )
     );
       

            services.AddDbContext<MasterSourceContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
        )
    );

            //         services.AddDbContext<InstitutionRoleModulePreviledgeContext>(options =>
            //    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            //    )
            //);


            services.AddDbContext<MasterRoleContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
       )
   );


            services.AddDbContext<MasterRoleTypeContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
       )
   );

            services.AddScoped<MasterRolePreviledgesContext>().AddDbContext<MasterRolePreviledgesContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);



            services.AddScoped<WrittenTestScheduleContext>().AddDbContext<WrittenTestScheduleContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
       )
   );

            services.AddScoped<StudentDetailsContext>().AddDbContext<StudentDetailsContext>(options =>
 options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
 )
);

            services.AddScoped<OralTestScheduleContext>().AddDbContext<OralTestScheduleContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            services.AddScoped<WrittenTestMarksEntryContext>().AddDbContext<WrittenTestMarksEntryContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            )
            );


            services.AddScoped<OralTestMarksENtryContext>().AddDbContext<OralTestMarksENtryContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            )
            );


            services.AddScoped<MasterModulesContext>().AddDbContext<MasterModulesContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          )
          );

            services.AddDbContext<AcademicContext>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
      )
  );
            services.AddDbContext<ProspectusContext>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
      )
  );

            services.AddDbContext<MasterSectionContext>(options =>
     options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
     )
 );

            services.AddDbContext<MasterReferenceContext>(options =>
    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
    )
);

            services.AddScoped<MasterSubjectContext>().AddDbContext<MasterSubjectContext>(options =>
    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
    )
);

            services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            )
            );

            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            //}).AddEntityFrameworkStores<ApplicationDBContext, int>()
            //  .AddDefaultTokenProviders();


            services.AddIdentity<ApplicationUser, ApplicationRole>()
       .AddEntityFrameworkStores<ApplicationDBContext>();


            services.AddScoped<OralTestMarksENtryContext>().AddDbContext<OralTestMarksENtryContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            )
            );

            services.AddDbContext<StaffLoginContext>(options =>
             options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
             )
             );


            services.AddDbContext<enquiryreportContext>(options =>
                  options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
                  )
                  );

            services.AddDbContext<ReportProspectusContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          )
          );

            services.AddDbContext<TransfrPreAdmtoAdmContext>(options =>
 options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
 )
 );
            services.AddDbContext<Preadmission_School_Registration_CatergoryContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            services.AddDbContext<AdmissionStatusContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            services.AddDbContext<MasterMainMenuContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            services.AddDbContext<ScheduleReportContext>(options =>
  options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
  )
  );

            services.AddDbContext<InstituteMainMenuContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            services.AddDbContext<RegistrationReportContext>(options =>
  options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
  )
  );


            services.AddDbContext<MarksReportContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);
            services.AddDbContext<FeeGroupContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);
            services.AddDbContext<AdmissionFormContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
)
);
            services.AddDbContext<SubjectwisePeriodSettingsContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
)
);



            services.AddScoped<OrganisationContext>();

         
            services.AddMvc().AddJsonOptions(options =>
            {

                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

           
           


            //services.AddSingleton<Test,TestImpl>();
            //ADD INTERFACE AND IMPL HERE
            //--------------------------START----------------------------------------//

            services.AddScoped<CollegeStudentappInterface, CollegeStudentappImpl>();
            services.AddScoped<ApplicationFormInterface, ApplicationFormImpl>();
            services.AddScoped<CLGStatusInterface, CLGStatusImpl>();

            //TotalCount report on 04-08-2021
            services.AddScoped<TotalCountClgReportInterface, TotalCountClgImpl>();
            services.AddScoped<TransfrPreToAdmClgInterface, TransfrPreToAdmClgImpl>();
           // services.AddScoped<DocumentViewClgInterface, DocumentViewClgImpl>();
            services.AddScoped<DocumentViewClgInterface, DocumentViewClgImpl>();
            services.AddScoped<OralTestScheduleClgInterface, OralTestScheduleClgImpl>();
            services.AddScoped<PreLiveMeetingScheduleClgInterface, PreLiveMeetingScheduleClgImpl>();
            //
            //--------------------------END----------------------------------------//


            Mapper.Initialize(config =>

            {

                //ADD DMO AND DTO MAP HERE
                //--------------------------START----------------------------------------//

                config.CreateMap<Enquiry, Enq>().ReverseMap();
                config.CreateMap<Registration, regis>().ReverseMap();
                config.CreateMap<userDetails, usrdetails>().ReverseMap();
                config.CreateMap<StudentApplication, StudentApplicationDTO>().ReverseMap();
                config.CreateMap<OralTestScheduleDMO, OralTestScheduleDTO>().ReverseMap();

                // Added 19-9-2016
                config.CreateMap<StudentGuardian, StudentApplicationDTO>().ReverseMap();
                config.CreateMap<StudentUploadImage, StudentApplicationDTO>().ReverseMap();
                // config.CreateMap<StudentTrnxDoc, StudentApplicationDTO>().ReverseMap();

                config.CreateMap<StudentTrnxDoc, PreadmissionSchoolRegistrationDocumentsDTO>().ReverseMap();

                config.CreateMap<StudentSibling, StudentSiblingDTO>().ReverseMap();
                config.CreateMap<StudentPreviousSchool, StudentPrevSchoolDTO>().ReverseMap();
                config.CreateMap<StudentTransport, StudentApplicationDTO>().ReverseMap();

                config.CreateMap<MasterConfiguration, MasterConfigurationDTO>().ReverseMap();
                // Added 19-9-2016

                // Added on 6-10-2016 by sachin
                config.CreateMap<InstituteTemplate, InstitutionTemplateDTO>().ReverseMap();
                // Added on 6-10-2016 by sachin

                config.CreateMap<Organisation, OrganisationDTO>().ReverseMap();
                config.CreateMap<MasterPage, MasterPageDTO>().ReverseMap();
                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();
                config.CreateMap<MasterRole, MasterRoleDTO>().ReverseMap();

                config.CreateMap<OrganisationEmail, Organisation_EmailIdDTO>().ReverseMap();
                config.CreateMap<OrganisationMobile, Organisation_MobileDTO>().ReverseMap();
                config.CreateMap<OrganisationPhone, Organisation_Phone_NoDTO>().ReverseMap();

                config.CreateMap<MasterModule, MasterModuleDTO>().ReverseMap();
                config.CreateMap<MasterModules, MasterModulesDTO>().ReverseMap();
                config.CreateMap<MasterPageModuleMapping, MasterPageModuleMappingDTO>().ReverseMap();
                config.CreateMap<MasterSource, MasterSourceDTO>().ReverseMap();

                config.CreateMap<TransactionNumbering, TransactionNumberingDTO>().ReverseMap();
                config.CreateMap<Institution_Module, Institution_ModuleDTO>().ReverseMap();
                config.CreateMap<Institution_Module_Page, Institution_Module_PageDTO>().ReverseMap();

                //  config.CreateMap<InstitutionRoleModulePreviledgeDMO, InstitutionRoleModulePreviledgeDTO>().ReverseMap();

                config.CreateMap<MasterRole, MasterRoleDTO>().ReverseMap();
                config.CreateMap<MasterRoleType, MasterRoleTypeDTO>().ReverseMap();

                config.CreateMap<WrittenTestStudentSubjectWiseMarksDMO, WirettenTestSubjectWiseStudentMarksDTO>().ReverseMap();
                config.CreateMap<WIrttenTestSubjectWiseMarksDMO, WirttenTestSubjectWiseMarksEntryDTO>().ReverseMap();
                config.CreateMap<WrittenTestStudentWiseTotalMarksDMO, WrittenTestSutdentTotalMarksDTO>().ReverseMap();
                config.CreateMap<WIrttenTestSubjectWiseMarksDMO, WrittenTestMarksBindDataDTO>().ReverseMap();
                config.CreateMap<WrittenTestScheduleMarksDMO, WrittenTestScheduleWiseMarksDTO>().ReverseMap();

                config.CreateMap<WrittenTestScheduleDMO, WrittenTestScheduleDTO>().ReverseMap();


                config.CreateMap<OralTestOralByMarksDMO, OralTestOralByMarksDTO>().ReverseMap();
                config.CreateMap<OralTestScheduleMarksMapDMO, OralTestScheduleMarksMapDTO>().ReverseMap();
                config.CreateMap<OralTestStudentWiseMarksDMO, OralTestStudentWiseMarksDTO>().ReverseMap();
                config.CreateMap<OralTestStudentStatusDMO, OralTestStudentStatusDTO>().ReverseMap();
                config.CreateMap<OralTestOralByMarksDMO, OralTestMarksBindDataDTO>().ReverseMap();
                config.CreateMap<MasterRolePreviledgeDMO, MasterRolePreviledgeDTO>().ReverseMap();

                //ps
                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();

                config.CreateMap<Institution_MobileNo, Institution_MobileDTO>().ReverseMap();
                config.CreateMap<Institution_Phone_No, Institution_Phone_NoDTO>().ReverseMap();
                config.CreateMap<Institution_EmailId, Institution_EmailIdDTO>().ReverseMap();

                config.CreateMap<Preadmission_SeatBlocked_Student, Preadmission_SeatBlocked_StudentDTO>().ReverseMap();
                config.CreateMap<School_M_Class, School_M_ClassDTO>().ReverseMap();

                config.CreateMap<MasterCategory, MasterCategoryDTO>().ReverseMap();
                config.CreateMap<MasterTemplate, MasterTemplateDTO>().ReverseMap();
                config.CreateMap<MasterBorad, MasterBoardDTO>().ReverseMap();
                config.CreateMap<MasterSchoolType, MasterSchoolTypeDTO>().ReverseMap();
                config.CreateMap<MasterAcademic, AcademicDTO>().ReverseMap();
                config.CreateMap<Prospectus, ProspectusDTO>().ReverseMap();

                config.CreateMap<School_M_Section, MasterSectionDTO>().ReverseMap();

                config.CreateMap<MasterReference, MasterRefernceDTO>().ReverseMap();

                config.CreateMap<MasterSubjectDMO, MasterSubjectDTO>().ReverseMap();
                config.CreateMap<StaffLoginDMO, StaffLoginDTO>().ReverseMap();
                config.CreateMap<DomainModel.Model.ApplicationUserRole, AppUserRoleDTO>().ReverseMap();

                config.CreateMap<Master_Numbering, Master_NumberingDTO>().ReverseMap(); //11/11/2016  
                config.CreateMap<InstitutionRolePrivileges, InstitutionRolePrivilegesDTO>().ReverseMap(); // 15/11/2016


                config.CreateMap<Adm_M_Student, Adm_M_StudentDTO>().ReverseMap();
                config.CreateMap<Master_Institution_SubscriptionValidity, Master_Institution_SubscriptionValidityDTO>().ReverseMap();

                config.CreateMap<Preadmission_School_Registration_CatergoryDMO, Preadmission_School_Registration_CatergoryDTO>().ReverseMap();
                config.CreateMap<Fee_Master_ConcessionDMO, Fee_Master_ConcessionDTO>().ReverseMap();

                config.CreateMap<Preadmission_School_Registration_Concession_StatusDMO, Preadmission_School_Registration_Concession_StatusDTO>().ReverseMap();

                config.CreateMap<AdmissionStatus, AdmissionStatusDTO>().ReverseMap();  //added on 04 Jan 2017

                config.CreateMap<MasterMainMenuDMO, MasterMainMenuDTO>().ReverseMap();
                config.CreateMap<UserRoleWithInstituteDMO, UserRoleWithInstituteDTO>().ReverseMap();

                config.CreateMap<OralTestScheduleMarksMapDMO, ScheduleReportDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<Preadmission_SeatBlocked_Student, SeatBlockReportDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<IVRM_Master_Menu_Page_MappingDMO, IVRM_Master_Menu_Page_MappingDTO>().ReverseMap(); //added by vishnu
                config.CreateMap<MasterMenuPageMappingInstituteWise, IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<Masterclasscategory, MasterClassCategoryDTO>().ReverseMap();

                config.CreateMap<InstituteMainMenuDTO, InstituteMainMenuDMO>().ReverseMap();
           


                config.CreateMap<PaymentDetails, Prospectus>().ReverseMap();
                config.CreateMap<Prospepaymentamount, Prospectus>().ReverseMap();
                config.CreateMap<IVRM_Master_SubjectsDMO, MasterSubjectAllMDTO>().ReverseMap();


                config.CreateMap<IVRM_Master_SubjectsDMO, subject_orderDTO>().ReverseMap();

                config.CreateMap<GeneralConfigDMO, GeneralConfigDTO>().ReverseMap();


                config.CreateMap<PA_School_Application_ProspectusDMO, PA_School_Application_ProspectusDTO>().ReverseMap();


                // Added on 25-10-2017
                config.CreateMap<IVRM_Mandatory_Setting, IVRM_Mandatory_SettingDTO>().ReverseMap();
                config.CreateMap<IVRM_Mandatory_Setting_IW, IVRM_Mandatory_Setting_IWDTO>().ReverseMap();

                config.CreateMap<StudentHelthcertificateDMO, StudentHelthcertificateDTO>().ReverseMap();

                config.CreateMap<PointsDMO, PointsDTO>().ReverseMap();
                config.CreateMap<PA_Student_Transport_ApplicationDMO, StudentHelthcertificateDTO>().ReverseMap();
                config.CreateMap<subCaste, StateDTO>().ReverseMap();

                config.CreateMap<Adm_Master_College_StudentDMO, AdmMasterCollegeStudentDTO>().ReverseMap();

                //--------------------------END----------------------------------------//




            });

            // services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/CollegePreadmission-{Date}.txt");

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
