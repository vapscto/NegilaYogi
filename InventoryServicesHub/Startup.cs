using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Purchase.Inventory;
using InventoryServicesHub.com.vaps.Implementation;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Master.Implementation;
using InventoryServicesHub.com.vaps.Master.Interface;
using InventoryServicesHub.com.vaps.Purchase.Implementation;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using InventoryServicesHub.com.vaps.Sales.Implementation;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

namespace InventoryServicesHub
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            //   var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
          //  var sqlConnectionString = "Data Source=172.16.32.20\\MSSQLSERVER2019;Initial Catalog=VapsDemoDatabase;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;";
            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=IVRM_TestDb;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<InventoryContext>().AddDbContext<InventoryContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("InventoryServicesHub")
         )
         );
            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDBContext>()
             .AddDefaultTokenProviders();          
            services.AddScoped<INV_ConfigurationInterface, INV_ConfigurationImpl>();
            services.AddScoped<INV_DashboardInterface, INV_DashboardImpl>();
            services.AddScoped<INV_MasterGroupInterface, INV_MasterGroupImpl>();
            services.AddScoped<INV_MasterUOMInterface, INV_MasterUOMImpl>();
            services.AddScoped<INV_MasterTaxInterface, INV_MasterTaxImpl>();
            services.AddScoped<INV_MasterStoreInterface, INV_MasterStoreImpl>();
            services.AddScoped<INV_MasterCustomerInterface, INV_MasterCustomerImpl>();
            services.AddScoped<INV_MasterItemInterface, INV_MasterItemImpl>();
            services.AddScoped<INV_MasterProductInterface, INV_MasterProductImpl>();
            services.AddScoped<INV_MasterSupplierInterface, INV_MasterSupplierImpl>();
            services.AddScoped<INV_StockInterface, INV_StockImpl>();
            services.AddScoped<INVMasterCategoryInterface, INVMasterCategoryImpl>();
            //=================================Purchase Transcation================================//
            services.AddScoped<INV_PurchaseRequisitionInterface, INV_PurchaseRequisitionImpl>();
            services.AddScoped<INV_PurchaseIndentInterface, INV_PurchaseIndentImpl>();
            services.AddScoped<INV_PI_ToSupplierInterface, INV_PI_ToSupplierImpl>();
            services.AddScoped<INV_QuotationInterface, INV_QuotationImpl>();
            services.AddScoped<INV_QuotationComparisonInterface, INV_QuotationComparisonImpl>();
            services.AddScoped<INV_T_GRNInterface, INV_T_GRNImpl>();
            services.AddScoped<INV_PurchaseOrderInterface, INV_PurchaseOrderImpl>();
            services.AddScoped<INV_VendorPaymentInterface, INV_VendorPaymentImpl>();
            services.AddScoped<INV_VendorPayment_ReportInterface, INV_VendorPayment_ReportImpl>();
            //=================================Sales Transcation==================================//
            services.AddScoped<INV_OpeningBalanceInterface, INV_OpeningBalanceImpl>();
            services.AddScoped<INV_T_SalesInterface, INV_T_SalesImpl>();
            services.AddScoped<INV_ItemConsumptionInterface, INV_ItemConsumptionImpl>();
            services.AddScoped<INV_PhyStock_UpdationInterface, INV_PhyStock_UpdationImpl>();
            services.AddScoped<CLG_INV_T_SalesInterface, CLG_INV_T_SalesImpl>();
            //================================= Masters Reports=================================//
            services.AddScoped<INV_ItemReportInterface, INV_ItemReportImpl>();
            //================================= Purchase Reports=================================//
            services.AddScoped<INV_R_GRNInterface, INV_R_GRNImpl>();
            services.AddScoped<INV_PR_ReportInterface, INV_PR_ReportImpl>();
            services.AddScoped<INV_PI_ReportInterface, INV_PI_ReportImpl>();
            services.AddScoped<INV_Quotation_ReportInterface, INV_Quotation_ReportImpl>();
            services.AddScoped<INV_PO_ReportInterface, INV_PO_ReportImpl>();

            //================================= Sales Reports===================================//            
            services.AddScoped<INV_R_SalesInterface, INV_R_SalesImpl>();
            services.AddScoped<INV_R_StockInterface, INV_R_StockImpl>();
            services.AddScoped<INV_MonthEndReportInterface, INV_MonthEndReportImpl>();
            services.AddScoped<INV_ItemConsumptionReportInterface, INV_ItemConsumptionReportImpl>();
            services.AddScoped<CLG_INV_SalesReportInterface, CLG_INV_SalesReportImpl>();


            //================== Production Inventory ================

            services.AddScoped<INV_MasterProductStagesInterface, INV_MasterProductStagesImpl>();
            services.AddScoped<INV_ProductReportInterface, INV_ProductReportImpl>();
            services.AddScoped<PhyStock_UpdationInterface, PhyStock_UpdationImpl>();
            services.AddScoped<DCS_T_SalesInterface, DCS_T_SalesImpl>();
            services.AddScoped<DCS_R_StockInterface,DCS_R_StockImpl>();
            services.AddScoped<DCS_Vendor_PaymentInterface, DCS_Vendor_PaymentImpl>();
            services.AddScoped<INV_T_SalesReturnInterface, INV_T_SalesReturnImpl>();
            services.AddScoped<Sales_Return_Approval_Interface, Sales_Return_Approval_Impl>();
            services.AddScoped<INV_StockSummaryInterface, INV_StockSummaryIMPL>();
            services.AddScoped<ClientInvoiceInterface, ClientInvoiceImpl>();
            services.AddScoped<ClientProformaInvoiceInterface, ClientProformaInvoiceImpl>();
            services.AddScoped<ImsClientInterface, ImsClientImpl>();
            services.AddScoped<MastersProjectInterface, MastersProjectImpl>();
            services.AddScoped<MastersModuleInterface, MastersModuleImpl>();
            services.AddScoped<ISM_ClientProject_MappingInterface, ISM_ClientProject_MappingIMPL>();
            Mapper.Initialize(config =>
            {
                config.CreateMap<INV_PurchaseIndentDTO, INV_M_PurchaseRequisitionDMO>().ReverseMap();
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

            loggerFactory.AddFile("Logs/InventoryServicesHub-{Date}.txt");

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
