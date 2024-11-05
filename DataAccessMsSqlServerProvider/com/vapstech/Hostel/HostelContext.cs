using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider.com.vapstech.Hostel
{
    public class HostelContext : DbContext
    {
        public HostelContext(DbContextOptions<HostelContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<HL_Master_Hostel_DMO> HL_Master_Hostel_DMO { get; set; }
        public DbSet<HL_Hostel_Student_BiometricDMO> HL_Hostel_Student_BiometricDMO { get; set; }
        public DbSet<HL_Hostel_Student_Biometric_DetailsDMO> HL_Hostel_Student_Biometric_DetailsDMO { get; set; }
        public DbSet<HL_Hostel_GatePass_DMO> HL_Hostel_GatePass_DMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> Adm_School_Y_StudentDMO { get; set; }
        public DbSet<School_M_Class> Adm_School_M_ClassDMO { get; set; }
        public DbSet<School_M_Section> school_M_Section { get; set; }         
        public DbSet<Hostel_Student_GatePass_ApprovalDMO> Hostel_Student_GatePass_ApprovalDMO { get; set; }
        public DbSet<HL_Master_Bed_DMO> HL_Master_Bed_DMO { get; set; }
        public DbSet<HR_Master_Floor_DMO> HR_Master_Floor_DMO { get; set; }
        public DbSet<HR_Master_Room_DMO> HR_Master_Room_DMO { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<CLGADM_master_courseDMO> CLGADM_master_courseDMO { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }
        public DbSet<HL_Master_Facility_DMO> HL_Master_Facility_DMO { get; set; }
        public DbSet<HL_Master_MessCategory_DMO> HL_Master_MessCategory_DMO { get; set; }
        public DbSet<HL_Master_Mess_DMO> HL_Master_Mess_DMO { get; set; }
        public DbSet<HL_Master_Hostel_Photos_DMO> HL_Master_Hostel_Photos_DMO { get; set; }
        public DbSet<HL_Master_Floor_Facilities_DMO> HL_Master_Floor_Facilities_DMO { get; set; }
        public DbSet<HL_Master_Room_Facilities_DMO> HL_Master_Room_Facilities_DMO { get; set; }
        public DbSet<HL_Master_Hostel_Mess_DMO> HL_Master_Hostel_Mess_DMO { get; set; }
        public DbSet<HL_Master_Hostel_Facilities_DMO> HL_Master_Hostel_Facilities_DMO { get; set; }
        public DbSet<HL_Master_Hostel_Warden_DMO> HL_Master_Hostel_Warden_DMO { get; set; }
        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<HL_Master_MessMenu_DMO> HL_Master_MessMenu_DMO { get; set; }
        public DbSet<HL_Master_Room_Category_DMO> HL_Master_Room_Category_DMO { get; set; }
        public DbSet<HL_Master_Room_Tariff_DMO> HL_Master_Room_Tariff_DMO { get; set; }
        public DbSet<HlMasterRoom_FeeGroupDMO> HL_Master_Room_FeeGroup_DMO { get; set; }
        public DbSet<HL_Hostel_Student_Request_DMO> HL_Hostel_Student_Request_DMO { get; set; }
        public DbSet<HL_Hostel_Staff_Request_DMO> HL_Hostel_Staff_Request_DMO { get; set; }
        public DbSet<HL_Hostel_Student_Request_Confirm_DMO> HL_Hostel_Student_Request_Confirm_DMO { get; set; }
        public DbSet<HL_Hostel_Staff_Request_Confirm_DMO> HL_Hostel_Staff_Request_Confirm_DMO { get; set; }
        public DbSet<HL_Hostel_Staff_Allot_DMO> HL_Hostel_Staff_Allot_DMO { get; set; }
        public DbSet<HL_Hostel_Guest_Allot_DMO> HL_Hostel_Guest_Allot_DMO { get; set; }
        public DbSet<HL_Hostel_Student_Allot_DMO> HL_Hostel_Student_Allot_DMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }

        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<HL_Hostel_Student_Request_College_DMO> HL_Hostel_Student_Request_College_DMO { get; set; }
        public DbSet<HL_Hostel_Student_Request_College_Confirm_DMO> HL_Hostel_Student_Request_College_Confirm_DMO { get; set; }
        public DbSet<HL_Hostel_Student_Allot_College_DMO> HL_Hostel_Student_Allot_College_DMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }

        public DbSet<Institution> Institute { get; set; }

        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<HL_Master_Mess_MessCategoryDMO> HL_Master_Mess_MessCategoryDMO { get; set; }
        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfigurationDMO { get; set; }
         public DbSet<HL_Hostel_Student_Transfer_CollegeDMO> HL_Hostel_Student_Transfer_CollegeDMO { get; set; }
         
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
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }

    }
}
