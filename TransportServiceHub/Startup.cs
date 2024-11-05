using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using TransportServiceHub.Interfaces;
using TransportServiceHub.Services;
using DomainModel.Model.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub
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

            //var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=IVRM_DevDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=Demo_Hutchings;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("TransportServiceHub")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<TransportContext>().AddDbContext<TransportContext>(options =>
        options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("TransportServiceHub")
        )
        );
            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            //}).AddEntityFrameworkStores<ApplicationDBContext, int>()
            // .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
     .AddEntityFrameworkStores<ApplicationDBContext>();

            // Add framework services.

            services.AddScoped<MasterAreaInterface, MasterAreaImpl>();
            services.AddScoped<MasterLocationInterface, MasterLocationImpl>();
            services.AddScoped<MasterDriverInterface, MasterDriverImpl>();
            services.AddScoped<MasterRouteInterface, MasterRouteImpl>();
            services.AddScoped<DriverChartInterface, DriverChartImpl>();
            services.AddScoped<MsterSessionInterface, MsterSessionImpl>();
            services.AddScoped<MasterFuelInterface, MasterFuelImpl>();
            services.AddScoped<MasterVehicleTypeInterface, MasterVehicleTypeImpl>();
            services.AddScoped<MasterVehicleInterface, MasterVehicleImpl>();
            services.AddScoped<RouteLocationMappingInterface, RouteLocationMappingImpl>();
            services.AddScoped<MasterRouteScheduleInterface, MasterRouteScheduleImpl>();
            services.AddScoped<DriverEmployeeMappingInterface, DriverEmployeeMappingImpl>();
            services.AddScoped<VehicalDriverMappingInterface, VehicalDriverMappingImpl>();
            services.AddScoped<VehicalRouteMappingInterface, VehicalRouteMappingImpl>();
            services.AddScoped<VehicalDriverSubstituteInterface, VehicalDriverSubstituteImpl>();
            services.AddScoped<TransportReportInterface, TransportReportImpl>();
            services.AddScoped<TransportApprovedInterface, TransportApprovedImpl>();
            services.AddScoped<RouteSessionTotalStrengthInterface, RouteSessionTotalStrengthImpl>();
            services.AddScoped<RTODetailsInterface, RTODetailsImpl>();


            
            services.AddScoped<UsrPwdInterface, UsrPwdImpl>();
            services.AddScoped<SMSEmailSendInterface, SMSEmailSendImpl>();

            services.AddScoped<RouteStatusReportInterface, RouteStatusReportImpl>();
            services.AddScoped<TransportStatusReportInterface, TransportStatusReportImpl>();

            services.AddScoped<BuspassprintformInterface, BuspassprintformImpl>();
            services.AddScoped<VahicalCertificateInterface, VahicalCertificateImpl>();
            services.AddScoped<ConsolidatedBusRouteListInterface, ConsolidatedBusRouteListImpl>();
            services.AddScoped<ScheduleWiseBusRouteInterface, ScheduleWiseBusRouteImpl>();
            services.AddScoped<BusRoutesDetailsInterface, BusRoutesDetailsImpl>();
            services.AddScoped<RouteTermFeeDetailsInterface, RouteTermFeeDetailsImpl>();
            services.AddScoped<LocationFeeGroupMappingInterface, LocationFeeGroupMappingImpl>();
            services.AddScoped<StdRouteLocationMapInterface, StdRouteLocationMapImpl>();
            services.AddScoped<DriverChartReportInterface, DriverChartReportImpl>();
            services.AddScoped<MasterServiceStationInterface, MasterServiceStationImpl>();
            services.AddScoped<ExpirySettingsInterface, ExpirySettingsImpl>();
            services.AddScoped<VahicalCertificateReportInterface, VahicalCertificateReportImpl>();
            services.AddScoped<StudentRouteMappingReportInterface, StudentRouteMappingReportImpl>();
            services.AddScoped<RouteSessionScheduleInterface, RouteSessionScheduleImpl>();
            services.AddScoped<TrnsMonthEndReportInterface, TrnsMonthEndReportImpl>();
            services.AddScoped<TRApplDetailsInterface, TRApplDetailsImpl>();

            //Bus Hire.
            services.AddScoped<MasterHire_Group_RateInterface, MasterHirer_Group_Rate_Impl>();
            services.AddScoped<TripOnlineBookingInterface, TripOnlineBookingImpl>();
            services.AddScoped<TripInterface, TripImpl>();
            services.AddScoped<TRGroupConsoleReportInterface, TRGroupConsoleReportImpl>();
            services.AddScoped<TRPartyBillReceiptReportInterface, TRPartyBillReceiptReportImpl>();
            services.AddScoped<BusBookingDetailsReportInterface, BusBookingDetailsReportImpl>();
            services.AddScoped<KMLogBookInterface, KMLogBookImpl>();
            services.AddScoped<TriphiresmsemailreportInterface, TriphiresmsemailreportImpl>();
            services.AddScoped<KMNotupReportInterface, KMNotupReportImpl>();

            //COLLEGE TRANSPORT
            services.AddScoped<CLGStudentRouteMappingInterface, CLGStudentRouteMappingImpl>();
            services.AddScoped<CLGTRNCommonInterface, CLGTRNCommonImpl>();
            services.AddScoped<CLGStudentRouteMappingReportInterface, CLGStudentRouteMappingReportImpl>();
            services.AddScoped<CLGRouteSessionStrengthInterface, CLGRouteSessionStrengthImpl>();
            services.AddScoped<CLGStudentBuspassFormInterface, CLGStudentBuspassFormImpl>();
            services.AddScoped<CLGTransportApproveInterface, CLGTransportApproveImpl>();
            services.AddScoped<CLGBusPassInterface, CLGBusPassImpl>();
            services.AddScoped<CLGRouteStatusReportInterface, CLGRouteStatusReportImpl>();
            services.AddScoped<CLGConsolidatedBusRouteInterface, CLGConsolidatedBusRouteImpl>();
            services.AddScoped<CLGBusRoutesDetailsInterface, CLGBusRoutesDetailsImpl>();
            services.AddScoped<CLGStdRouteUpdateInterface, CLGStdRouteUpdateImpl>();
            services.AddScoped<TrnStudentLocationDetailsInterface, TrnStudentLocationDetailsImpl>();


            Mapper.Initialize(config =>
            {
                config.CreateMap<TR_Route_ScheduleDMO, MasterRouteScheduleDTO>().ReverseMap();
                config.CreateMap<MasterRouteDMO, MasterRouteDTO>().ReverseMap();
                config.CreateMap<SMSEmailSettingDTO, SMSEmailSendDTO>().ReverseMap();

                //Bus Hire.
                config.CreateMap<TripOnlineBookingDMO, TripOnlineBookingDTO>().ReverseMap();
                config.CreateMap<TripDMO, TripDTO>().ReverseMap();

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
            loggerFactory.AddFile("Logs/Transport-{Date}.txt");

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
