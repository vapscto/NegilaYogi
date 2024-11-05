using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PreadmissionDTOs;

namespace DataAccessMsSqlServerProvider
{
    public class logincontext : DbContext
    {

        public logincontext(DbContextOptions<logincontext> options) :base(options)
        { }

        public DbSet<logindata> logindata { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<logindata>()
               .ToTable("users");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<logindata>();
          
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
