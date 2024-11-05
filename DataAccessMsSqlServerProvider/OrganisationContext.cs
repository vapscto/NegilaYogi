using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class OrganisationContext:DbContext
    {
        public OrganisationContext(DbContextOptions<OrganisationContext> options) :base(options)
        {
        }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<State> State { get; set; }

        public DbSet<OrganisationEmail> organisationEmail { get; set; }
        public DbSet<OrganisationMobile> organisationMobile { get; set; }
        public DbSet<OrganisationPhone> organisationPhone { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Organisation>().HasKey(m => m.MO_Id);

            builder.Entity<OrganisationEmail>().HasKey(m => m.MOE_Id);

            builder.Entity<OrganisationMobile>().HasKey(m => m.MOMN_Id);

            builder.Entity<OrganisationPhone>().HasKey(m => m.MOPN_Id);

            //builder.Entity<Organisation>()
            //   .ToTable("Master_Organization");

            // base.OnModelCreating(builder);

            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            //ChangeTracker.DetectChanges();

            //updateUpdatedProperty<Organisation>();
            //updateUpdatedProperty<OrganisationEmail>();
            //updateUpdatedProperty<OrganisationMobile>();
            //updateUpdatedProperty<OrganisationPhone>();

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
