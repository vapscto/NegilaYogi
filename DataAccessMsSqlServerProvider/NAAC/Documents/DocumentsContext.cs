using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Model.NAAC.Documents;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.com.vapstech.College.Fees;

namespace DataAccessMsSqlServerProvider.NAAC.Documents
{
   public class DocumentsContext : DbContext
    {
        public DocumentsContext(DbContextOptions<DocumentsContext> options) : base(options) { }
        public DbSet<NaacDocumentUploadDMO> NaacDocumentUploadDMO { get; set; }
        public DbSet<NaacDocumentUploadDetailsDMO> NaacDocumentUploadDetailsDMO { get; set; }
        public DbSet<NAAC_Master_SL_FileDMO> NAAC_Master_SL_FileDMO { get; set; }
        public DbSet<NAAC_Master_SL_File_CommentsDMO> NAAC_Master_SL_File_CommentsDMO { get; set; }
        public DbSet<NAAC_Master_SL_CommentsDMO> NAAC_Master_SL_CommentsDMO { get; set; }

        public DbSet<Institution> Institution { get; set; }
        public DbSet<NAAC_Master_SL_LinkDMO> NAAC_Master_SL_LinkDMO { get; set; }
        public DbSet<ApplicationUserDMO> ApplicationUserDMO { get; set; }
        public DbSet<ApplicationUserRoleDMO> ApplicationUserRoleDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }

        public DbSet<HR_Employee_Awards_DMO> HR_Employee_Awards_DMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HR_Master_EmployeeType> HR_Master_EmployeeType { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }


        //Reports

        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<Naac_Temp_OTState_OTCntry_Report21_DMO> Naac_Temp_OTState_OTCntry_Report21_DMO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            
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
