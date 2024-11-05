using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using PreadmissionDTOs;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class studentbirthdayreportContext: DbContext
    {

        public studentbirthdayreportContext(DbContextOptions<studentbirthdayreportContext> options) :base(options)
          
        { }

        //public DbSet<studentbirthdayreportDMO> studentbirthdayreportDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            //builder.Entity<studentbirthdayreportDMO>().HasKey(m => m.AMST_AdmNo);
            builder.Entity<Adm_M_Student>().HasKey(m => m.AMST_AdmNo);
            builder.Entity<School_M_Class>().HasKey(m => m.ASMCL_Id);
            builder.Entity<School_M_Section>().HasKey(m => m.ASMS_Id);
            builder.Entity<School_Adm_Y_StudentDMO>().HasKey(m => m.ASYST_Id);

        }

        public DbSet<AdmissionStandardDMO> School_M_ClassDMO { get; set; }
        public DbSet<School_M_Class> AdmClass { get; set; }
        public DbSet<School_M_Section> admsection { get; set; }
        public DbSet<Adm_M_Student> student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> yearstudent { get; set; }
        public DbSet<AcademicYear> year { get; set; }

        // public DbSet<StudentApplication> StudentApplication { get; set; }
        //public DbSet<MasterSubjectDMO> MasterSubjectDMO { get; set; }
        //public DbSet<MasterConfiguration> MasterConfiguration { get; set; }


    }


    }

