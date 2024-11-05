using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.ClubManagement
{
   public class ClubManagementContext :DbContext
    {
        public ClubManagementContext(DbContextOptions<ClubManagementContext> options) : base(options)
        {
            Database.SetCommandTimeout(30000);
        }
        public DbSet<CMS_MasterDepartmenDMO> CMS_MasterDepartmenDMO { get; set; }
        public DbSet<Month> MonthDMO { get; set; }
        public DbSet<CMS_Master_InstallmentTypeDMO> CMS_Master_InstallmentTypeDMO { get; set; }
        public DbSet<CMS_Master_InstallmentsDMO> CMS_Master_InstallmentsDMO { get; set; }
        //CMS_MembershipApplicationDMO
        public DbSet<CMS_MembershipApplicationDMO> CMS_MembershipApplicationDMO { get; set; }
        public DbSet<CMS_Member_CategoryDMO> CMS_Member_CategoryDMO { get; set; }
        public DbSet<CMS_TransactionDMO> CMS_TransactionDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<CMS_MasterMemberDMO> CMS_MasterMemberDMO { get; set; }
        public  DbSet<CMS_Master_Member_QualificationDMO> CMS_Master_Member_QualificationDMO { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<IVRM_Master_Gender> gender { get; set; }
        public DbSet<Country> Country { get; set; }
       public DbSet<castecategoryDMO> castecategoryDMO   { get; set; }
        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
        public DbSet<AcademicYear> AcademicYearDMO { get; set; }
        public DbSet<MasterReligionDMO> MasterReligionDMO { get; set; }
        public DbSet<CMS_Master_MemberExperienceDMO> CMS_Master_MemberExperienceDMO { get; set; }
        public DbSet<CMS_Master_MemberMobileNoDMO> CMS_Master_MemberMobileNoDMO { get; set; }
        public DbSet<CMS_Master_Member_EmailIDMO> CMS_Master_Member_EmailIDMO { get; set; }
        public DbSet<CMS_MasterMember_DocumentsDMO> CMS_MasterMember_DocumentsDMO { get; set; }
        public DbSet<CMS_Master_MemberBlockedDMO> CMS_Master_MemberBlockedDMO { get; set; }
        public DbSet<CMS_Transaction_MemberDMO> CMS_Transaction_MemberDMO { get; set; }
        public DbSet<CMS_Transaction_NonMemberDMO> CMS_Transaction_NonMemberDMO { get; set; }
        public DbSet<CMS_Member_StatusDMO> CMS_Member_StatusDMO { get; set; }
        public DbSet<CMS_TransactionsTypeDMO> CMS_TransactionsTypeDMO { get; set; }

        public DbSet<CMS_TransactionsType_InstallmentsDMO> CMS_TransactionsType_InstallmentsDMO { get; set; }
        public DbSet<CMS_TransactionsType_TaxDMO> CMS_TransactionsType_TaxDMO { get; set; }
        public DbSet<CMS_Transaction_DetailsDMO> CMS_Transaction_DetailsDMO { get; set; }
        public DbSet<CMS_ConfigurationDMO> CMS_ConfigurationDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }
    }
}
