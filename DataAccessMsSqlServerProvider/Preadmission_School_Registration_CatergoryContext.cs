using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataAccessMsSqlServerProvider
{
    public class Preadmission_School_Registration_CatergoryContext : DbContext
    {
        public Preadmission_School_Registration_CatergoryContext(DbContextOptions<Preadmission_School_Registration_CatergoryContext> options) : base(options)
        { }
        public DbSet<Preadmission_School_Registration_CatergoryDMO> Preadmission_School_Registration_CatergoryDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }
        public DbSet<StudentSibling> StudentSibling { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }

        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }

        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }

        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }


        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }

        public DbSet<PA_Student_Sibblings> PA_Student_Sibblings { get; set; }
        public DbSet<Preadmission_School_Registration_Concession_StatusDMO> Preadmission_School_Registration_Concession_StatusDMO { get; set; }

        public DbSet<PA_Student_Sibblings_Details> PA_Student_Sibblings_Details { get; set; }

        public DbSet<PAStudentEmployee> PAStudentEmployee { get; set; }

        public DbSet<Preadmission_School_Registration_Employee> Preadmission_School_Registration_Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Preadmission_School_Registration_Concession_StatusDMO>().HasKey(m => m.PSRCS_ID);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Preadmission_School_Registration_Concession_StatusDMO>();

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
