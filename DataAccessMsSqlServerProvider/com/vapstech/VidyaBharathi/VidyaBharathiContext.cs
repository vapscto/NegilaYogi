using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi
{
    public class VidyaBharathiContext : DbContext
    {
        public VidyaBharathiContext(DbContextOptions<VidyaBharathiContext> options) : base(options)
        {
            Database.SetCommandTimeout(30000);
        }
        public DbSet<IVRM_User_Login_StateDMO> IVRM_User_Login_StateDMO { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Institution> Institute { get; set; }
        public DbSet<Month> IVRM_Month_DMO { get; set; }
        public DbSet<VBSC_Master_Competition_CategoryDMO> VBSC_Master_Competition_CategoryDMO { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<MasterCompetitionCategory_ClassesDMO> MasterCompetitionCategory_ClassesDMO { get; set; }
        public DbSet<VBSC_Master_Competition_Category_LevelsDMO> VBSC_Master_Competition_Category_LevelsDMO { get; set; }
        public DbSet<VBSC_Master_Competition_LevelDMO> VBSC_Master_Competition_LevelDMO { get; set; }
        public DbSet<VBSC_Master_EventsDMO> VBSC_Master_EventsDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<IVRM_User_Login_DistrictDMO> IVRM_User_Login_DistrictDMO { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<ApplicationUserDMO> ApplicationUserDMO { get; set; }
        public DbSet<VBSC_Events_CategoryDMO> VBSC_Events_CategoryDMO { get; set; }
        public DbSet<VBSC_Master_SportsCCName_UOMDMO> VBSC_Master_SportsCCName_UOMDMO { get; set; }
        public DbSet<VBSC_Master_UOMDMO> VBSC_Master_UOMDMO { get; set; }
        public DbSet<VBSC_Master_SportsCCNameDMO> VBSC_Master_SportsCCNameDMO { get; set; }
        public DbSet<VBSC_EventsDMO> VBSC_EventsDMO { get; set; }
        public DbSet<VBSC_Master_SportsCCGroupNameDMO> VBSC_Master_SportsCCGroupNameDMO { get; set; }
        public DbSet<VBSC_Events_Category_StudentsDMO> VBSC_Events_Category_StudentsDMO { get; set; }
    
        public DbSet<Institution> MasterInstitute { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

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

            
        }
    }
}
