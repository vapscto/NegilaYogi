using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODExamSectionPerformanceImpl : Interfaces.HODExamSectionPerformanceInterface
    {

        private readonly PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        public ExamContext _exm;
        public HODExamSectionPerformanceImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext para)
        {
            _PortalContext = cpContext;
            _db = db;
            _exm = para;
        }

        public HODExamSectionPerformance_DTO Getdetails(HODExamSectionPerformance_DTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;

        }

        public HODExamSectionPerformance_DTO getclassexam(HODExamSectionPerformance_DTO data)
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                data.classlist = (from a in _PortalContext.School_M_Class
                                  from b in _PortalContext.Exm_Category_ClassDMO
                                  from h in _PortalContext.HOD_DMO
                                  from hc in _PortalContext.IVRM_HOD_Class_DMO

                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && h.IHOD_Id == hc.IHOD_Id && hc.ASMCL_Id == a.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && h.HRME_Id == loginData.FirstOrDefault().Emp_Code && b.ECAC_ActiveFlag.Equals(true) && h.IHOD_Flg == "HOD" && h.IHOD_ActiveFlag == true  && a.ASMCL_ActiveFlag.Equals(true))
                                  select new HODExamSectionPerformance_DTO
                                  {
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Id = a.ASMCL_Id
                                  }).Distinct().ToArray();

                data.exmstdlist = (from a in _PortalContext.Exm_Yearly_CategoryDMO
                                   from b in _PortalContext.Exm_Yearly_Category_ExamsDMO
                                   from c in _PortalContext.exammasterDMO

                                   where (a.MI_Id == c.MI_Id && b.EME_Id == c.EME_Id && a.EYC_Id == b.EYC_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && c.EME_ActiveFlag.Equals(true))
                                   select new HODExamSectionPerformance_DTO
                                   {
                                       EME_ExamName = c.EME_ExamName,
                                       EME_Id = c.EME_Id
                                   }).Distinct().OrderBy(t => t.EME_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HODExamSectionPerformance_DTO getcategory(HODExamSectionPerformance_DTO data)
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();

                List<long> assignEmpclass = new List<long>();
                List<long> emca_idss = new List<long>();
                var assignclass = (from a in _PortalContext.HOD_DMO
                                   from b in _PortalContext.IVRM_HOD_Class_DMO
                                   where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == loginData.FirstOrDefault().Emp_Code && a.MI_Id == data.MI_Id && a.IHOD_Flg == "HOD" && a.IHOD_ActiveFlag == true)
                                   select new HODExamTopper_DTO
                                   {
                                       ASMCL_Id = b.ASMCL_Id,
                                   }).Distinct().ToList();

                if (assignclass.Count > 0)
                {
                    foreach (var classid in assignclass)
                    {
                        assignEmpclass.Add(classid.ASMCL_Id);
                    }
                }

                var categid = (from t in _PortalContext.Exm_Category_ClassDMO
                               where (t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && assignEmpclass.Contains(t.ASMCL_Id))
                               select new HODExamTopper_DTO
                               {
                                   EMCA_Id = t.EMCA_Id,
                               }).Distinct().ToList();

                if (categid.Count > 0)
                {
                    foreach (var emca in categid)
                    {
                        emca_idss.Add(emca.EMCA_Id);
                    }
                }


                data.fillcategory = (from a in _PortalContext.Exm_Master_CategoryDMO
                                     from b in _PortalContext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.EMCA_Id == b.EMCA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && emca_idss.Contains(b.EMCA_Id) && b.EYC_ActiveFlg.Equals(true) && a.EMCA_ActiveFlag.Equals(true))

                                     select new HODExamSectionPerformance_DTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName
                                     }).Distinct().OrderBy(t => t.EMCA_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HODExamSectionPerformance_DTO getsubject(HODExamSectionPerformance_DTO data)
        {
            try
            {

                data.subjlist = (from a in _PortalContext.Exm_Category_ClassDMO
                                 from b in _PortalContext.Exm_Yearly_CategoryDMO
                                 from c in _PortalContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _PortalContext.Ch_Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from e in _PortalContext.IVRM_Master_SubjectsDMO
                                 from f in _PortalContext.ExmStudentMarksProcessSubjectwise
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMCL_Id == f.ASMCL_Id && f.ASMAY_Id == a.ASMAY_Id
                                 && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.MI_Id == f.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && c.EME_Id == f.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id && e.ISMS_Id == f.ISMS_Id)
                                 select new HODExamSectionPerformance_DTO
                                 {
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = e.ISMS_SubjectName,

                                 }).Distinct().OrderBy(t => t.ISMS_Id).ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public HODExamSectionPerformance_DTO showreport(HODExamSectionPerformance_DTO data)
        {
            try
            {
                data.seclist = (
                                 from a in _PortalContext.ExmStudentMarksProcessSubjectwise
                                 from b in _PortalContext.School_M_Section
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && a.ASMS_Id == b.ASMS_Id)
                                 select new HODExamSectionPerformance_DTO
                                 {
                                     ASMC_SectionName = b.ASMC_SectionName,
                                     ESTMPS_SectionAverage = a.ESTMPS_SectionAverage,

                                 }
                               ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


    }
}
