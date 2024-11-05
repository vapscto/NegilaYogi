using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using corewebapi18072016;
using corewebapi18072016.Controllers.com.vapstech.AuthServer;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel;
using DomainModel.Model;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IVRMUX
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
            var configurationSection = Configuration.GetSection("FacadeUrl");
            var StudentFacadeUrlOne = configurationSection.GetValue<string>("StudentFacadeUrlOne");
            var StudentFacadeUrlTwo = configurationSection.GetValue<string>("StudentFacadeUrlTwo");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddSession();

            //  var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_NewSharedHost;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            //var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = sqlConnectionString;
                options.SchemaName = "dbo";
                options.TableName = "SQLSessions";

            });

            services.AddSession(options =>
            {
                options.CookieName = "IVRM.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(120);

            });

            services.AddMvc();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.AddMvc(
               config =>
               {
                   config.Filters.Add(new GlobalInterceptor());
               });


            services.AddOpenIddict();

            services.AddDbContext<DbContext>(options =>
            {
                // Configure the context to use an in-memory store.
                options.UseInMemoryDatabase(nameof(DbContext));
                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDBContext>();
                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();
                // Enable the token endpoint.
                options.EnableTokenEndpoint("/api/Authorization/connect/token");
                // Enable the password flow.
                options.AllowPasswordFlow();
                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();
            });
            // Register the validation handler, that is used to decrypt the tokens.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OAuthValidationDefaults.AuthenticationScheme;
            })
            .AddOAuthValidation();

            //services.AddScoped<GlobalInterceptor>();

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                options.UseSqlServer(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("IVRMUX")
                    // this is needed unless you are on mssql 2012 or higher
                    .UseRowNumberForPaging()
                )
            );

            services.AddDbContext<Enquirycontext>(options =>
              options.UseSqlServer(
                  sqlConnectionString,
                  b => b.MigrationsAssembly("IVRMUX")
                  .UseRowNumberForPaging()
              )
          );

            services.AddDbContext<registrationcontext>(options =>
             options.UseSqlServer(
                 sqlConnectionString,
                 b => b.MigrationsAssembly("IVRMUX")
             )
         );

            services.AddDbContext<StudentApplicationContext>(options =>
              options.UseSqlServer(
                  sqlConnectionString,
                  b => b.MigrationsAssembly("IVRMUX")
              )
          );

            services.AddScoped<OrganisationContext>().AddDbContext<OrganisationContext>(options =>
             options.UseSqlServer(
                 sqlConnectionString,
                 b => b.MigrationsAssembly("IVRMUX")
                 .UseRowNumberForPaging()
             )
         );

            services.AddDbContext<MasterPageContext>(options =>
         options.UseSqlServer(
             sqlConnectionString,
             b => b.MigrationsAssembly("IVRMUX")
         )
     );

            services.AddScoped<MasterPageModuleMappingContext>().AddDbContext<MasterPageModuleMappingContext>(options =>
      options.UseSqlServer(
          sqlConnectionString,
          b => b.MigrationsAssembly("IVRMUX")
      )
  );
            services.AddScoped<MasterSourceContext>().AddDbContext<MasterSourceContext>(options =>
      options.UseSqlServer(
          sqlConnectionString,
          b => b.MigrationsAssembly("IVRMUX")
      )
  );

            services.AddScoped<MasterRoleContext>().AddDbContext<MasterRoleContext>(options =>
options.UseSqlServer(
sqlConnectionString,
b => b.MigrationsAssembly("IVRMUX")
)
);

            services.AddScoped<MasterRoleTypeContext>().AddDbContext<MasterRoleTypeContext>(options =>
   options.UseSqlServer(
       sqlConnectionString,
       b => b.MigrationsAssembly("IVRMUX")
   )
);

            services.AddScoped<MasterRolePreviledgesContext>().AddDbContext<MasterRolePreviledgesContext>(options =>
options.UseSqlServer(
sqlConnectionString,
b => b.MigrationsAssembly("IVRMUX")
)
);


            services.AddDbContext<CommunicationContext>(options =>
       options.UseSqlServer(
          sqlConnectionString,
          b => b.MigrationsAssembly("IVRMUX")
      )
  );
            services.AddScoped<ProspectusContext>().AddDbContext<ProspectusContext>(options =>
           options.UseSqlServer(
               sqlConnectionString,
               b => b.MigrationsAssembly("IVRMUX")
           )
       );
            services.AddScoped<AcademicContext>().AddDbContext<AcademicContext>(options =>
          options.UseSqlServer(
               sqlConnectionString,
               b => b.MigrationsAssembly("IVRMUX")
           )
      );


            services.AddScoped<ProspectusContext>().AddDbContext<ProspectusContext>(options =>
           options.UseSqlServer(
               sqlConnectionString,
               b => b.MigrationsAssembly("IVRMUX")
           )
       );
            services.AddScoped<AcademicContext>().AddDbContext<AcademicContext>(options =>
          options.UseSqlServer(
               sqlConnectionString,
               b => b.MigrationsAssembly("IVRMUX")
           )
      );
            services.AddScoped<MasterCategoryContext>().AddDbContext<MasterCategoryContext>(options =>
               options.UseSqlServer(
                   sqlConnectionString,
                   b => b.MigrationsAssembly("IVRMUX")
               )
           );
            services.AddScoped<MasterTemplateContext>().AddDbContext<MasterTemplateContext>(options =>
          options.UseSqlServer(
              sqlConnectionString,
              b => b.MigrationsAssembly("IVRMUX")
          )
      );
            services.AddScoped<MasterBoardandSchoolTypeContext>().AddDbContext<MasterBoardandSchoolTypeContext>(options =>
          options.UseSqlServer(
              sqlConnectionString,
              b => b.MigrationsAssembly("IVRMUX")
          )
      );


            services.AddScoped<StudentTcReportContext>().AddDbContext<StudentTcReportContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX")));

            services.AddScoped<StudentYearLossReportContext>().AddDbContext<StudentYearLossReportContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX")));


            services.AddScoped<studentbirthdayreportContext>().AddDbContext<studentbirthdayreportContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX")));

            services.AddScoped<StudentAddressBook1Context>().AddDbContext<StudentAddressBook1Context>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX")));

            services.AddScoped<StudentAddressBook2Context>().AddDbContext<StudentAddressBook2Context>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX")));



            services.AddScoped<ClassWiseDailyAttendanceContext>().AddDbContext<ClassWiseDailyAttendanceContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
         )
         );

            services.AddScoped<monthendreportContext>().AddDbContext<monthendreportContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
        )
        );

            services.AddScoped<GovernmentBondContext>().AddDbContext<GovernmentBondContext>(options =>
      options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
      )
      );

            services.AddScoped<StudentAttendanceReportContext>().AddDbContext<StudentAttendanceReportContext>(options =>
                options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
                )
                );

            services.AddScoped<readmitstudentContext>().AddDbContext<readmitstudentContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
       )
       );


            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IVRMUX"));
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>()
         .AddEntityFrameworkStores<ApplicationDBContext>()
         .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/#/login");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
