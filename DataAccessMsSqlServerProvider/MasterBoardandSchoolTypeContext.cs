using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterBoardandSchoolTypeContext:DbContext
    {
        public MasterBoardandSchoolTypeContext(DbContextOptions<MasterBoardandSchoolTypeContext> options) : base(options)
        { }
        public DbSet<MasterBorad> masterborad { get; set; }
        public DbSet<MasterSchoolType> masterschoolType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterBorad>().HasKey(m => m.IVRMMB_Id);
            builder.Entity<MasterSchoolType>().HasKey(t => t.IVRMMTYP_Id);
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
