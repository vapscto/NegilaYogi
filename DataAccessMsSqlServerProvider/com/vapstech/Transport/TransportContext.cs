using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.Fee;
using System.Net;

namespace DataAccessMsSqlServerProvider.com.vapstech.Transport
{
    public class TransportContext : DbContext
    {

        public TransportContext(DbContextOptions<TransportContext> options) : base(options)
        { }
        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<MasterLocationDMO> MasterLocationDMO { get; set; }
        public DbSet<MasterDriverDMO> MasterDriverDMO { get; set; }
        public DbSet<VehicleDriver> VehicleDriver { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<Driver_Employee> Driver_Employee { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<Route_Location> Route_Location { get; set; }
        public DbSet<MasterRouteDMO> MasterRouteDMO { get; set; }
        public DbSet<MasterRouteAreaMappingDMO> MasterRouteAreaMappingDMO { get; set; }
        public DbSet<VehicleRouteDMo> VehicleRouteDMo { get; set; }
        public DbSet<DriverChartDMO> DriverChartDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<MasterSessionDMO> MsterSessionDMO { get; set; }
        public DbSet<VehicleDriverSessionDMO> VehicleDriverSessionDMO { get; set; }
        public DbSet<VehicleRouteSessionDMO> VehicleRouteSessionDMO { get; set; }
        public DbSet<Master_VehicleDMO> Master_VehicleDMO { get; set; }
        public DbSet<MasterFuelDMO> MasterFuelDMO { get; set; }
        public DbSet<MasterVehicleTypeDMO> MasterVehicleTypeDMO { get; set; }
        public DbSet<TR_Route_ScheduleDMO> TR_Route_ScheduleDMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }
        public DbSet<TR_Route_Sch_SessionDMO> TR_Route_Sch_SessionDMO { get; set; }
        public DbSet<VehicalDriverSubstituteDMO> TR_Vehicle_Driver_SubstituteDMO { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }

        public DbSet<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }
        public DbSet<AcademicYear> AcademicYearDMO { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<Adm_Student_Transport_ApplicationDMO> Adm_Student_Transport_ApplicationDMO { get; set; }

        public DbSet<Adm_Student_Transport_Application_UpdateDMO> Adm_Student_Transport_Application_UpdateDMO  { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<TransportApprovedDMO> TransportApprovedDMO { get; set; }

        public DbSet<ApplUser> ApplUser { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }

        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfigurationDMO { get; set; }
        public DbSet<FeeStudentTransactionDMO> FeeStudentTransactionDMO { get; set; }
        public DbSet<FeeTermDMO> FeeTerms { get; set; }
        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<FeeHeadDMO> FeeHeadDMO { get; set; }
        public DbSet<FeeYearlygroupHeadMappingDMO> FeeYearlygroupHeadMappingDMO { get; set; }

        public DbSet<FeeTermDMO> FeeTermDMO { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> FeeMasterTermHeadsDMO { get; set; }

        public DbSet<FeeTransactionPaymentDMO> FeeTransactionPaymentDMO { get; set; }
        public DbSet<Fee_Y_Payment_School_StudentDMO> Fee_Y_Payment_School_StudentDMO { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        //Bus Hire.
        public DbSet<MasterHirerDMO> MasterHirerDMO { get; set; }
        public DbSet<MasterHirerGroupDMO> MasterHirerGroupDMO { get; set; }
        public DbSet<MasterHirerRateDMO> MasterHirerRateDMO { get; set; }
        public DbSet<TripOnlineBookingDMO> TripOnlineBookingDMO { get; set; }
        public DbSet<TripDMO> TripDMO { get; set; }
        public DbSet<TR_Trip_PaymentDMO> TR_Trip_PaymentDMO { get; set; }
        public DbSet<TR_Trip_Payment_TripsDMO> TR_Trip_Payment_TripsDMO { get; set; }
        public DbSet<FeeStudentTransactionDMO> feestudentstatus { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> FeeHead { get; set; }
        public DbSet<TRVehicleDriverAllottmentDMO> TRVehicleDriverAllottmentDMO { get; set; }
        public DbSet<RTODetailsDMO> RTODetailsDMO { get; set; }
        //praveen
        public DbSet<Institution> Institution_master { get; set; }
        public DbSet<VahicalCertificateDMO> VahicalCertificateDMO { get; set; }
        public DbSet<VahicalCertificateDocumentDMO> VahicalCertificateDocumentDMO { get; set; }
        public DbSet<TR_Location_FeeGroup_MappingDMO> TR_Location_FeeGroup_MappingDMO { get; set; }
        public DbSet<TR_student_LocMappingDMO> TR_student_LocMappingDMO { get; set; }

        public DbSet<TR_Part_PerticularsDMO> TR_Part_PerticularsDMO { get; set; }

        public DbSet<TR_Master_ServStationDMO> TR_Master_ServStationDMO { get; set; }

        public DbSet<TR_PartperticularTypeDMO> TR_PartperticularTypeDMO { get; set; }
        public DbSet<TransportStandardsDMO> TransportStandardsDMO { get; set; }
        public DbSet<TR_Route_Sch_Sess_LocationDMO> TR_Route_Sch_Sess_LocationDMO { get; set; }
        public DbSet<TR_KM_LogBookDMO> TR_KM_LogBookDMO { get; set; }
        public DbSet<Month> MonthDMO { get; set; }
        public DbSet<FeeStudentGroupMappingDMO> FeeStudentGroupMappingDMO { get; set; }
        public DbSet<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }
        public DbSet<TR_ServiceDMO> TR_ServiceDMO { get; set; }
        public DbSet<TR_Service_DetailsDMO> TR_Service_DetailsDMO { get; set; }
        public DbSet<TR_Service_PayementDMO> TR_Service_PayementDMO { get; set; }
       
        
        //COLLEGE TRANSPORT
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<CLGStudentRouteMappingDMO> CLGStudentRouteMappingDMO { get; set; }
        public DbSet<CLGStudentRouteFeeGroupDMO> CLGStudentRouteFeeGroupDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
  
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CLGAdm_Std_Transport_ApplicationDMO> CLGAdm_Std_Transport_ApplicationDMO { get; set; }
        public DbSet<Fee_College_Student_StatusDMO> Fee_College_Student_StatusDMO { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<Clg_Fee_AmountEntry_DMO> Clg_Fee_AmountEntry_DMO { get; set; }
        public DbSet<CLG_Fee_College_Master_Amount_Semesterwise> CLG_Fee_College_Master_Amount_Semesterwise { get; set; }
        public DbSet<Fee_Y_Payment_College_StudentDMO> Fee_Y_Payment_College_Student { get; set; }
        public DbSet<Fee_T_College_PaymentDMO> Fee_T_College_PaymentDMO { get; set; }
        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }
        public DbSet<MOBILE_INSTITUTION> MOBILE_INSTITUTION { get; set; }
        public DbSet<CLGTransportApprovedDMO> CLGTransportApprovedDMO { get; set; }
        public DbSet<Fee_College_Master_Student_GroupHeadDMO> Fee_College_Master_Student_GroupHeadDMO { get; set; }
        public DbSet<Fee_Y_PaymentDMO> CLGFee_Y_PaymentDMO { get; set; }
        public DbSet<CLGAdm_Student_Trans_Appl_update_CollegeDMO> CLGAdm_Student_Trans_Appl_update_CollegeDMO { get; set; }
        public DbSet<TR_Master_Vehicle_DocumentsDMO> TR_Master_Vehicle_DocumentsDMO { get; set; }

        public DbSet<TR_Location_AmountDMO> TR_Location_AmountDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterSessionDMO>().ToTable("TR_Master_Session", "TRN");
            builder.Entity<MasterSessionDMO>().HasKey(m => m.TRMS_Id);
            builder.Entity<VehicleDriverSessionDMO>().HasKey(m => m.TRVDS_Id);
            builder.Entity<VehicleRouteSessionDMO>().HasKey(m => m.TRVRS_Id);
            builder.Entity<MasterAreaDMO>().HasKey(m => m.TRMA_Id);
            builder.Entity<MasterLocationDMO>().HasKey(m => m.TRML_Id);
            builder.Entity<MasterDriverDMO>().HasKey(m => m.TRMD_Id);
            builder.Entity<VehicleDriver>().HasKey(m => m.TRVD_Id);
            builder.Entity<Driver_Employee>().HasKey(m => m.TRDE_Id);
            builder.Entity<MasterEmployee>().HasKey(m => m.HRME_Id);

            builder.Entity<Route_Location>().HasKey(m => m.TRMRL_Id);
            builder.Entity<MasterRouteDMO>().HasKey(m => m.TRMR_Id);

            builder.Entity<VehicleRouteDMo>().HasKey(m => m.TRVR_Id);
            builder.Entity<DriverChartDMO>().HasKey(m => m.TRDC_Id);
            builder.Entity<Master_VehicleDMO>().HasKey(m => m.TRMV_Id);
            builder.Entity<TR_Route_ScheduleDMO>().HasKey(m => m.TRRSC_Id);
            builder.Entity<TR_Route_Sch_SessionDMO>().HasKey(m => m.TRRSCS_Id);
            builder.Entity<VehicalDriverSubstituteDMO>().HasKey(m => m.TRVDST_Id);


            builder.Entity<Adm_M_Student>().HasKey(m => m.AMST_Id);
            builder.Entity<AcademicYear>().HasKey(m => m.ASMAY_Id);

            //Bus Hire.
            builder.Entity<MasterHirerDMO>().HasKey(m => m.TRMH_Id);
            builder.Entity<MasterHirerGroupDMO>().HasKey(m => m.TRHG_Id);
            builder.Entity<MasterHirerRateDMO>().HasKey(m => m.TRHR_Id);
            builder.Entity<TripOnlineBookingDMO>().HasKey(m => m.TRTOB_Id);
            builder.Entity<TripDMO>().HasKey(m => m.TRTP_Id);
            builder.Entity<TR_Trip_PaymentDMO>().HasKey(m => m.TRTPP_Id);
            builder.Entity<TR_Trip_Payment_TripsDMO>().HasKey(m => m.TRTPPT_Id);
            builder.Entity<TRVehicleDriverAllottmentDMO>().HasKey(m => m.TRVDA_Id);
            builder.Entity<TR_Location_AmountDMO>().HasKey(m => m.TRMLAMT_Id);
            builder.Entity<TR_Area_AmountDMO>().HasKey(m => m.TRMAAMT_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
