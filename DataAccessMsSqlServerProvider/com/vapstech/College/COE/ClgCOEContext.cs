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
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.College.COE;
using DomainModel.Model.com.vapstech.FrontOffice;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.COE
{
    public class ClgCOEContext : DbContext
    {
        public ClgCOEContext(DbContextOptions<ClgCOEContext> options) : base(options)
        { }

        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }                       
        public DbSet<COE_Events_ClassesDMO> COE_Events_ClassesDMO { get; set; }
        public DbSet<COE_Events_EmployeesDMO> COE_Events_EmployeesDMO { get; set; }
        public DbSet<COE_Events_ImagesDMO> COE_Events_ImagesDMO { get; set; }
        public DbSet<COE_Events_OthersDMO> COE_Events_OthersDMO { get; set; }
        public DbSet<COE_Events_VideosDMO> COE_Events_VideosDMO { get; set; }
        public DbSet<TT_Master_Staff_AbbreviationDMO> TT_Master_Staff_AbbreviationDMO { get; set; }
        public DbSet<COE_Events_CourseBranchDMO> COE_Events_CourseBranchDMO { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<COE_Master_EventsDMO>().ToTable("COE_Master_Events", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_EventsDMO>().ToTable("COE_Events", "COE");
            base.OnModelCreating(builder);
            builder.Entity<COE_Events_CourseBranchDMO>().ToTable("COE_Events_CourseBranch");
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
            updateUpdatedProperty<COE_Master_EventsDMO>();
            updateUpdatedProperty<COE_EventsDMO>();
            updateUpdatedProperty<COE_Events_CourseBranchDMO>();
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
