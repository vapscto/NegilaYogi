using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class ClasswisestudentdetailsContext:DbContext
    {
        public ClasswisestudentdetailsContext(DbContextOptions<ClasswisestudentdetailsContext> options) :base(options)
        { }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<School_M_Class> admissioncls { get; set; }

        public DbSet<School_M_Section> school_M_Section { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudent { get; set; }
    }
}
