using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;

using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.HRMS;

namespace DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement
{
    public class LMContext : DbContext
    {
        public LMContext(DbContextOptions<LMContext> options) : base(options)
        { }
        //public DbSet<TTMasterCategoryDMO> TTMasterCategoryDMO { get; set; }

        public DbSet<HR_Emp_Leave_Appl_AuthorisationDMO> HR_Emp_Leave_Appl_AuthorisationDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<HR_Master_GroupType_DMO> HR_Master_GroupType_DMO { get; set; }
        public DbSet<HR_Master_Department_DMO> HR_Master_Department_DMO { get; set; }
        public DbSet<HR_Master_Designation_DMO> HR_Master_Designation_DMO { get; set; }
        public DbSet<HR_Master_Leave_DMO> HR_Master_Leave_DMO { get; set; }
        //public DbSet<MasterLeaveDMO> MasterLeave_DMO { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<HR_Master_EarningsDeductions_DMO> HR_Master_EarningsDeductions_DMO { get; set; }
        public DbSet<HR_Emp_OB_Leave_DMO> HR_Emp_OB_Leave_DMO { get; set; }
        public DbSet<HR_Emp_Leave_Trans_DMO> HR_Emp_Leave_Trans_DMO { get; set; }
        public DbSet<HR_Master_Leave_Details_DMO> HR_Master_Leave_Details_DMO { get; set; }
        public DbSet<HR_Master_Leave_Details_CreditMonth_DMO> HR_Master_Leave_Details_CreditMonth_DMO { get; set; }
        public DbSet<HR_Master_Leave_Details_CFMonth_DMO> HR_Master_Leave_Details_CFMonth_DMO { get; set; }
        public DbSet<HR_Emp_Leave_Credit_DMO> HR_Emp_Leave_Credit_DMO { get; set; }
        public DbSet<HR_Master_Grade_DMO> HR_Master_Grade_DMO { get; set; }
        public DbSet<HR_Leave_Authorisation_DMO> HR_Leave_Authorisation_DMO { get; set; }
        public DbSet<HR_Leave_Auth_OrderNo_DMO> HR_Leave_Auth_OrderNo_DMO { get; set; }
        public DbSet<HR_Emp_Leave_Trans_Details_DMO> HR_Emp_Leave_Trans_Details_DMO { get; set; }
        public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<HR_Emp_Leave_ApplicationDMO> HR_Emp_Leave_ApplicationDMO { get; set; }
        public DbSet<HR_Master_LeaveYear_DMO> HR_Master_LeaveYear_DMO { get; set; }
        public DbSet<HR_Master_Leave_Details_CF_DMO> HR_Master_Leave_Details_CF_DMO { get; set; }
        public DbSet<HR_Master_Leave_Details_EnCash_DMO> HR_Master_Leave_Details_EnCash_DMO { get; set; }
        public DbSet<HR_Emp_Leave_Appl_DetailsDMO> HR_Emp_Leave_Appl_DetailsDMO { get; set; }
        //public DbSet<HR_Emp_Leave_StatusDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<HR_Emp_Leave_Application_DeputationDMO> HR_Emp_Leave_Application_DeputationDMO { get; set; }
        public DbSet<HR_Leave_Policy_Config_DMO> HR_Leave_Policy_Config_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<UserRoleWithInstituteDMO> IVRM_User_Login_InstitutionwiseDMO { get; set; }
    
   

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_GroupType_DMO>().HasKey(m => m.HRMGT_Id);

            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Department_DMO>().HasKey(m => m.HRMD_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Designation_DMO>().HasKey(m => m.HRMDES_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_DMO>().HasKey(m => m.HRML_Id);
            base.OnModelCreating(builder);
            builder.Entity<IVRM_Month_DMO>().HasKey(m => m.IVRM_Month_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Employee_DMO>().HasKey(m => m.HRME_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_EarningsDeductions_DMO>().HasKey(m => m.HRMED_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_OB_Leave_DMO>().HasKey(m => m.HREOBL_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_Leave_Trans_DMO>().HasKey(m => m.HRELT_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_Details_DMO>().HasKey(m => m.HRMLD_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_Details_CreditMonth_DMO>().HasKey(m => m.HRMLDCM_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_Details_CFMonth_DMO>().HasKey(m => m.HRMLDCFM_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_Leave_Credit_DMO>().HasKey(m => m.HRELC_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Grade_DMO>().HasKey(m => m.HRMG_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Leave_Authorisation_DMO>().HasKey(m => m.HRLA_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Leave_Auth_OrderNo_DMO>().HasKey(m => m.HRLAON_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_Leave_Trans_Details_DMO>().HasKey(m => m.HRELTD_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_LeaveYear_DMO>().HasKey(m => m.HRMLY_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_Details_CF_DMO>().HasKey(m => m.HRMLDCF_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Leave_Details_EnCash_DMO>().HasKey(m => m.HRMLDEC_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_Leave_Appl_DetailsDMO>().HasKey(m => m.HRELAPD_Id);

            base.OnModelCreating(builder);
            builder.Entity<HR_Emp_Leave_StatusDMO>().HasKey(m => m.HRELS_Id);
            base.OnModelCreating(builder);
            builder.Entity<HR_Leave_Policy_Config_DMO>().HasKey(m => m.HRLPC_Id);

            //base.OnModelCreating(builder);
            //builder.Entity<HR_Master_GroupType_DMO>().ToTable("HR_Master_GroupType");
            //builder.Entity<HR_Master_Department_DMO>().ToTable("HR_Master_Department");
            //builder.Entity<HR_Master_Designation_DMO>().ToTable("HR_Master_Designation");

        }
        //public override int SaveChanges()
        //{
        //    return base.SaveChanges();
        //}
        //private void updateUpdatedProperty<T>() where T : class
        //{
        //    var modifiedSourceInfo =
        //        ChangeTracker.Entries<T>()
        //            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        //    foreach (var entry in modifiedSourceInfo)
        //    {
        //        //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
        //    }
        //}
    }
}
