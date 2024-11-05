using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Placement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.Placement
{
  public  class PlacementContext : DbContext
    {
        public PlacementContext(DbContextOptions<PlacementContext> options) : base(options)
        {
            Database.SetCommandTimeout(30000);
        }
      //  public DbSet<IVRM_User_Login_StateDMO> IVRM_User_Login_StateDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<PL_Master_CompanyDMO> PL_Master_CompanyDMO { get; set; }
     
        public DbSet<PL_Master_Company_ContactDMO> PL_Master_Company_ContactDMO { get; set; }
        public DbSet<PL_CI_Schedule_CompanyDMO> PL_CI_Schedule_CompanyDMO { get; set; }
        public DbSet<PL_CI_Schedule_Company_JobTitle_StudentsDMO> PL_CI_Schedule_Company_JobTitle_StudentsDMO { get; set; }
        public DbSet<PL_CI_Schedule_Company_JobTitle_Students_StatusDMO> PL_CI_Schedule_Company_JobTitle_Students_StatusDMO { get; set; }
        public DbSet<PL_CampusInterview_ScheduleDMO> PL_CampusInterview_ScheduleDMO { get; set; }
        public DbSet<semmarkDMO> semmarkDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<mappingDMO> mappingDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
  
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<PL_CI_Schedule_Company_JobTitle_CriteriaDMO> PL_CI_Schedule_Company_JobTitle_CriteriaDMO { get; set; }

    
        public DbSet<PL_CI_Schedule_Company_JobTitleDMO> PL_CI_Schedule_Company_JobTitleDMO { get; set; }
        public DbSet<PL_CI_Schedule_Company_JobTitle_CourseBranchDMO> PL_CI_Schedule_Company_JobTitle_CourseBranchDMO { get; set; }

        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }
        // public DbSet<applicationRole> applicationRole { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);


        }
    }
}
