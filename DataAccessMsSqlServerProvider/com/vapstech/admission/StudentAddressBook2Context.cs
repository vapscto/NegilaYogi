﻿using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class StudentAddressBook2Context:DbContext
    {
       

        public StudentAddressBook2Context(DbContextOptions<StudentAddressBook2Context> options) :base(options)
          
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            //builder.Entity<studentbirthdayreportDMO>().HasKey(m => m.AMST_AdmNo);
            builder.Entity<Adm_M_Student>().HasKey(m => m.AMST_Id);
            builder.Entity<School_M_Class>().HasKey(m => m.ASMCL_Id);
            builder.Entity<School_M_Section>().HasKey(m => m.ASMS_Id);
            builder.Entity<School_Adm_Y_StudentDMO>().HasKey(m => m.ASYST_Id);

        }

        public DbSet<AcademicYear> year { get; set; }
        public DbSet<AdmissionStandardDMO> School_M_ClassDMO { get; set; }
        public DbSet<School_M_Class> AdmClass { get; set; }
        public DbSet<School_M_Section> admsection { get; set; }
        public DbSet<Adm_M_Student> student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Masterclasscategory> masterclasscategory { get; set; }      


    }
}
