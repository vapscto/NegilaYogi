using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.MobileApp;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class MasterPageContext: DbContext
    {
        public MasterPageContext(DbContextOptions<MasterPageContext> options) :base(options)
        { }

        public DbSet<MasterPage> masterpage { get; set; }

        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }
        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }
        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterPage>()
               .ToTable("IVRM_Page");

            // base.OnModelCreating(builder);

            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterPage>();

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
