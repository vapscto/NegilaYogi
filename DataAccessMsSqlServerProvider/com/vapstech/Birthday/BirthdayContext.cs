using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.BirthDay;


namespace DataAccessMsSqlServerProvider.com.vapstech.Birthday
{
    public class BirthdayContext : DbContext
    {
        public BirthdayContext(DbContextOptions<BirthdayContext> options) :base(options)
        { }
        //public DbSet<TTMasterCategoryDMO> TTMasterCategoryDMO { get; set; }

        public DbSet<BirthDayDMO> BirthDayDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> school_Adm_Y_StudentDMO { get; set; }
        public DbSet<MasterAcademic> academicYear { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<Masterclasscategory> masterclasscategory { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {

            //base.OnModelCreating(builder);
            //builder.Entity<TTMasterCategoryDMO>().HasKey(m => m.TTMC_Id);
           
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
           // updateUpdatedProperty<TTMasterCategoryDMO>();
           
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
