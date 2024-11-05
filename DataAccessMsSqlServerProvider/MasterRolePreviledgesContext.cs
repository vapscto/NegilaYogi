using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.MobileApp;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class MasterRolePreviledgesContext : DbContext
    {
        public MasterRolePreviledgesContext(DbContextOptions<MasterRolePreviledgesContext> options) :base(options)
        { }

        public DbSet<MasterRolePreviledgeDMO> masterRolePreviledgeDMO { get; set; }
        public DbSet<MasterRoleType> masterRoleType { get; set; }
        public DbSet<MasterPageModuleMapping> masterPageModuleMapping { get; set; }
        public DbSet<MasterPage> masterPage { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<MasterRole> MasterRole { get; set; }

        public DbSet<Institution> Institute { get; set; }

        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }

        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<MasterRolePreviledgeDMO>().HasKey(m => m.IVRMRP_Id);

            //builder.Entity<MasterPageModuleMapping>().HasKey(x => new { x.IVRMMP_Id, x.IVRMM_Id });

            builder.Entity<MasterRolePreviledgeDMO>()
               .ToTable("IVRM_Role_Privileges");

            // base.OnModelCreating(builder);

            //one to one

            //builder.Entity<MasterRolePreviledgeDMO>()
            // .HasOne(p => p.masterRoleType);


            //builder.Entity<MasterRolePreviledgeDMO>()
             //.HasOne(p => p.masterPageMapping);
             
           

             //builder.Entity<MasterPageModuleMapping>()
          //  .HasOne(p => p.masterPage);

        }

        public override int SaveChanges()
        {
            //ChangeTracker.DetectChanges();

            //updateUpdatedProperty<MasterRolePreviledgeDMO>();

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
