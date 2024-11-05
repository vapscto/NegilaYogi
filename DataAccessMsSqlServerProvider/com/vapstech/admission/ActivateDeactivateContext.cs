using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;

namespace DataAccessMsSqlServerProvider
{
    public class ActivateDeactivateContext : DbContext
    {
        public ActivateDeactivateContext(DbContextOptions<ActivateDeactivateContext> options) :base(options)
        { }

        public DbSet<ActivateDeactivateStudentDMO> ActivateDeactivateStudentDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> school_Adm_Y_StudentDMO { get; set; }
        public DbSet<MasterAcademic> academicYear { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<Masterclasscategory> masterclasscategory { get; set; }
        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Studentd { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }      
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Adm_Student_Deactivate_Active_ReasonDMO> Adm_Student_Deactivate_Active_ReasonDMO { get; set; }
        public DbSet<Adm_Student_EcsDetailsDMO> Adm_Student_EcsDetailsDMO { get; set; }


        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<MasterCategory> category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ActivateDeactivateStudentDMO>()
               .ToTable("adm_m_student");

            // base.OnModelCreating(builder);
            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<ActivateDeactivateStudentDMO>();

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
