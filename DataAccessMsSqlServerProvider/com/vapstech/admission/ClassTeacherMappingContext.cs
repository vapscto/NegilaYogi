using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class ClassTeacherMappingContext :DbContext
    {
        public ClassTeacherMappingContext(DbContextOptions<ClassTeacherMappingContext> options) :base(options)
        { }

        public ClassTeacherMappingContext()
        {
        }

        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<School_M_Class> classdetails { get; set; }
        public DbSet<School_M_Section> sectiondetails { get; set; }
        public DbSet<HR_Master_Employee_DMO> employee { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }

        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<MasterCategory> category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<ClassTeacherMappingDMO>().HasKey(m => m.IMCT_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<ClassTeacherMappingDMO>();     
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
