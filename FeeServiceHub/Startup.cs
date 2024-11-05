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
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.Fee.Tally;
using PreadmissionDTOs.com.vaps.Fees.Tally;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using FeeServiceHub.com.vaps.services.FinancialAccounting;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Juspay;

namespace FeeServiceHub
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
            services.AddMemoryCache();
            //  var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //   var sqlConnectionString = "Data Source=kusumavaps.database.windows.net,1433;Initial Catalog=vapskusuma;Persist Security Info=False;User ID=vapskusuma;Password=@zure2021V@p$EcaMpU$;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";

            // var sqlConnectionString = "Data Source=kusumavaps.database.windows.net,1433;Initial Catalog=vapskusuma;Persist Security Info=False;User ID=vapskusuma;Password=@zure2021V@p$EcaMpU$;Connection Timeout=30;";

            // var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=vapseclgcampus.database.windows.net,1433;Initial Catalog=vapsecampus;Persist Security Info=False;User ID=vapsclg;Password=Vts@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            //  var sqlConnectionString = "Data Source=bdcampus.database.windows.net,1433;Initial Catalog=bdcampus;Persist Security Info=False;User ID=baldwincampus;Password=b@Ldw!nDig!tal;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source = dcampus.database.windows.net,1433; Initial Catalog = DCAMPUS; Persist Security Info = False; User ID = decampus; Password = Digit@lc@mpu$@1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            //  var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            // var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";
            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=Demo_Hutchings;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            //var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";

            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

             
            services.AddScoped<ApplicationDBContext>().AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<FeeGroupContext>().AddDbContext<FeeGroupContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<CollFeeGroupContext>().AddDbContext<CollFeeGroupContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            services.AddScoped<PortalContext>().AddDbContext<PortalContext>(options =>
options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("corewebapi18072016")));

            // Add framework services. 

            //services.AddScoped<MasterClassHeldInterface, MasterClassHeldImpl>();

            services.AddScoped<FeeGroupInterface, FeeGroupImplimentation>();
            services.AddScoped<FeeHeadInterface, FeeHeadImpl>();
            services.AddScoped<FeeSpecialHeadInterface, FeeSpecialHeadImpl>();
            services.AddScoped<FeeGroupGroupingInterface, FeeGroupGroupingImpl>();
            services.AddScoped<FeeClassCategoryInterface, FeeClassCategoryImpl>();
            services.AddScoped<FeeFineSlabInterface, FeeFineSlabImpl>();
            services.AddScoped<FeeMasterTermsInterface, FeeMasterTermsImpl>();
            services.AddScoped<FeeInstallmentInterface, FeeInstallmentImpl>();
            services.AddScoped<FeeMasterConfigInterface, FeeMasterConfigImpl>();
            services.AddScoped<StudentFeeEnablePartialPaymentInterface, StudentFeeEnablePartialPaymentImpl>();

            services.AddScoped<YearlyFeeGroupMappingInterfaces, YearlyFeeGroupMappingImpl>();
            services.AddScoped<FeeAmountEntryInterfaces, FeeAmountEntryImpl>();
            services.AddScoped<StudentFeeGroupMappingInterface, StudentFeeGroupMappingImpl>();
            //added
            services.AddScoped<StudentFeeGroupMappingGroupDeletionInterface, StudentFeeGroupMappingGroupDeletionImpl>();
            //
            services.AddScoped<FeeStudentTransactionInterface, FeeStudentTransactionImpl>();

            services.AddScoped<FeeChequeBounceInterface, FeeChequeBounceImpl>(); // added on 28/11/2016

            services.AddScoped<FeeRefundableInterface, FeeRefundableImpl>(); // added on 28/11/2016
            services.AddScoped<FeeConcessionInterface, FeeConcessionImpl>(); // added on 28/11/2016
            services.AddScoped<FeeConcessionNewInterface, FeeConcessionNewImpl>();
            services.AddScoped<FeeTrailAuditReportInterface, FeeTrailAuditReportImpl>();

            services.AddScoped<FeeSummaryReportInterface, FeeSummaryReportImpl>();
            services.AddScoped<FeeStudentConcessionReportInterface, FeeStudentConcessionReportImpl>();
            services.AddScoped<FeeReceiptReportInterface, FeeReceiptReportImpl>();
            services.AddScoped<FeeThirdPartyReportInterface, FeeThirdPartyReportImpl>();
            services.AddScoped<ClassSecessionWiseFeeCollectionReportInterface, ClassSecessionWiseFeeCollectionReportImpl>();
            services.AddScoped<FeeArrearRegisterReportInterface, FeeArrearRegisterReportImpl>();
            services.AddScoped<FeeConsolidatedReportInterface, FeeConsolidatedReportImpl>();
            services.AddScoped<FeeITReceiptReportInterface, FeeItReceiptReportImpl>();

            services.AddScoped<MonthendReportInterface, MonthEndReportImpl>();
            services.AddScoped<MonthlyCollectionReportInterface, MonthlyCollectionReportImpl>();
            services.AddScoped<FeeDefaulterReportInterface, FeeDefaulterReportImpl>();
            services.AddScoped<FeeCollectionReportInterface, FeeCollectionReportImpl>();
            services.AddScoped<FeeClasswiseConcessionReportInterface, FeeClasswiseConcessionReportImpl>();
            services.AddScoped<FeeStatusReportInterface, FeeStatusReportImpl>();
            services.AddScoped<FeeInstallmentReportInterface, FeeInstallmentReportImpl>();
            services.AddScoped<FeeHeadWisecollectionReportInterface, FeeHeadWisecollectionReportImpl>();
           // services.AddScoped<FeeClassWiseReportInterface, FeeClassWiseReportImpl>();
            services.AddScoped<FeeConcessionReportInterface, FeeConcessionReportImpl>();
            //keerthana
            services.AddScoped<Financial_YearInterface, Financial_YearImpl>();
            services.AddScoped<HlMasterRoom_FeeGroupInterface, HlMasterRoom_FeeGroupImpl>();
            //shilpa
            services.AddScoped<CategoryConcessionGroupMappingInterface, CategoryConcessionGroupMappingImpl>();

            services.AddScoped<ClassCategoryReportInterface, ClassCategoryReportImp>();
            services.AddScoped<FeeInstallmentDetailsInterface, FeeInstallmentDetailsImpl>();
            services.AddScoped<FeeHeadWiseReportInterface, FeeHeadWiseReportImpl>();
            services.AddScoped<FeeDueDateReportInterface, FeeDueDateReportImpl>();
            services.AddScoped<FeeGroupWiseStudentReportInterface, FeeGroupWiseStudentReportImpl>();
            //services.AddScoped<FeeAccountsTallyReportInterface, FeeAccountTallyReportImpl>();
            services.AddScoped<FeeRemittanceCertificateInterface, FeeRemittanceCertificateImpl>();
            services.AddScoped<FeeDailyCollectionInterface, FeeDailyCollectionImpl>();
            services.AddScoped<FeeChallanReportInterface, FeeChallanReportImpl>();
            services.AddScoped<FeeRefundInterface, FeeRefundImpl>();
            services.AddScoped<FeeOpeningBalanceInterface, FeeOpeningBalanceImpl>();
            services.AddScoped<FeeOnlinePaymentInterface, FeeOnlinePaymentImpl>();

            services.AddScoped<FeeSummarizedReportInterface, FeeSummarizedReportImpl>();

            services.AddScoped<FeeAdjustmentInterface, FeeAdjustmentImpl>();
            services.AddScoped<FeeWaivedOffInterface, FeeWaivedOffImpl>();

            services.AddScoped<feeitreportInterface, feeitreportImpl>();

            services.AddScoped<FeePrevilegeInterface, FeePrevilegeImpl>();

            services.AddScoped<PreadmissionOnlinePaymentInterface, PreadmissionOnlinePaymentImpl>();

            services.AddScoped<FeePrevilegeInterface, FeePrevilegeImpl>();

            //Sripad Joshi on 14-10-2017
            services.AddScoped<FeePaymentGatewayDetailsInterface, FeePaymentGatewayDetailsImpl>();

            services.AddScoped<OnlinePaymentHeadGroupMappingInterface, OnlinePaymentHeadGroupMappingImpl>();
            services.AddScoped<FeeMasterOtherStudentInterface, FeeMasterOtherStudentImpl>();

            services.AddScoped<StaffFeeGroupMappingInterface, StaffFeeGroupMappingImpl>();

            services.AddScoped<FeeCardDetailsEntryInterface, FeeCardDetailsEntryImpl>();
            services.AddScoped<FeeStaffOthersTransactionInterface, FeeStaffOthersTransactionImpl>();//added by mahaboob
            services.AddScoped<StudentRouteMappingInterface, StudentRouteMappingImpl>();//added by mahaboob
            services.AddScoped<FeePreadmissionInterface, FeePreadmissionImpl>();

            services.AddScoped<FeeMasterGroupwiseAutoReceiptInterface, FeeMasterGroupwiseAutoReceiptImpl>();

            services.AddScoped<FeeDemandRegisterInterface, FeeDemandRegisterImpl>();

            services.AddScoped<FeeAccountsPositionInterface, FeeAccountsPositionImpl>();
            services.AddScoped<Student_SettlementInterface, Student_SettlementImpl>();
            services.AddScoped<ThirdPartyTransactionInterface, ThirdPartyTransactionImpl>();
            services.AddScoped<FeeStreamGroupMappingInterface, FeeStreamGroupMappingImpl>();
            services.AddScoped<FeeDetailsReportInterface, FeeDetailsReportImpl>();
            services.AddScoped<FeeHeadLedMapInter, HeadLedgerMapImplemen>();

            services.AddScoped<FeeTallyTransactionInterface, FeeTallyTransactionImpl>();
            services.AddScoped<Fee_Tally_Master_CompanyInterface, Fee_Tally_Master_CompanyIMPL>();

            services.AddScoped<FeeMasterConcessionInterface, FeeMasterConcesionImpl>();
            services.AddScoped<ECSBulkImportInterface, ECSBulkImportImpl>();

            services.AddScoped<SiblingEmployeeMappingInterface, SiblingEmployeeMappingImpl>();
            services.AddScoped<TuitionFeeCertificateInterface, TuitionFeeCertificateImpl>();
            services.AddScoped<FeeActivityRequestInterface, FeeActivityRequestImpl>();

            services.AddScoped<ModeOfPaymentInterface, ModeOfPaymentImpl>();
            services.AddScoped<MasterActivityGroupHeadInterface, MasterActivityGroupHeadImpl>();

            services.AddScoped<Financial_YearInterface, Financial_YearImpl>();
            services.AddScoped<HlMasterRoom_FeeGroupInterface, HlMasterRoom_FeeGroupImpl>();
            services.AddScoped<BankDetailsInterface, BankDetailsImpl>();
            services.AddScoped<StudentFeeGroupMappingNextAcaYrInterface, StudentFeeGroupMappingNextAcaYrImpl>();
            // FAAccounting
            services.AddScoped<FAMasterGroupInterface, FAMasterGroupIMPL>();
            services.AddScoped<FAUser_GroupInterface, FAUser_GroupIMPL>();
            services.AddScoped<FiancialAccuntingLedgerInterface, FiancialAccuntingLedgerIMPL>();
            services.AddScoped<FiancialAccountingVoucherInterface, FiancialAccountingVoucherIMPL>();
            services.AddScoped<FAMasterCompanyInterface, FAMasterCompanyImpl>();
            services.AddScoped<AreaGroupMappingInterface, AreaGroupMappingImpl>();
            services.AddScoped<FinancialAccountingReportInterface, FinancialAccountingReportIMPL>();

            services.AddScoped<FeeWizardInterface,FeeWizardImpl>();
            services.AddScoped<CategoryWiseFeeCollectionInterface, CategoryWiseFeeCollectionImpl>();
            services.AddScoped<DefaulterFeeCollectionGraphInterface, DefaulterFeeCollectionGraphImpl>();
            services.AddScoped<StudentWiseFeeCollectionInterface, StudentWiseFeeCollectionImpl>();
            services.AddScoped<DateWiseFeeCollectionInterface, DateWiseFeeCollectionImpl>();
            services.AddScoped<AcademicYearWiseCollectionInterface, AcademicYearWiseCollectionImpl>();
            services.AddScoped<FeesMakerAndCheckerInterface,FeesMakerAndCheckerImpl>();
            services.AddScoped<FeeYearlyRebateSettingInterface, FeeYearlyRebateSettingImpl>();
            services.AddScoped<FeeTermWiseRebateSettingInterface, FeeTermWiseRebateSettingImpl>();
            services.AddScoped<FeeReceiptImportInterface, FeeReceiptImportImpl>();

            services.AddScoped<FeePDCInterface, FeePDCImpl>();
            services.AddScoped<MasterNarrationInterface, MasterNarrationImpl>();

            services.AddScoped<FeeReceiptImportStthomasInterface, FeeReceiptImportStthomasImpl>();
            services.AddScoped<AutoLedgerCreationInterface, AutoLedgerCreationImpl>();
            services.AddScoped<GenderWisePaidDetailsInterface, GenderWisePaidDetailsImpl>();
            services.AddScoped<ExcelImportNotDoneReportInterface, ExcelImportNotDoneReportImpl>();
            services.AddScoped<FeeOnlinePaymentStthomasInterface, FeeOnlinePaymentStthomasImpl>();

            services.AddScoped<FeeDefaulterReportStthomasInterface, FeeDefaulterReportStthomasImpl>();
            services.AddScoped<FeeAmountEntryStthomasInterface, FeeAmountEntryStthomasImpl>();

            Mapper.Initialize(config =>
            {
                // config.CreateMap<SMSEmailSetting, SmsEmailDTO>().ReverseMap(); // 15/11/2016

                config.CreateMap<FeeStudentAdjustment, FeeStudentAdjustmentDTO>().ReverseMap();
                config.CreateMap<FeeStudentWaivedOffDMO, FeeStudentWaiveOffDTO>().ReverseMap();
                config.CreateMap<FeeTermWiseRebateSettingDMO, FeeTermWiseRebateSettingDTO>().ReverseMap();

                config.CreateMap<FeeGroupDMO, FeeGroupDTO>().ReverseMap();
                config.CreateMap<FeeYearGroupDMO, FeeYearlyGroupDTO>().ReverseMap();
                config.CreateMap<FeeHeadDMO, FeeHeadDTO>().ReverseMap();
                config.CreateMap<FeeSpecialFeeGroupDMO, FeeSpecialFeeGroupDTO>().ReverseMap();
                config.CreateMap<FeeGroupMappingDMO, FeeGroupMappingDTO>().ReverseMap();
                config.CreateMap<FeeClassCategoryDMO, FeeClassCategoryDTO>().ReverseMap();
                config.CreateMap<FeeYearlyClassCategoryDMO, FeeYearlyClassCategoryDTO>().ReverseMap();
                config.CreateMap<FeeFineSlabDMO, FeeFineSlabDTO>().ReverseMap();
                config.CreateMap<FeeTermDMO, FeeTermDTO>().ReverseMap();
                config.CreateMap<FeeInstallmentDMO, FeeInstallmentDTO>().ReverseMap();
                config.CreateMap<FeeInstallmentsyearlyDMO, FeeInstallmentyeralyDTO>().ReverseMap();
                config.CreateMap<FeeInstallmentDueDateDMO, FeeInstalmentDueDateDTO>().ReverseMap();
                config.CreateMap<FeeMasterConfigurationDMO, FeeMasterConfigurationDTO>().ReverseMap();
                config.CreateMap<FeeMasterTermHeadsDMO, FeeMasterTermHeadsDTO>().ReverseMap();
                config.CreateMap<MasterTermFeeHeadsDueDateDMO, FeeMasterTermFeeHeadsDueDateDTO>().ReverseMap();
                config.CreateMap<FeeGroupGroupingDMO, FeegroupgroupingDTO>().ReverseMap();
                config.CreateMap<FeeSpecialFeeGroupsGroupingDMO, FeeSpecialFeeGroupsGroupDTO>().ReverseMap();


                config.CreateMap<FeeAmountEntryDMO, FeeAmountEntryDTO>().ReverseMap(); //11/11/2016
                config.CreateMap<FeeStudentGroupMappingDMO, FeeStudentGroupMappingDTO>().ReverseMap(); //11/11/2016
                config.CreateMap<FeeStudentGroupInstallmentMappingDMO, FeeStudentGroupInstallmentMappingDTO>().ReverseMap();
                config.CreateMap<FeeStudentTransactionDMO, FeeStudentTransactionDTO>().ReverseMap();

                config.CreateMap<FeePaymentDetailsDMO, FeePaymentDetailsDTO>().ReverseMap();
                config.CreateMap<FeeTransactionPaymentDMO, FeeTransactionPaymentDTO>().ReverseMap();
                config.CreateMap<FeeChequeBounceDMO, FeeChequeBounceDTO>().ReverseMap();
                config.CreateMap<FeeMasterRefundDMO, FeeMasterRefundDTO>().ReverseMap();
                config.CreateMap<FeeTDueDateECSDMO, FeeTDueDateECSDTO>().ReverseMap();
                config.CreateMap<FeeTDueDateRegularDMO, FeeTDueDateRegularDTO>().ReverseMap();
                config.CreateMap<MasterMonthDMO, MasterMonthDMO>().ReverseMap(); // 15/11/2016
                config.CreateMap<MasterMonthECSDMO, MasterMonthECSDMO>().ReverseMap(); // 15/11/2016

                config.CreateMap<FeeTFineSlabDMO, FeeTFineSlabDTO>().ReverseMap(); // 15/11/2016
                config.CreateMap<FeeTFineSlabECSDMO, FeeTFineSlabECSDTO>().ReverseMap(); // 15/11/2016

                config.CreateMap<V_StudentPendingDMO, V_StudentPendingDTO>().ReverseMap(); // 15/11/2016


                config.CreateMap<MasterYearlyClassCategoryClassDMO, MasterYearlyClassCategoryClassDTO>().ReverseMap(); // 15/11/2016

                config.CreateMap<FeeConcessionDMO, FeeConcessionDTO>().ReverseMap(); // 15/11/2016

                config.CreateMap<FeeTransactionPaymentDMO, MonthlyCollectionReportDTO>().ReverseMap();   // vishnu
                config.CreateMap<FeeTransactionPaymentDMO, FeeHeadWisecollectionReportDTO>().ReverseMap();  // vishnu
                config.CreateMap<Adm_M_Student, FeeTrailAuditDTO>().ReverseMap();

                config.CreateMap<FeeClassCategoryDMO, FeeHeadWiseReportDTO>().ReverseMap();// 15/11/2016
                config.CreateMap<FeeClassCategoryDMO, FeeDueDateReportDTO>().ReverseMap();
                config.CreateMap<FeeGroupDMO, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<AdmissionClass, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<School_M_Section, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<MasterCategory, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<MasterCategory, FeeRemittanceCertificateDTO>().ReverseMap();

                config.CreateMap<FeeClassCategoryDMO, FeeHeadWiseReportDTO>().ReverseMap();// 15/11/2016
                config.CreateMap<FeeClassCategoryDMO, FeeDueDateReportDTO>().ReverseMap();
                config.CreateMap<FeeGroupDMO, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<AdmissionClass, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<School_M_Section, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<Adm_M_Student, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<MasterCategory, FeeGroupWiseStudentReportDTO>().ReverseMap();
                config.CreateMap<MasterCategory, FeeRemittanceCertificateDTO>().ReverseMap();
                config.CreateMap<FeeTransactionPaymentDMO, DailyCollectionReportDTO>().ReverseMap();

                config.CreateMap<FEeGroupLoginPreviledgeDMO, FeePrevilegeDTO>().ReverseMap();

                config.CreateMap<FeeBankDetailsDMO, FeeBankDetailsDTO>().ReverseMap();//kiran on 19/04/17

                config.CreateMap<FEeGroupLoginPreviledgeDMO, FeePrevilegeDTO>().ReverseMap();

                config.CreateMap<Fee_Master_Staff_GroupHead, FeeStudentGroupMappingDTO>().ReverseMap();
                config.CreateMap<Fee_Master_Staff_GroupHead_Installments, FeeStudentGroupMappingDTO>().ReverseMap();

                //Sripad Joshi on 14-10-2017
                config.CreateMap<Fee_PaymentGateway_DetailsDMO, Fee_PaymentGateway_DetailsDTO>().ReverseMap();
                config.CreateMap<Fee_OnlinePayment_MappingDMO, Fee_OnlinePayment_MappingDTO>().ReverseMap();
                config.CreateMap<FeeMasterOtherStudentDMO, FeeMasterOtherStudentDTO>().ReverseMap();

                config.CreateMap<Master_Numbering, Master_NumberingDTO>().ReverseMap();

                config.CreateMap<FeeCardDetailsEntryDMO, FeeCardDetailEntryDTO>().ReverseMap();

                config.CreateMap<FeePaymentDetailsDMO, FeeStaffOthersTransactionDTO>().ReverseMap();

                config.CreateMap<FeePaymentDetailsDMO, FeeStaffOthersTransactionDTO>().ReverseMap();
                config.CreateMap<Fee_M_Online_TransactionDMO, FeeStudentTransactionDTO>().ReverseMap();
                config.CreateMap<Fee_T_Online_TransactionDMO, FeeStudentTransactionDTO>().ReverseMap();

                config.CreateMap<Fee_Groupwise_AutoReceipt_GroupsDMO, Fee_Groupwise_AutoReceipt_GroupsDTO>().ReverseMap();
                config.CreateMap<Fee_Groupwise_AutoReceiptDMO, Fee_Groupwise_AutoReceiptDTO>().ReverseMap();

                config.CreateMap<HeadLedgerMappingDMO, HeadLedgerCodeMapDTO>().ReverseMap();
                config.CreateMap<Adm_Master_Activities, Adm_Master_ActivitiesDTO>().ReverseMap();
                config.CreateMap<FeePaymentDetailsDMO, FeesMakerAndCheckerDTO>().ReverseMap();
                config.CreateMap<FeePDCDMO, FeePDCDTO>().ReverseMap();

            });

            services.AddMvc().AddJsonOptions(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                //options.SerializerSettHlMasterRoom_FeeGroupDTOings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
               
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/feehub-{Date}.txt");

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
