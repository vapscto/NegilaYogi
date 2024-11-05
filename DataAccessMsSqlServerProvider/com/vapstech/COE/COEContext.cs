using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
//using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.COE;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Alumni;

namespace DataAccessMsSqlServerProvider.com.vapstech.COE
{
    public class COEContext : DbContext
    {
        public COEContext(DbContextOptions<COEContext> options) :base(options)
        { }

        // public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<COE_Events_ClassesDMO> COE_Events_ClassesDMO { get; set; }
        public DbSet<COE_Events_EmployeesDMO> COE_Events_EmployeesDMO { get; set; }
        public DbSet<COE_Events_ImagesDMO> COE_Events_ImagesDMO { get; set; }
        public DbSet<COE_Events_OthersDMO> COE_Events_OthersDMO { get; set; }
        public DbSet<COE_Events_VideosDMO> COE_Events_VideosDMO { get; set; }
        public DbSet<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }
        public DbSet<COE_Events_SMSMailPN_DMO> COE_Events_SMSMailPN_DMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //base.OnModelCreating(builder);
            //builder.Entity<TTMasterCategoryDMO>().HasKey(m => m.TTMC_Id);
            base.OnModelCreating(builder);
            builder.Entity<COE_Master_EventsDMO>().ToTable("COE_Master_Events", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_EventsDMO>().ToTable("COE_Events","COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_ClassesDMO>().ToTable("COE_Events_Classes", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_EmployeesDMO>().ToTable("COE_Events_Employees", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_ImagesDMO>().ToTable("COE_Events_Images", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_OthersDMO>().ToTable("COE_Events_Others", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_VideosDMO>().ToTable("COE_Events_Videos", "COE");


        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            //updateUpdatedProperty<TTMasterCategoryDMO>();
            updateUpdatedProperty<COE_Master_EventsDMO>();
            updateUpdatedProperty<COE_EventsDMO>();
            updateUpdatedProperty<COE_Events_ClassesDMO>();
            updateUpdatedProperty<COE_Events_EmployeesDMO>();
            updateUpdatedProperty<COE_Events_ImagesDMO>();
            updateUpdatedProperty<COE_Events_OthersDMO>();
            updateUpdatedProperty<COE_Events_VideosDMO>();

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
