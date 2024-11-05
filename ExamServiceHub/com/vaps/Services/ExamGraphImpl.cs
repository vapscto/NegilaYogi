using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamGraphImpl : ExamGraphInterface
    {
        public ExamContext _examctxt;
        public MasterSubjectContext _subctxt;
        public ExamGraphImpl(ExamContext obj, MasterSubjectContext obj1)
        {
            _examctxt = obj;
            _subctxt = obj1;
        }

        public ExamGraphDTO getdetails(ExamGraphDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examctxt.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                data.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _examctxt.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                data.classlist = admlist.ToArray();


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _examctxt.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList();
                data.exmstdlist = esmp.ToArray();

                List<Exm_Master_CategoryDMO> Exm_Master_CategoryDMO = new List<Exm_Master_CategoryDMO>();
                Exm_Master_CategoryDMO = _examctxt.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_ActiveFlag == true).ToList();
                data.Exm_Master_Category = Exm_Master_CategoryDMO.ToArray();

                data.sublist = _examctxt.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).OrderBy(t => t.ISMS_OrderFlag).ToList().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public ExamGraphDTO onreport(ExamGraphDTO data)
        {
            try
            {
                if (data.report_type == "classwise")
                {
                    var getemcaid = _examctxt.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                     && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                    var geteycid = _examctxt.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.ASMAY_Id == data.ASMAY_Id && a.EYC_ActiveFlg == true).Distinct().ToList();

                    List<long> ismsid = new List<long>();

                    var getsubjects = (from a in _examctxt.Exm_Yearly_CategoryDMO
                                       from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                       from c in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from d in _examctxt.IVRM_School_Master_SubjectsDMO
                                       where (a.EYC_Id == b.EYC_Id && b.EYCE_Id == c.EYCE_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true
                                       && c.EYCES_ActiveFlg == true && d.ISMS_Id == c.ISMS_Id && d.ISMS_ActiveFlag == 1 && c.EYCES_AplResultFlg == true
                                       && a.ASMAY_Id == data.ASMAY_Id && b.EME_Id == data.EME_Id && b.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                                       && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id)
                                       select new ExamGraphDTO
                                       {
                                           ISMS_Id = c.ISMS_Id
                                       }).Distinct().ToList();

                    foreach (var c in getsubjects)
                    {
                        ismsid.Add(c.ISMS_Id);
                    }

                    data.datareport = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                       from b in _examctxt.exammasterDMO
                                       from c in _examctxt.IVRM_School_Master_SubjectsDMO
                                       where (a.EME_Id == b.EME_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && ismsid.Contains(a.ISMS_Id) && a.ASMAY_Id==data.ASMAY_Id)
                                       select new ExamGraphDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           ESTMPS_ClassAverage = a.ESTMPS_ClassAverage,
                                           EME_ExamOrder = Convert.ToInt32(c.ISMS_OrderFlag)
                                       }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                }

                else if (data.report_type == "subwise")
                {
                    data.datareport1 = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                        from b in _examctxt.exammasterDMO
                                        from c in _examctxt.School_M_Section
                                        where (a.EME_Id == b.EME_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id && a.EME_Id == data.EME_Id && a.ASMAY_Id==data.ASMAY_Id)
                                        select new ExamGraphDTO
                                        {
                                            ASMC_SectionName = c.ASMC_SectionName,
                                            ESTMPS_SectionAverage = a.ESTMPS_SectionAverage
                                        }).Distinct().ToArray();
                }

                else if (data.report_type == "subwisewithallexam")
                {
                    List<long> emeid = new List<long>();

                    foreach (var c in data.tempexamlist)
                    {
                        emeid.Add(c.EME_Id);
                    }

                    var getclasssection = (from a in _examctxt.AdmissionClass
                                           from b in _examctxt.School_M_Section
                                           from c in _examctxt.Exm_Category_ClassDMO
                                           from d in _examctxt.AcademicYear
                                           from e in _examctxt.Exm_Master_CategoryDMO
                                           where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id
                                           && d.ASMAY_Id == c.ASMAY_Id && c.EMCA_Id == e.EMCA_Id
                                           && a.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                           && c.EMCA_Id == data.EMCA_Id && c.ECAC_ActiveFlag == true
                                           && c.ASMCL_Id == data.ASMCL_Id && c.ASMAY_Id == data.ASMAY_Id)
                                           select b).Distinct().OrderBy(b => b.ASMC_Order).ToArray();

                    data.getclasssection = getclasssection;


                    var get_eycid = _examctxt.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).ToList();


                    var get_exam_list = (from a in _examctxt.Exm_Yearly_Category_ExamsDMO
                                         from b in _examctxt.exammasterDMO
                                         from c in _examctxt.Exm_Yearly_CategoryDMO
                                         where (a.EYC_Id == c.EYC_Id && a.EME_Id == b.EME_Id
                                         && a.EYCE_ActiveFlg == true && c.EYC_ActiveFlg == true
                                         && b.EME_ActiveFlag == true && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id==data.ASMAY_Id
                                         && get_eycid.Contains(a.EYC_Id) && emeid.Contains(a.EME_Id))
                                         select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                    data.get_exam_list = get_exam_list;

                    var get_exam_list_id = _examctxt.Exm_Yearly_Category_ExamsDMO.Where(a => get_eycid.Contains(a.EYC_Id) && emeid.Contains(a.EME_Id)).Select(a => a.EME_Id).ToList();

                    var get_marks_avg = (from a in _examctxt.ExmStudentMarksProcessSubjectwiseDMO
                                         from b in _examctxt.exammasterDMO
                                         from c in _examctxt.Exm_Yearly_Category_ExamsDMO
                                         where (a.EME_Id == b.EME_Id && a.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                         && a.ASMAY_Id == data.ASMAY_Id && get_eycid.Contains(c.EYC_Id)
                                         && get_exam_list_id.Contains(a.EME_Id) && a.ASMCL_Id == data.ASMCL_Id
                                         && a.ISMS_Id == data.ISMS_Id)
                                         select new ExamGraphDTO
                                         {
                                             ESTMPS_SectionAverage = a.ESTMPS_SectionAverage,
                                             ASMS_Id = a.ASMS_Id,
                                             EME_Id = a.EME_Id,
                                             EME_ExamOrder = b.EME_ExamOrder

                                         }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                    data.get_marks_avg = get_marks_avg;
                }

                else if (data.report_type == "studentwiseavg")
                {
                    data.studentwiseavg = (from a in _examctxt.ExmStudentMarksProcessDMO
                                           from b in _examctxt.School_Adm_Y_Student
                                           from c in _examctxt.Adm_M_Student
                                           from d in _examctxt.AdmissionClass
                                           from e in _examctxt.School_M_Section
                                           from f in _examctxt.AcademicYear
                                           where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                           && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id
                                           && b.AMAY_ActiveFlag == 1 && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1)
                                           select new ExamGraphDTO
                                           {
                                               ESTMP_Percentage = a.ESTMP_Percentage,
                                               studentname = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),

                                           }).Distinct().OrderBy(a => a.studentname).ToArray();
                }

                data.instname = _examctxt.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.classteacher = (from a in _examctxt.ClassTeacherMappingDMO
                                     from b in _examctxt.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                     select new VikasaSubjectwiseCumulativeReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         empname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : "  " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : "  " + b.HRME_EmployeeLastName)).Trim(),
                                     }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamGraphDTO OnAcdyear(ExamGraphDTO data)
        {
            try
            {
                data.classlist = (from a in _examctxt.AcademicYear
                                  from b in _examctxt.Exm_Category_ClassDMO
                                  from c in _examctxt.AdmissionClass
                                  where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                  select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamGraphDTO onclasschange(ExamGraphDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.AcademicYear
                                from b in _examctxt.Exm_Category_ClassDMO
                                from c in _examctxt.AdmissionClass
                                from d in _examctxt.School_M_Section
                                where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();

                data.exmstdlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                   from b in _examctxt.Exm_Category_ClassDMO
                                   from c in _examctxt.Exm_Yearly_CategoryDMO
                                   from d in _examctxt.Exm_Yearly_Category_ExamsDMO
                                   from e in _examctxt.AcademicYear
                                   from f in _examctxt.AdmissionClass
                                   from g in _examctxt.School_M_Section
                                   from h in _examctxt.masterexam
                                   where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id
                                   && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                   && h.EME_ActiveFlag == true)
                                   select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.Exm_Master_Category = (from a in _examctxt.Exm_Category_ClassDMO
                                            from b in _examctxt.Exm_Yearly_CategoryDMO
                                            from c in _examctxt.Exm_Master_CategoryDMO
                                            where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                            && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EMCA_ActiveFlag == true)
                                            select c).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamGraphDTO onsectionchange(ExamGraphDTO data)
        {
            try
            {
                data.exmstdlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                   from b in _examctxt.Exm_Category_ClassDMO
                                   from c in _examctxt.Exm_Yearly_CategoryDMO
                                   from d in _examctxt.Exm_Yearly_Category_ExamsDMO
                                   from e in _examctxt.AcademicYear
                                   from f in _examctxt.AdmissionClass
                                   from g in _examctxt.School_M_Section
                                   from h in _examctxt.masterexam
                                   where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id
                                   && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                   && h.EME_ActiveFlag == true)
                                   select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamGraphDTO onchangeexam(ExamGraphDTO data)
        {
            try
            {
                data.sublist = (from a in _examctxt.Exm_Yearly_Category_ExamsDMO
                                from b in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                from c in _examctxt.IVRM_School_Master_SubjectsDMO
                                from d in _examctxt.Exm_Yearly_CategoryDMO
                                from e in _examctxt.Exm_Master_CategoryDMO
                                from f in _examctxt.Exm_Category_ClassDMO
                                where (a.EYC_Id == d.EYC_Id && a.EYCE_Id == b.EYCE_Id && b.ISMS_Id == c.ISMS_Id && d.EMCA_Id == e.EMCA_Id && e.EMCA_Id == f.EMCA_Id
                                && d.EMCA_Id == f.EMCA_Id && a.EYCE_ActiveFlg == true && b.EYCES_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1
                                && d.EYC_ActiveFlg == true && e.EMCA_ActiveFlag == true && f.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id
                                && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id)
                                select c).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamGraphDTO onchangecategory(ExamGraphDTO data)
        {
            try
            {
                data.sublist = (from a in _examctxt.Exm_Yearly_Category_ExamsDMO
                                from b in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                from c in _examctxt.IVRM_School_Master_SubjectsDMO
                                from d in _examctxt.Exm_Yearly_CategoryDMO
                                from e in _examctxt.Exm_Master_CategoryDMO
                                from f in _examctxt.Exm_Category_ClassDMO
                                where (a.EYC_Id == d.EYC_Id && a.EYCE_Id == b.EYCE_Id && b.ISMS_Id == c.ISMS_Id && d.EMCA_Id == e.EMCA_Id && e.EMCA_Id == f.EMCA_Id
                                && d.EMCA_Id == f.EMCA_Id && a.EYCE_ActiveFlg == true && b.EYCES_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1
                                && d.EYC_ActiveFlg == true && e.EMCA_ActiveFlag == true && f.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id
                                && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && d.EMCA_Id == data.EMCA_Id && e.EMCA_Id == data.EMCA_Id
                                && f.EMCA_Id == data.EMCA_Id)
                                select c).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamGraphDTO onchangesubject(ExamGraphDTO data)
        {
            try
            {
                data.getnewexamlist = (from a in _examctxt.Exm_Yearly_Category_ExamsDMO
                                       from b in _examctxt.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from c in _examctxt.IVRM_School_Master_SubjectsDMO
                                       from d in _examctxt.Exm_Yearly_CategoryDMO
                                       from e in _examctxt.Exm_Master_CategoryDMO
                                       from f in _examctxt.Exm_Category_ClassDMO
                                       from g in _examctxt.masterexam
                                       where (a.EYC_Id == d.EYC_Id && a.EYCE_Id == b.EYCE_Id && b.ISMS_Id == c.ISMS_Id
                                       && d.EMCA_Id == e.EMCA_Id && e.EMCA_Id == f.EMCA_Id && a.EME_Id == g.EME_Id
                                       && d.EMCA_Id == f.EMCA_Id && a.EYCE_ActiveFlg == true
                                       && b.EYCES_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1
                                       && d.EYC_ActiveFlg == true && e.EMCA_ActiveFlag == true
                                       && f.ECAC_ActiveFlag == true && d.ASMAY_Id == data.ASMAY_Id
                                       && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id
                                       && d.EMCA_Id == data.EMCA_Id && e.EMCA_Id == data.EMCA_Id
                                       && f.EMCA_Id == data.EMCA_Id && b.ISMS_Id == data.ISMS_Id)
                                       select g).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
