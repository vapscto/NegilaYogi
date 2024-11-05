using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Exam;

namespace DataAccessMsSqlServerProvider
{
    public class StudentApplicationContext : DbContext
    {
        public StudentApplicationContext(DbContextOptions<StudentApplicationContext> options) : base(options)
        { Database.SetCommandTimeout(300000000); }
        public DbSet<StudentApplication> Enq { get; set; }
        public DbSet<StudentGuardian> st_grdn { get; set; }
        public DbSet<StudentPreviousSchool> stprev { get; set; }
        public DbSet<StudentSibling> stsblng { get; set; }
        public DbSet<StudentTransport> sttrns { get; set; }
        public DbSet<StudentUploadImage> mstDoc { get; set; }
        public DbSet<StudentTrnxDoc> trxDoc { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<Preadmission_School_Registration_Employee> Preadmission_School_Registration_Employee { get; set; }
        public DbSet<PA_Student_Vaccines> PA_Student_Vaccines { get; set; }

        public DbSet<PA_Master_Vaccines> PA_Master_Vaccines { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> MasterSubjectDMO { get; set; }

        public DbSet<Exm_Master_GroupDMO> Exm_Master_GroupDMO { get; set; }

        public DbSet<Exm_Master_Group_SubjectsDMO> Exm_Master_Group_SubjectsDMO { get; set; }

        public DbSet<PA_School_Application_ElectiveSujects> PA_School_Application_ElectiveSujects { get; set; }

        public DbSet<Adm_M_Category> Adm_M_Category { get; set; }

        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<Route> route { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<GenCategory> genCategory { get; set; }

        public DbSet<PointsDMO> PointsDMO { get; set; }

        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<Preadmission_Special_Registration> Preadmission_Special_Registration { get; set; }

        public DbSet<Preadmission_App_Points_AgeDMO> PAstudentage { get; set; }

        public DbSet<Preadmission_App_Points_IncomeDMO> PAstudentincome { get; set; }

        public DbSet<Preadmission_App_Points_casteDMO> PAstudentcatse { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }

        public DbSet<Caste> caste { get; set; }

        public DbSet<MasterSource> MasterSource { get; set; }

        public DbSet<PreadmissionStudnetSource> PreadmissionStudnetSource { get; set; }

        public DbSet<PAStudentEmployee> PAStudentEmployee { get; set; }

        public DbSet<subCaste> subcaste  { get; set; }
        public DbSet<Religion> religion { get; set; }
        public DbSet<CasteCategory> castecategory { get; set; }

        public DbSet<Adm_School_Master_Stream> Master_Streams { get; set; }

        // 30-9-2016
        public DbSet<MasterConfiguration> masterConfig { get; set; }
        // 10-11-2016
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Institution> Inst { get; set; }
        public DbSet<School_M_Section> Section { get; set; }

        public DbSet<StudentSibling> StudentSibling { get; set; }

        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }
        public DbSet<MasterDocumentDMO> MasterDocumentDMO { get; set; }

        public DbSet<Master_Numbering> Master_Numbering { get; set; } //16/12/2016


        public DbSet<Pre_Adm_Syllabus> Pre_Adm_Syllabus { get; set; } //20/12/2016

        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; } //08/03/2017

        public DbSet<PA_Student_Sibblings> PA_Student_Sibblings { get; set; }


        // 15-12-2016
        // public DbSet<PreadmissionSchoolRegistrationDocuments> PreadmissionSchoolRegistrationDocuments { get; set; }
        public DbSet<Preadmission_Cast_Doc_MappingDMO> Preadmission_Cast_Doc_MappingDMO { get; set; }

        public DbSet<PreadmissionSchoolRegistrationStudentLogin> PreadmissionSchoolRegistrationStudentLogin { get; set; }
        public DbSet<StudentHelthcertificateDMO> StudentHelthcertificate { get; set; }


        public DbSet<Preadmission_Dashboard_PageDMO> dashboard { get; set; }

        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }


        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<MasterRouteDMO> MasterRouteDMO { get; set; }
        public DbSet<Route_Location> Route_Location { get; set; }
        public DbSet<MasterLocationDMO> MasterLocationDMO { get; set; }
        public DbSet<PA_Student_Transport_ApplicationDMO> PA_Student_Transport_ApplicationDMO { get; set; }

        public DbSet<PA_School_Application_ProspectusDMO> PA_School_Application_ProspectusDMO { get; set; }

        //Addded by roopa
        public DbSet<PA_School_Application_Reference> PA_School_Application_Reference { get; set; }
        public DbSet<PA_School_Application_Source> PA_School_Application_Source { get; set; }
        public DbSet<MasterSource> Preadmission_Master_Source { get; set; }
        public DbSet<MasterReference> Preadmission_Master_Reference { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<StudentApplication>().ToTable("Preadmission_School_Registration");

            // Added on 20-9-2016
            base.OnModelCreating(builder);
            builder.Entity<StudentGuardian>().ToTable("Preadmission_School_Registration_Guardian");

            base.OnModelCreating(builder);
            builder.Entity<StudentSibling>().ToTable("Preadmission_School_Registration_SiblingsDetails");

            base.OnModelCreating(builder);
            builder.Entity<StudentPreviousSchool>().ToTable("Preadmission_School_Registration_PrevSchool");

            base.OnModelCreating(builder);
            builder.Entity<StudentTransport>().ToTable("Preadmission_School_Registration_TransportDetails");

            base.OnModelCreating(builder);
            builder.Entity<StudentUploadImage>().ToTable("Preadmission_School_Master_Documents");

            base.OnModelCreating(builder);
            builder.Entity<StudentTrnxDoc>().ToTable("Preadmission_School_Registration_Documents");

            //base.OnModelCreating(builder);
            //builder.Entity<StudentGuardian>().ToTable("Preadmission_School_Master_Documents");
            // Added on 20-9-2016
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<StudentApplication>();
            updateUpdatedProperty<StudentGuardian>();
            updateUpdatedProperty<StudentSibling>();
            updateUpdatedProperty<StudentPreviousSchool>();
            updateUpdatedProperty<StudentUploadImage>();
            updateUpdatedProperty<StudentTrnxDoc>();
            updateUpdatedProperty<StudentTransport>();
            updateUpdatedProperty<PA_School_Application_Reference>();
            updateUpdatedProperty<PA_School_Application_Source>();


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
