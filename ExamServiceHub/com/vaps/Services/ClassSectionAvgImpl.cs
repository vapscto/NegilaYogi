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
    public class ClassSectionAvgImpl : ClassSectionAvgInterface
    {
        public ExamContext _examctxt;
        public ClassSectionAvgImpl(ExamContext obj)
        {
            _examctxt = obj;
        }

        public ClassSectionAvgDTO getdetails(ClassSectionAvgDTO data)
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

                data.examlist = _examctxt.masterexam.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(t=>t.EME_ExamOrder).ToArray();

                data.sublist = _examctxt.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).OrderBy(t=>t.ISMS_OrderFlag).ToList().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public ClassSectionAvgDTO onselectCategory(ClassSectionAvgDTO data)
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

        public ClassSectionAvgDTO onselectclass(ClassSectionAvgDTO data)
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


        public ClassSectionAvgDTO onreport(ClassSectionAvgDTO data)
        {
            try
            {
                if (data.report_type == "all")
                {
                    if (data.check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from b in _examctxt.AdmissionClass
                                           from c in _examctxt.AcademicYear
                                           from d in _examctxt.exammasterDMO
                                           from e in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.EME_Id == d.EME_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id )
                                           select new ClassSectionAvgDTO
                                           {
                                               ESTMPS_ClassAverage = a.ESTMPS_ClassAverage,
                                               ESTMPS_SectionAverage = a.ESTMPS_SectionAverage
                                           }
                                    ).Distinct().ToArray();
                    }
                    else
                    {

                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from b in _examctxt.AdmissionClass
                                           from c in _examctxt.AcademicYear
                                           from d in _examctxt.exammasterDMO
                                           from e in _examctxt.IVRM_School_Master_SubjectsDMO
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.EME_Id == d.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id)
                                           select new ClassSectionAvgDTO
                                           {
                                               ESTMPS_ClassAverage = a.ESTMPS_ClassAverage,
                                               ESTMPS_SectionAverage = a.ESTMPS_SectionAverage
                                           }
                                    ).Distinct().ToArray();
                    }
                }
                else if(data.report_type == "individual")
                {
                    if (data.check_type == "1")
                    {
                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from b in _examctxt.AdmissionClass
                                           from c in _examctxt.AcademicYear
                                           from d in _examctxt.exammasterDMO
                                           from e in _examctxt.IVRM_School_Master_SubjectsDMO
                                           from f in _examctxt.School_M_Section
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.EME_Id == d.EME_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && f.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ClassSectionAvgDTO
                                           {
                                               ESTMPS_ClassAverage = a.ESTMPS_ClassAverage,
                                               ESTMPS_SectionAverage = a.ESTMPS_SectionAverage
                                           }
                                    ).Distinct().ToArray();
                    }
                    else
                    {

                        data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                           from b in _examctxt.AdmissionClass
                                           from c in _examctxt.AcademicYear
                                           from d in _examctxt.exammasterDMO
                                           from e in _examctxt.IVRM_School_Master_SubjectsDMO
                                           from f in _examctxt.School_M_Section
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.EME_Id == d.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && f.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                           select new ClassSectionAvgDTO
                                           {
                                               ESTMPS_ClassAverage = a.ESTMPS_ClassAverage,
                                               ESTMPS_SectionAverage = a.ESTMPS_SectionAverage
                                           }
                                    ).Distinct().ToArray();
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


    }
}
