using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterDocumentContext : DbContext
    {
        public MasterDocumentContext(DbContextOptions<MasterDocumentContext> options) :base(options)
        { }

        public DbSet<MasterDocumentDMO> MasterDocumentDMO { get; set; }
        public DbSet<StudentDocumentDMO> StudentDocumentDMO { get; set; }
        public DbSet<PreadmissionSchoolRegistrationDocuments> PreadmissionSchoolRegistrationDocuments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterDocumentDMO>().HasKey(m => m.AMSMD_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterDocumentDMO>();

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