.AddCookie();

            services.AddScoped<StaffLoginContext>().AddDbContext<StaffLoginContext>(options =>
        options.UseSqlServer(
             sqlConnectionString,
             b => b.MigrationsAssembly("IVRMUX")
         )
    );

            services.AddScoped<AttendanceEntryTypeContext>().AddDbContext<AttendanceEntryTypeContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
         )
         );

            services.AddScoped<SubjectwisePeriodSettingsContext>().AddDbContext<SubjectwisePeriodSettingsContext>(options =>
       options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
       )
       );

            services.AddScoped<MasterPeriodContext>().AddDbContext<MasterPeriodContext>(options =>
   options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")
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


            services.AddDbContext<ActivateDeactivateContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          )
          );


            services.AddDbContext<AdmissionStandardContext>(options =>
          options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
          )
          );

            services.AddDbContext<FeeGroupContext>(options =>
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

            services.AddDbContext<ScheduleReportContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
        )
        );

            services.AddDbContext<InstituteMainMenuContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
)
);

            //changes
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });
            //changes


            services.AddScoped<IDataAccessProvider, DataAccessMsSqlServerProvider.DataAccessMsSqlServerProvider>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                //options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.Configure<FacadeUrl>(Configuration.GetSection("FacadeUrl"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:51572").AllowAnyMethod());
            });

            //services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiforgery)
        {
            app.Use(next => context =>
            {
                if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) || string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                       new CookieOptions() { HttpOnly = false });
                }
                return next(context);
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto
            });

            app.UseSession();

            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseIdentity();

            //app.UseOAuthValidation();
            //app.UseOpenIddict();

            app.UseAuthentication();

            app.UseMvc();
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");
            app.UseCors("AllowSpecificOrigin");

        }


        //private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        //{
        //    // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
        //    using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        //        await context.Database.EnsureCreatedAsync();

        //        var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

        //        if (await manager.FindByClientIdAsync("4", cancellationToken) == null)
        //        {
        //            var descriptor = new OpenIddictApplicationDescriptor
        //            {
        //                ClientId = "4",
        //                ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
        //                DisplayName = "MVC client application",
        //                PostLogoutRedirectUris = { new Uri("http://localhost:51572") },
        //                RedirectUris = { new Uri("http://localhost:51572") }
        //            };

        //            await manager.CreateAsync(descriptor, cancellationToken);
        //        }

        //        // To test this sample with Postman, use the following settings:
        //        //
        //        // * Authorization URL: http://localhost:54540/connect/authorize
        //        // * Access token URL: http://localhost:54540/connect/token
        //        // * Client ID: postman
        //        // * Client secret: [blank] (not used with public clients)
        //        // * Scope: openid email profile roles
        //        // * Grant type: authorization code
        //        // * Request access token locally: yes
        //        if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
        //        {
        //            var descriptor = new OpenIddictApplicationDescriptor
        //            {
        //                ClientId = "postman",
        //                DisplayName = "Postman",
        //                RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") }
        //            };

        //            await manager.CreateAsync(descriptor, cancellationToken);
        //        }
        //    }
        //}

        //private void SetOpenIdConnectOptions(OpenIdConnectOptions options)
        //{
        //    options.ClientId = Configuration["auth:oidc:clientid"];
        //   // options.ClientSecret = Configuration["OpenIdSettings:ClientSecret"];
        //    options.Authority = Configuration["auth:oidc:authority"];
        //   // options.MetadataAddress = $"{Configuration["OpenIdSettings:Authority"]}/.well-known/openid-configuration";
        //    options.GetClaimsFromUserInfoEndpoint = true;
        //    options.SignInScheme = "Cookies";
        //    options.ResponseType = OpenIdConnectResponseType.IdToken;

        //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //    {
        //        // This sets the value of User.Identity.Name to users AD username
        //        //NameClaimType = IdentityClaimTypes.WindowsAccountName,
        //        //RoleClaimType = IdentityClaimTypes.Role,
        //        AuthenticationType = "Cookies",
        //        ValidateIssuer = false
        //    };

        //    // Scopes needed by application
        //    options.Scope.Add("openid");
        //    options.Scope.Add("profile");
        //    options.Scope.Add("roles");
        //}


    }
}
