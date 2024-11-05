using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PortalHub.com.vaps.HOD.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODStudentStrengthImpl : Interfaces.HODStudentStrengthInterface
    {
        private static ConcurrentDictionary<string, ADMClassSectionStrengthDTO> _login =
         new ConcurrentDictionary<string, ADMClassSectionStrengthDTO>();

        private readonly PortalContext _ChairmanDashboardContext;

        public DomainModelMsSqlServerContext _db;
        public HODStudentStrengthImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                var Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                          from b in _db.admissioncls
                                          from c in _db.Adm_M_Student
                                          from d in _db.IVRM_HOD_Class_DMO
                                          from e in _db.HOD_DMO
                                          where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == c.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == d.ASMCL_Id && d.IHOD_Id == e.IHOD_Id && e.HRME_Id == loginData.FirstOrDefault().Emp_Code && e.IHOD_Flg == "HOD" && e.IHOD_ActiveFlag == true)
                                          group new { a, b } by a.ASMCL_Id into g
                                          select new ADMClassSectionStrengthDTO
                                          {
                                              ASMAY_Id = g.FirstOrDefault().a.ASMAY_Id,
                                              asmcL_Id = g.FirstOrDefault().a.ASMCL_Id,
                                              Class_Name = g.FirstOrDefault().b.ASMCL_ClassName,
                                              stud_count = g.Count()
                                          }).Distinct().ToList();
                data.Fillstudentstrenth = Fillstudentstrenth.ToArray();
                var ids = Fillstudentstrenth.Select(d => d.asmcL_Id).ToList();


                data.sectionwisestrenth = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           from d in _db.Section
                                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && d.ASMS_Id == a.ASMS_Id && ids.Contains(a.ASMCL_Id) && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                           select new
                                           {

                                               asmcL_Id = b.ASMCL_Id,
                                               Class_Name = b.ASMCL_ClassName,
                                               sectionname = d.ASMC_SectionName,
                                               asmS_Id = a.ASMS_Id,
                                               stud_count = a.AMST_Id
                                           })

                                         .Distinct().GroupBy(id => new
                                         {
                                             id.asmcL_Id,
                                             id.Class_Name,
                                             id.asmS_Id,
                                             id.sectionname

                                         }).Select(g => new ADMClassSectionStrengthDTO
                                         {
                                             asmcL_Id = g.Key.asmcL_Id,
                                             Class_Name = g.Key.Class_Name,
                                             asmS_Id = g.Key.asmS_Id,
                                             sectionname = g.Key.sectionname,
                                             stud_count = g.Count()
                                             // ,ASMAY_Id=g.Key.asmayid
                                         }).ToArray();



            }
            catch (Exception ex)
            {

            }
            return data;

        }


        public ADMClassSectionStrengthDTO Getsectioncount(ADMClassSectionStrengthDTO data)
        {
            try
            {
                data.fillsectioncount = (from a in _db.School_Adm_Y_StudentDMO
                                         from b in _db.admissioncls
                                         from c in _db.Adm_M_Student
                                         from d in _db.Section
                                         where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.classid && d.ASMS_Id == a.ASMS_Id)
                                         select new
                                         {
                                             section = d.ASMC_SectionName,
                                             stud_count = a.AMST_Id
                                         }).Distinct().GroupBy(id => id.section).Select(g => new ADMClassSectionStrengthDTO { section = g.Key, stud_count = g.Count() }).ToArray();
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public ADMClassSectionStrengthDTO Getsection(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
                data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           from d in _db.Section
                                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.classid && d.ASMS_Id == a.ASMS_Id)
                                           select new
                                           {
                                               section = d.ASMC_SectionName,
                                               stud_count = a.AMST_Id
                                           }).Distinct().GroupBy(id => id.section).Select(g => new ADMClassSectionStrengthDTO { section = g.Key, stud_count = g.Count() }).ToArray();

            }
            catch (Exception ex)
            {

            }
            return data;

        }
        public ADMClassSectionStrengthDTO getclass(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _ChairmanDashboardContext.IVRM_HOD_Class_DMO
                                   from d in _ChairmanDashboardContext.HOD_DMO
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.ASMCL_Id == c.ASMCL_Id && c.IHOD_Id == d.IHOD_Id && d.HRME_Id == loginData.FirstOrDefault().Emp_Code && a.AMAY_ActiveFlag == 1
                                   && d.IHOD_Flg == "HOD" && d.IHOD_ActiveFlag == true)

                                   select new ADMClassSectionStrengthDTO
                                   {
                                       Class_Name = b.ASMCL_ClassName,
                                       asmcL_Id = b.ASMCL_Id
                                   }).Distinct().OrderBy(t => t.asmcL_Id).ToArray();
            }
            catch (Exception ex)
            {

            }
            return data;

        }


    }
}
