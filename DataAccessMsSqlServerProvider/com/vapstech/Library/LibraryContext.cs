using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Library;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.FrontOffice;

namespace DataAccessMsSqlServerProvider.com.vapstech.Library
{
    public class LibraryContext :DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) :base(options)
        {
            Database.SetCommandTimeout(30000);
        }
        public  DbSet<MasterCategoryDMO> MasterCategoryDMO { get; set; }
        public DbSet<MasterDepartmentDMO> MasterDepartmentDMO { get; set; }
        public DbSet<MasterPeriodicityDMO> MasterPeriodicityDMO { get; set; }
        public DbSet<MasterFloorDMO> MasterFloorDMO { get; set; }
        public DbSet<MasterLanguageDMO> MasterLanguageDMO { get; set; }
        public DbSet<MasterSubject_DMO> MasterSubject_DMO { get; set; }
        public DbSet<MasterPublisherDMO> MasterPublisherDMO { get; set; }
        public DbSet<IVRM_Master_Subjects_DMO> IVRM_Master_Subjects_DMO { get; set; }
        public DbSet<RackDetailsDMO> RackDetailsDMO { get; set; }
        public DbSet<Lib_Rack_SubjectDMO> Lib_Rack_SubjectDMO { get; set; }
        public DbSet<CirculationParameterDMO> CirculationParameterDMO { get; set; }
        public DbSet<MasterTimeSlabDMO> MasterTimeSlabDMO { get; set; }
        public DbSet<MasterVanderDMO> MasterVanderDMO { get; set; }
        public  DbSet<MasterGuestDMO> MasterGuestDMO { get; set; }
        public DbSet<MasterAuthorDMO> MasterAuthorDMO { get; set; }
        public DbSet<BookRegisterDMO> BookRegisterDMO { get; set; }
        public DbSet<Lib_M_Book_Accn_DMO> Lib_M_Book_Accn_DMO { get; set; }
        public DbSet<MasterDonorDMO> MasterDonorDMO { get; set; }
        public DbSet<School_M_Class> Adm_School_M_ClassDMO { get; set; }
        public DbSet<BookTransactionDMO> BookTransactionDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<School_M_Section> school_M_Section { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }

        public DbSet<LIB_Master_Book_VendorDMO> LIB_Master_Book_VendorDMO { get; set; }

       
        public DbSet<LIB_Master_Library_DMO> LIB_Master_Library_DMO { get; set; }
        public DbSet<LIB_Master_Accessories_DMO> LIB_Master_Accessories_DMO { get; set; }
        public DbSet<LIB_User_Library_DMO> LIB_User_Library_DMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<LIB_Master_Book_Library_DMO> LIB_Master_Book_Library_DMO { get; set; }
        public DbSet<LIB_Master_Book_Accessories_DMO> LIB_Master_Book_Accessories_DMO { get; set; }

        public DbSet<AcademicYear> AcademicYearDMO { get; set; }
        public DbSet<LIB_Master_Author_DMO> LIB_Master_Author_DMO { get; set; }

        //==================Praveen
        public DbSet<LIB_Book_Circulation_ParameterDMO> LIB_Book_Circulation_ParameterDMO { get; set; }
        public DbSet<LIB_NonBook_Circulation_Parameter_StudentDMO> LIB_NonBook_Circulation_Parameter_StudentDMO { get; set; }
        public DbSet<LIB_NonBook_Circulation_Parameter_StaffDMO> LIB_NonBook_Circulation_Parameter_StaffDMO { get; set; }
        public DbSet<LIB_Circulation_Parameter_StudentDMO> LIB_Circulation_Parameter_StudentDMO { get; set; }
        public DbSet<LIB_Book_Transaction_StudentDMO> LIB_Book_Transaction_StudentDMO { get; set; }
        public DbSet<LIB_Book_Transaction_StaffDMO> LIB_Book_Transaction_StaffDMO { get; set; }
        public DbSet<LIB_Book_Transaction_DepartmentDMO> LIB_Book_Transaction_DepartmentDMO { get; set; }
        public DbSet<LIB_Circulation_Parameter_OthersDMO> LIB_Circulation_Parameter_OthersDMO { get; set; }
        public DbSet<LIB_Circulation_Parameter_StaffDMO> LIB_Circulation_Parameter_StaffDMO { get; set; }
        public DbSet<LIB_NonBook_Circulation_Parameter_OthersDMO> LIB_NonBook_Circulation_Parameter_OthersDMO { get; set; }
        public DbSet<LIB_NonBook_Circulation_ParameterDMO> LIB_NonBook_Circulation_ParameterDMO { get; set; }
        public DbSet<HR_Master_GroupTypeDMO> HR_Master_GroupTypeDMO { get; set; }
        public DbSet<LIB_Circulation_Parameter_Student_CollegeDMO> LIBCirculationParameterStudentCollegeDMO { get; set; }
        public DbSet<LIB_Book_Transaction_Student_CollegeDMO> LIB_Book_Transaction_Student_CollegeDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Institution> Institute { get; set; }
        public DbSet<LIB_ELibraryLinksDMO> AddELibraryLinksDMO { get; set; }

        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }        public DbSet<School_M_Section> School_M_Section { get; set; }        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }        public DbSet<FO_Biometric_DeviceDMO> FO_Biometric_DeviceDMO { get; set; }


        //=========================        

        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<LIB_Library_Class_DMO> LIB_Library_Class_DMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<ApplUser> ApplicationUser { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Month> MonthDMO { get; set; }
        public DbSet<LIB_Master_Subscription_DMO> LIB_Master_Subscription_DMO { get; set; }

        public DbSet<LIB_Master_NonBook_DMO> LIB_Master_NonBook_DMO { get; set; }
        public DbSet<LIB_Master_NonBook_AccnNo_DMO> LIB_Master_NonBook_AccnNo_DMO { get; set; }
        public DbSet<LIB_Master_NonBook_KeyFactor_DMO> LIB_Master_NonBook_KeyFactor_DMO { get; set; }
        public DbSet<LIB_Master_NonBook_Library_DMO> LIB_Master_NonBook_Library_DMO { get; set; }
        public DbSet<LIB_Master_NonBook_Accessories_DMO> LIB_Master_NonBook_Accessories_DMO { get; set; }
        public DbSet<LIB_Master_Book_KeyFactor_DMO> LIB_Master_Book_KeyFactor_DMO { get; set; }
        public DbSet<ImageClipping_DMO> ImageClipping_DMO { get; set; }
        public DbSet<LIB_NonBook_Transaction_DMO> LIB_NonBook_Transaction_DMO { get; set; }
        public DbSet<LIB_NonBook_Transaction_Student_DMO> LIB_NonBook_Transaction_Student_DMO { get; set; }
        public DbSet<LIB_NonBook_Transaction_Staff_DMO> LIB_NonBook_Transaction_Staff_DMO { get; set; }
        public DbSet<LIB_NonBook_Transaction_Department_DMO> LIB_NonBook_Transaction_Department_DMO { get; set; }
        public DbSet<LIB_NonBook_Circulation_Parameter_Student_CollegeDMO> LIB_NonBook_Circulation_Parameter_Student_CollegeDMO { get; set; }
        public DbSet<LIB_NonBook_Transaction_Student_College_DMO> LIB_NonBook_Transaction_Student_College_DMO { get; set; }
        public DbSet<LIB_ConfigurationDMO> LIB_ConfigurationDMO { get; set; }
        public DbSet<Adm_Student_PunchDMO> Adm_Student_PunchDMO { get; set; }
        public DbSet<Adm_Student_Punch_CollegeDMO> Adm_Student_Punch_CollegeDMO { get; set; }
        //added By sanjeev
        public DbSet<Master_Book_FilesDMO> Master_Book_FilesDMO { get; set; }
        public DbSet<Adm_Student_Punch_DetailsDMO> Adm_Student_Punch_DetailsDMO { get; set; }
        public DbSet<Adm_Student_Punch_College_DetailsDMO> Adm_Student_Punch_College_DetailsDMO { get; set; }
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

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }
    }
}
