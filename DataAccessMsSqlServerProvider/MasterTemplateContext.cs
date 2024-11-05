using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterTemplateContext : DbContext
    {
        public MasterTemplateContext(DbContextOptions<MasterTemplateContext> options) : base(options)
        { }
        public DbSet<MasterTemplate> mastertemplate { get; set; }
        public DbSet<MasterPage> masterpage { get; set; }
        public DbSet<SatRegistrationDMO> SatRegistrationDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterTemplate>().HasKey(m => m.IVRMT_Id);
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
               
            }
        }
    }
}
