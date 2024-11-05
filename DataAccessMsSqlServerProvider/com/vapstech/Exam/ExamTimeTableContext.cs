using DomainModel.Model.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.Exam
{
    public class ExamTimeTableContext : DbContext
    {
        public ExamTimeTableContext(DbContextOptions<ExamTimeTableContext> options) : base(options)
        { }
        public ExamTimeTableContext()
        {
        }
        public DbSet<Exm_TT_M_SessionDMO> Exm_TT_M_SessionDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Exm_TT_M_SessionDMO>().ToTable("Exm_TT_M_Session", "Exm");

            base.OnModelCreating(builder);
            builder.Entity<Exm_TT_M_SessionDMO>().HasKey(m => m.ETTS_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            updateUpdatedProperty<Exm_TT_M_SessionDMO>();
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
