using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;

namespace DataAccessMsSqlServerProvider
{
    public class TransfrPreAdmtoAdmContext :DbContext
    {
        public TransfrPreAdmtoAdmContext(DbContextOptions<TransfrPreAdmtoAdmContext> options) :base(options)
        { }

        public DbSet<Adm_M_Student> organisationEmail { get; set; }
        public DbSet<MasterAcademic> MasterAcademic { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }

        public DbSet<SMSEmailSetting> sMSEmailSetting { get; set; }

        public DbSet<MasterConfiguration> masterConfig { get; set; }

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
