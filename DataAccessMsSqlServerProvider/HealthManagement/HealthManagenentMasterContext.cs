using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.HealthManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.HealthManagement
{
    public class HealthManagenentMasterContext: DbContext
    {
        public HealthManagenentMasterContext(DbContextOptions<HealthManagenentMasterContext> options) : base(options)
        { }
        public DbSet<HM_M_Observation_DMO> HM_M_Observation_DMO_con { get; set; }
        public DbSet<HM_M_ExaminationDMO> HM_M_ExaminationDMO_con { get; set; }
        public DbSet<HM_M_DoctorDMO> HM_M_DoctorDMO_con { get; set; }
        public DbSet<HM_M_CleannessDMO> HM_M_CleannessDMO_con { get; set; }
        public DbSet<HM_M_BehaviourDMO> HM_M_BehaviourDMO_con { get; set; }
        public DbSet<HM_M_IllnessDMO> HM_M_IllnessDMO { get; set; }
        public DbSet<HM_T_IllnessDMO> HM_T_IllnessDMO { get; set; }
        public DbSet<MasterAcademic> Academic { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> Adm_School_Y_StudentDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<HM_M_IllnessDMO>().HasKey(m => m.HMMILL_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<HM_M_IllnessDMO>();

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