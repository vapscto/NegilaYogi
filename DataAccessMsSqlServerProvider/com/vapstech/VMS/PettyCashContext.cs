using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using DomainModel.Model.com.vapstech.VMS.PettyCash;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager
{
    public class PettyCashContext : DbContext
    {
        public PettyCashContext(DbContextOptions<PettyCashContext> options) : base(options)
        {
            { Database.SetCommandTimeout(900000000); }
        }
        public DbSet<ApplRole> ApplicationRole_con { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole_con { get; set; }
        public DbSet<ApplUser> ApplicationUser { get; set; }
        public DbSet<PC_Master_ParticularsDMO> PC_Master_ParticularsDMO { get; set; }
        public DbSet<PC_RequisitionDMO> PC_RequisitionDMO { get; set; }
        public DbSet<PC_RequisitionDocumentDMO> PC_RequisitionDocumentDMO { get; set; }
        public DbSet<PC_Requisition_DetailsDMO> PC_Requisition_DetailsDMO { get; set; }
        public DbSet<PC_IndentDMO> PC_IndentDMO { get; set; }
        public DbSet<PC_Indent_DetailsDMO> PC_Indent_DetailsDMO { get; set; }
        public DbSet<PC_Indent_ApprovedDMO> PC_Indent_ApprovedDMO { get; set; }
        public DbSet<PC_Indent_Approved_DetailsDMO> PC_Indent_Approved_DetailsDMO { get; set; }
        public DbSet<PC_ExpenditureDMO> PC_ExpenditureDMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
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
