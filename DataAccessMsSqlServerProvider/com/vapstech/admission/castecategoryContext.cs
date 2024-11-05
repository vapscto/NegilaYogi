using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace DataAccessMsSqlServerProvider
{
    public class castecategoryContext : DbContext
    {
        public castecategoryContext(DbContextOptions<castecategoryContext> options) :base(options)
        { }

        public castecategoryContext()
        {
        }

        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }
        public DbSet<Adm_M_Student> studentdmo { get; set; }
        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
       
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<IVRM_Master_HTMLTemplatesDMO> IVRM_Master_HTMLTemplatesDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<castecategoryDMO>().HasKey(m => m.IMCC_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<castecategoryDMO>();

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
