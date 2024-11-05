using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MarksReportContext : DbContext
    {
        public MarksReportContext(DbContextOptions<MarksReportContext> options) :base(options)
        { }
        public MarksReportContext()
        {
        }
        public DbSet<StudentDetailsDMO> StudentDetailsDMO { get; set; }

       public DbSet<WrittenTestScheduleDMO> writentest { get; set; }
        public DbSet<OralTestScheduleDMO> oraltest { get; set; }
        public DbSet<MasterSubjectDMO> mastersubject { get; set; }

        public DbSet<IVRM_Master_SubjectsDMO> allSubject { get; set; }
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }
        public DbSet<CasteCategory> castecategory { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }
            //public DbSet<OralTestOralByMarksDMO> OralTestOralByMarksDMO { get; set; }
        //public DbSet<OralTestScheduleMarksMapDMO> OralTestScheduleMarksMapDMO { get; set; }
        public DbSet<OralTestStudentWiseMarksDMO> OralTestStudentWiseMarksDMO { get; set; }
        public DbSet<OralTestScheduleStudentInsertDMO> OralTestScheduleStudentInsertDMO { get; set; }

        public DbSet<WrittenTestStudentSubjectWiseMarksDMO> WrittenTestStudentSubjectWiseMarksDMO { get; set; }
        public DbSet<WrittenTestScheduleStudentInsertDMO> WrittenTestScheduleStudentInsertDMO { get; set; }
        public DbSet<WIrttenTestSubjectWiseMarksDMO> WIrttenTestSubjectWiseMarksDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

    }
}
