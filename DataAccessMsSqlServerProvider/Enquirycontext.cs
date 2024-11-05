using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.admission;

namespace DataAccessMsSqlServerProvider
{
    public class Enquirycontext:DbContext
    {
        public Enquirycontext(DbContextOptions<Enquirycontext> options) :base(options)
        { }

        public DbSet<Enquiry> Enquir { get; set; }

        public DbSet<Course> course { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<City> city { get; set; }
        //Sripad Added
        public DbSet<MasterAcademic> academicyear { get; set; }

        public DbSet<TransactionNumbering> TransactionNumbering { get; set; }
        public DbSet<Transaction_Numbering_Type> Transaction_Numbering_Type { get; set; }
        public DbSet<Institution> Institution { get; set; }

       // public DbSet<AcademicYear> AcademicYear { get; set; }

        public DbSet<Master_Numbering> Master_Numbering { get; set; } //11/11/2016

        
  
        public DbSet<StudentUserLoginDMO> StudentUserLoginDMO { get; set; }
   
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
   


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Enquiry>()
               .ToTable("Preadmission_School_Enquiry");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<StudentUserLoginDMO>().HasKey(m => m.IVRMSTUUL_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Enquiry>();

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
