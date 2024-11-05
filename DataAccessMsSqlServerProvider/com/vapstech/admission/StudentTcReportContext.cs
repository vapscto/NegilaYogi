using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class StudentTcReportContext: DbContext
    {
        public StudentTcReportContext(DbContextOptions<StudentTcReportContext> options) :base(options)
        { }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<School_M_Class> admissioncls { get; set; }

        public DbSet<School_M_Section> school_M_Section { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudent { get; set; }

        public DbSet<StudentTC> studenttc { get; set; }
        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }

        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<MasterCategory> category { get; set; }

    }
}
