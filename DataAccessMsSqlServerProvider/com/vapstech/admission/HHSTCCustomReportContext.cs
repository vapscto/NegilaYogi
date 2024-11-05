using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class HHSTCCustomReportContext : DbContext
    {
        public HHSTCCustomReportContext(DbContextOptions<HHSTCCustomReportContext> options) : base(options)
        { }
        public DbSet<State> statedmo { get; set; }
        public DbSet<MasterAcademic> accyear { get; set; }
        public DbSet<School_M_Class> accclass { get; set; }
        public DbSet<School_M_Section> accsection { get; set; }
        public DbSet<Adm_M_Student>student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO>yearwisestudent { get; set; }
        public DbSet<StudentTC> studenttc { get; set; }
        public DbSet<Religion> religion { get; set; }
        public DbSet<mastercasteDMO> caste { get; set; }
        public DbSet<MasterCompanyDMO> companyname { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<StudentPrevSchoolDMO> StudentPrevSchoolDMO { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CasteCategory> CasteCategory { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        public DbSet<Fee_Y_Payment_School_StudentDMO> Fee_Y_Payment_School_StudentDMO { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<Adm_School_Master_Stream> Adm_School_Master_Stream { get; set; }
    }
}
