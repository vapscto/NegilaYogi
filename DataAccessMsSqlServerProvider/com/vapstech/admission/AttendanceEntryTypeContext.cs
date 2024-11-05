using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class AttendanceEntryTypeContext : DbContext
    {
        public AttendanceEntryTypeContext(DbContextOptions<AttendanceEntryTypeContext> options) :base(options)
        { }
        public DbSet<Adm_studentAttendance> studentattendance { get; set; }
        public DbSet<AttendanceEntryTypeDMO> AttendanceEntryTypeDMO { get; set; }
        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<School_M_Class> AdmClass { get; set; }

        public DbSet<Institution> Institute { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AttendanceEntryTypeDMO>().HasKey(m => m.ASAET_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<AttendanceEntryTypeDMO>();

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
