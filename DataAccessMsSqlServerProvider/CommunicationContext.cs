using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class CommunicationContext : DbContext
    {
        public CommunicationContext(DbContextOptions<CommunicationContext> options) :base(options)
        { }

        public DbSet<OrganisationEmail> organisationEmail { get; set; }
        public DbSet<OrganisationMobile> organisationMobile { get; set; }
        public DbSet<OrganisationPhone> organisationPhone { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrganisationEmail>()
               .ToTable("Master_Organization");

            base.OnModelCreating(builder);
            builder.Entity<OrganisationMobile>()
               .ToTable("OrganisationMobile");

            base.OnModelCreating(builder);
            builder.Entity<OrganisationPhone>()
               .ToTable("OrganisationPhone");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<OrganisationEmail>();
            updateUpdatedProperty<OrganisationMobile>();
            updateUpdatedProperty<OrganisationPhone>();

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
