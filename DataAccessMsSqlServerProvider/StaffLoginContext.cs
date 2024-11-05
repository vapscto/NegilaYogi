using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel;
using DomainModel.Model.com.vapstech.MobileApp;

namespace DataAccessMsSqlServerProvider
{
    public class StaffLoginContext:DbContext
    {
        public StaffLoginContext(DbContextOptions<StaffLoginContext> options) :base(options)
        { }

        public DbSet<MasterEmployee> masterStaff { get; set; }
        public DbSet<Multiple_Mobile_DMO> Staffmultiplemobile { get; set; }

        public DbSet<Multiple_Email_DMO> Staffmultipleemail  { get; set; }
        public DbSet<Institution> institution { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<MasterRoleType> masterRoleType { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<MasterCategory> masterCategory { get; set; }
        public DbSet<StaffLoginDMO> staffLoginDMO { get; set; }
        public DbSet<MasterPageModuleMapping> masterPageModuleMapping { get; set; }
        public DbSet<MasterPage> masterPage { get; set; }
        public DbSet<MasterRolePreviledgeDMO> masterRolePreviledgeDMO { get; set; }
        public DbSet<Institution_Module> institution_Module { get; set; }
        public DbSet<Institution_Module_Page> institution_Module_Page { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<Dashboard_page_mapping> Dashboard_page_mapping { get; set; }
        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }

        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }
        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserLogin> userLogin { get; set; }
        public DbSet<UserLoginEmployee> userLoginEmployee { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<InstitutionRolePrivileges> institutionRolePrivileges { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<ApplRole> approle { get; set; }

        public DbSet<UserLoginPrivileges> UserLoginPrivileges { get; set; }

        public DbSet<Institution_MobileNo> institutemobile { get; set; }

        public DbSet<Institution_EmailId> instituteemail { get; set; }

        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }

        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<StaffLoginDMO>()
               .ToTable("IVRM_Staff_User_Privileges");

            // base.OnModelCreating(builder);

            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<StaffLoginDMO>();

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
