using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class StudentPerformanceReportImpl : StudentPerformanceReportInterface
    {
        public ExamContext _examctxt;
        public StudentPerformanceReportImpl(ExamContext obj)
        {
            _examctxt = obj;
        }


        public StudentPerformanceReportDTO getdetails(StudentPerformanceReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.catlist = (
                                from d in _examctxt.Exm_Master_CategoryDMO
                                where (d.MI_Id == data.MI_Id && d.EMCA_ActiveFlag == true)
                                select d).Distinct().ToArray();

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.studentlist = (from a in _examctxt.Adm_M_Student
                                    from b in _examctxt.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                    select new StudentPerformanceReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim()
                                    }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();

                data.sublist = _examctxt.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public StudentPerformanceReportDTO onselectCategory(StudentPerformanceReportDTO data)
        {
            try
            {
                data.ctlist = (from a in _examctxt.Adm_M_Student
                               from b in _examctxt.School_Adm_Y_StudentDMO
                               from c in _examctxt.AdmissionClass
                               from d in _examctxt.Exm_Category_ClassDMO
                               where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.MI_Id == data.MI_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && data.EMCA_Id == d.EMCA_Id && d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true)
                               select c).Distinct().OrderBy(t=>t.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StudentPerformanceReportDTO onselectclass(StudentPerformanceReportDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.Adm_M_Student
                                from b in _examctxt.School_Adm_Y_StudentDMO
                                from c in _examctxt.School_M_Section
                                from d in _examctxt.Exm_Category_ClassDMO
                                where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && c.MI_Id == data.MI_Id && c.ASMS_Id == b.ASMS_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == b.ASMCL_Id && d.ECAC_ActiveFlag == true && c.ASMS_Id == d.ASMS_Id && data.EMCA_Id == d.EMCA_Id)
                                select c).Distinct().OrderBy(t=>t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StudentPerformanceReportDTO onselectSection(StudentPerformanceReportDTO data)
        {
            try
            {
                var EMCA_Id = _examctxt.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;
                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.studentlist = (from a in _examctxt.Adm_M_Student
                                    from b in _examctxt.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                    select new StudentPerformanceReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim()
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StudentPerformanceReportDTO onshow(StudentPerformanceReportDTO data)
        {
            try
            {
                data.showgraph = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id)
                                select new StudentPerformanceReportDTO
                                {
                                    ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                    ESTMPS_ClassHighest = a.ESTMPS_ClassHighest,
                                    ESTMPS_SectionHighest = a.ESTMPS_SectionHighest
                                }
                                ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
