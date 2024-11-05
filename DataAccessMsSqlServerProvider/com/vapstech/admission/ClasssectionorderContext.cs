using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class ClasssectionorderContext :DbContext
    {
        public ClasssectionorderContext(DbContextOptions<ClasssectionorderContext> options) :base(options)
        { }

        public ClasssectionorderContext()
        {
        }

        public DbSet<School_M_Class> accclass { get; set; }
        public DbSet<School_M_Section> accsection { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<School_M_Class>().HasKey(m => m.ASMCL_Id);
            builder.Entity<School_M_Section>().HasKey(m => m.ASMS_Id);

        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<School_M_Class>();
            updateUpdatedProperty<School_M_Section>();

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
