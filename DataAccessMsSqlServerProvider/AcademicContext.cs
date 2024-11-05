using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class AcademicContext: DbContext
    {
        public AcademicContext(DbContextOptions<AcademicContext> options) : base(options)
        { }
        public DbSet<MasterAcademic> Academic { get; set; }
        public DbSet<Institution> institution { get; set; }

        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Adm_M_Student> adm_m_student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> adm_Y_student { get; set; }
        public DbSet<StudentApplication> preadmission { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MasterAcademic>().HasKey(m => m.ASMAY_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterAcademic>();

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
