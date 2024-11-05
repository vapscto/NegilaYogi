using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Birthday
{
    public class ClgBirthdayContext : DbContext
    {
        public ClgBirthdayContext(DbContextOptions<ClgBirthdayContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }

        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<FO_Master_HolidayWorkingDay_DatesDMO>().ToTable("FO_Master_HolidayWorkingDay_Dates", "FO");
            base.OnModelCreating(modelbuilder);
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
